using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pitang.Api2.Models;
using Pitang.Api2.Services;

namespace Pitang.Api2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        [AllowAnonymous]
        [Route("signin")]
        [HttpPost]
        public object Post([FromBody] Usuario toUsuario
            ,[FromServices] UsuarioService toUsuarioService
            ,[FromServices] SigingConfigurations toSigingConfigurations
            ,[FromServices] TokenConfigurations toTokenConfigurations)
        {

            if (string.IsNullOrWhiteSpace(toUsuario.email) || string.IsNullOrWhiteSpace(toUsuario.password))
                return this.Conflict(new Error { message = "Missing fields", errorCode = 409 });

            bool lbCredeciaisValidas = false;
            if(toUsuario != null && !String.IsNullOrWhiteSpace(toUsuario.email) && !String.IsNullOrWhiteSpace(toUsuario.password))
            {
                UsuarioResponse loUsuarioResponse = toUsuarioService.UsuarioGetLogIn(toUsuario.email, toUsuario.password);
                if (loUsuarioResponse == null)
                    return this.Conflict(new Error { message = "Invalid e-mail or password", errorCode = 409 });

                lbCredeciaisValidas = true;
            }

            if (lbCredeciaisValidas)
            {
                ClaimsIdentity loIdentity = new ClaimsIdentity(
                        new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                            new Claim(JwtRegisteredClaimNames.UniqueName, toUsuario.email)
                        }
                    );

                DateTime ldtDataCriacao = DateTime.Now;
                DateTime ldtDataExpira = ldtDataCriacao + TimeSpan.FromSeconds(toTokenConfigurations.Seconds);

                var loHandler = new JwtSecurityTokenHandler();
                
                var loSecurityToken = loHandler.CreateToken(new SecurityTokenDescriptor {
                    Issuer = toTokenConfigurations.Issuer,
                    Audience = toTokenConfigurations.Audience,
                    SigningCredentials = toSigingConfigurations._SigningCredentials,
                    Subject = loIdentity,
                    NotBefore = ldtDataCriacao,
                    Expires = ldtDataExpira
                });

                var loToken = loHandler.WriteToken(loSecurityToken);

                return new
                {
                    authenticated = true,
                    Created = ldtDataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = ldtDataExpira.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = loToken,
                    message = "Ok"
                };

            }
            else
            {
                return new Error
                {
                    errorCode = 403,
                    message = "Falha ao autenticar"
                };
            }
        }
    }
}