using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [AllowAnonymous] //permite que cualquier usuario pueda acceder a este controlador

    public class UsuarioController : MiControllerBase
    {
        //https://localhost:5001/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioData>> Login([FromBody] Login.Ejecuta parametros)
        {
            return await Mediator.Send(parametros);
        }
    }
}
