using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shop.Models;
using Microsoft.IdentityModel.Tokens;

namespace Shop.Services
{
    public static class TokenService
    {
        public static string GenerateToken(User user)
        {
            
            var tokenHandler = new JwtSecurityTokenHandler();
            //usando a chave do token
            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            //descrição doque vai ter no token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
               }),
                Expires = DateTime.UtcNow.AddHours(2),

                //SecurityAlgorithms.HmacSha256Signature encripta a chave
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //cria o token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //gera a string do token
            return tokenHandler.WriteToken(token);
        }
    }
}