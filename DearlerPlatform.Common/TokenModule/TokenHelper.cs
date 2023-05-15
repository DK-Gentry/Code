using DearlerPlatform.Common.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Common.TokenModule
{
    /// <summary>
    ///生成jwt字符串的类
    /// </summary>
    public static class TokenHelper
    {
        /// <summary>
        /// 创建一个jwt字符
        /// </summary>
        /// <param name="jwtTokenModel"></param>
        /// <returns></returns>
        public static string CreateToken(JwtTokenModel jwtTokenModel)
        {
            var claims = new[]
            {
                new Claim("Id",jwtTokenModel.Id.ToString()),
                new Claim("CustomerNo",jwtTokenModel.CustomerNo),
                new Claim("CustomerName",jwtTokenModel.CustomerName)
            };
            //生成密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenModel.Security));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:jwtTokenModel.Issuer,
                audience: jwtTokenModel.Audience,
                expires: DateTime.Now.AddMinutes(jwtTokenModel.Expires),
                signingCredentials:creds,
                claims:claims
                );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }
    }
}
