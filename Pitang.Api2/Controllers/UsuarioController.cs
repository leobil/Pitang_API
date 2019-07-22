using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pitang.Api2.Context;
using Pitang.Api2.Models;
using Pitang.Api2.Services;

namespace Pitang.Api2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _loUsuarioService;

        public UsuarioController(UsuarioService toUsuarioService)
        {
            _loUsuarioService = toUsuarioService;
        }

        [HttpPost]
        [Route("signup")]
        [Authorize("Bearer")]
        public ActionResult<Usuario> setUsuario([FromBody] Usuario toUsuario)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(new Error { message = this.ModelState.ToString(), errorCode = 400 });
                }
                var loUsuario = _loUsuarioService.UsuarioGetEmail(toUsuario.email);
                if (loUsuario != null)
                    return this.Conflict(new Error { message = "E-mail already exists", errorCode = 409 });

                if (string.IsNullOrWhiteSpace(toUsuario.firstName) ||
                   string.IsNullOrWhiteSpace(toUsuario.lastName) ||
                   string.IsNullOrWhiteSpace(toUsuario.email) ||
                   string.IsNullOrWhiteSpace(toUsuario.password) ||
                   toUsuario.phones == null)
                    return this.Conflict(new Error { message = "Missing fields", errorCode = 409 });

               
                if (_loUsuarioService.UsuarioAdd(toUsuario))
                {
                    return this.Ok("ok. cadastrado");
                }
                else
                {
                    return this.Conflict(new Error { message = "Missing fields", errorCode = 409 });
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(new Error() { errorCode = 400, message = e.Message});
            }


        }

        [HttpPost]
        [Route("me")]
        [Authorize("Bearer")]
        public ActionResult<Usuario> getUsuario(string tcAuthorization)
        {

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(new Error { message = this.ModelState.ToString(), errorCode = 400 });
            }

            var loToken = Request.Headers["Authorization"];
            


            return null;
        }
        

    }
}