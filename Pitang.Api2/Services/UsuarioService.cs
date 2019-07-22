using Pitang.Api2.Context;
using Pitang.Api2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitang.Api2.Services
{
    public class UsuarioService
    {
        private appDbContext _context;

        public UsuarioService(appDbContext context)
        {
            _context = context;
        }

        public bool UsuarioAdd(Usuario toUsuario)
        {
            try
            {
                _context.Usuarios.Add(toUsuario);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public Usuario UsuarioGetEmail(string tcEmail)
        {
            return _context.Usuarios.Where(
                u => u.email == tcEmail).FirstOrDefault();
        }

        public UsuarioResponse UsuarioGetLogIn(string tcEmail, string tcPassword)
        {
            Usuario loUsuario = _context.Usuarios.Where(
                u => u.email == tcEmail && u.password == tcPassword).FirstOrDefault();
            if(loUsuario != null)
            {
                return new UsuarioResponse()
                {
                    Id = loUsuario.Id,
                    firstName = loUsuario.firstName,
                    lastName = loUsuario.lastName,
                    email = loUsuario.email,
                    phones = loUsuario.phones

                };

            }
            else
            {
                return null;
            }
        }
    }
}
