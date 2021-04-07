using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EF.Essentials.Encryption
{
    public interface IUserTokenFactory
    {
        string GenerateJwtToken(ClaimsPrincipal principal, string issuer = "GenericCompany");
    }

    public class UserTokenFactory : IUserTokenFactory
    {
        private static int ExpiryMinutes = 15;
        private readonly ISignatureKeyContainer _keyContainer;
        private readonly IdentityOptions _identityOptions;

        public UserTokenFactory(ISignatureKeyContainer keyContainer, IOptions<IdentityOptions> identityOptions)
        {
            _keyContainer = keyContainer;
            _identityOptions = identityOptions.Value;
        }

        public string GenerateJwtToken(ClaimsPrincipal principal, string issuer = "GenericCompany")
        {
            var securityStampClaimType = _identityOptions.ClaimsIdentity.SecurityStampClaimType;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = issuer,
                Subject = new ClaimsIdentity(principal.Claims.Where(x => x.Type != securityStampClaimType)),
                Expires = DateTime.UtcNow.AddMinutes(ExpiryMinutes),
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(_keyContainer.Key.PrivateKey)
                    {KeyId = _keyContainer.Key.KeyId}, SecurityAlgorithms.RsaSha512)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}
