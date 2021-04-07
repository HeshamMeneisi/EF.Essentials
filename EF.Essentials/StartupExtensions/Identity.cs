using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
using EF.Essentials.Config;
using EF.Essentials.Encryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PemUtils;
using Serilog;

namespace EF.Essentials.StartupExtensions
{
    public static class Identity
    {
        public static void ConfigureIdentityServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthorization();
            serviceCollection.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = false;
                    var provider = serviceCollection.BuildServiceProvider();
                    config.TokenValidationParameters = CreateTokenValidationParameters(
                        provider.GetRequiredService<ISignatureKeyResolver>(),
                        provider.GetRequiredService<IOptions<ServiceConfig>>().Value.KeyStore);
                });
            serviceCollection.Configure<IdentityOptions>(ConfigureIdentityOptions);
        }

        private static TokenValidationParameters CreateTokenValidationParameters(ISignatureKeyResolver keyResolver, KeyStoreConfig keyStoreConfig)
        {
            if(keyStoreConfig.Bypass){
                Log.Fatal("!!!!!! Bypassing security signature, THIS CANNOT RUN ON PRODUCTION");
                // This allows us to reproduce production scenarios and step into the code using a legit JWT without
                // exposing the key service publicly
                return new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidIssuer = "GenericCompany",
                    ValidateAudience = false,
                    ValidAudiences =new []{"GenericCompany"},
                    ValidateIssuerSigningKey = false,
                    SignatureValidator = (token, parameters) => new JwtSecurityToken(token),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireSignedTokens = false
                };
            }

            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateActor = true,
                ValidateAudience = true,
                ValidIssuer = "GenericCompany",
                ValidAudiences = new[] {"GenericCompany"},
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                {
                    // todo: I know this .Result is a very bad idea (converting from async to sync)
                    // however there's no other way to do this, signing key resolver doesn't have a
                    // async version of this method, they are looking into it though
                    // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/468
                    var key = keyResolver.ResolveKey(kid).Result;
                    var pemReader = new PemReader(new MemoryStream(Encoding.UTF8.GetBytes(key)));
                    var publicKeyParameters = pemReader.ReadRsaKey();
                    return new[] {new RsaSecurityKey(publicKeyParameters)};
                }
            };
        }

        private static void ConfigureIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 3;

            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._";
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = false;
        }
    }
}
