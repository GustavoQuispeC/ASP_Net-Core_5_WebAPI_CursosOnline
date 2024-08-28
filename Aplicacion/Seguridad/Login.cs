using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<Usuario>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        //validaciones para el login
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        //manejador para el login del usuario
        public class Manejador : IRequestHandler<Ejecuta, Usuario>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;

            //inyeccion de dependencias para el login
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            //metodo para el login del usuario con validaciones
            public async Task<Usuario> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new { mensaje = "El usuario no existe" });

                }
                
                 var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                if (resultado.Succeeded)
                {
                    return usuario;
                }
                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized, new { mensaje = "Contraseña incorrecta" });
                
            }
        }
       
    }
}
