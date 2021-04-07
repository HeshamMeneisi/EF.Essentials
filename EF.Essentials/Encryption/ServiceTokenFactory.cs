using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using EF.Essentials.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EF.Essentials.Encryption
{
    public interface IServiceTokenFactory
    {
        string GenerateJwtToken(string issuer = "GenericCompany");
    }

    public class ServiceTokenFactory : IServiceTokenFactory
    {
        private static string ServiceName = Assembly.GetExecutingAssembly().FullName;

        private readonly ISignatureKeyContainer _keyContainer;

        private string _cachedToken;
        private DateTime _cacheTime;
        private SecurityConfig _config;

        public ServiceTokenFactory(ISignatureKeyContainer keyContainer, IOptions<SecurityConfig> config)
        {
            _config = config.Value;
            _keyContainer = keyContainer;
        }

        public string GenerateJwtToken(string issuer = "GenericCompany")
        {
            // This is not fully secure, services should authenticate from an auth service
            if (!string.IsNullOrEmpty(_cachedToken) && (DateTime.UtcNow - _cacheTime).TotalSeconds
                < _config.TokenTtl - Math.Round(_config.TokenTtl * 0.1))
                return _cachedToken;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = issuer,
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("IsService", ServiceName),
                }),
                Expires = DateTime.UtcNow.AddSeconds(_config.TokenTtl),
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(_keyContainer.Key.PrivateKey)
                    {KeyId = _keyContainer.Key.KeyId}, SecurityAlgorithms.RsaSha512)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            _cacheTime = DateTime.UtcNow;
            return _cachedToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
