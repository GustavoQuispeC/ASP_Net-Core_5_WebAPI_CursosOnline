
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    // https://localhost:5001/api/Instructor
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : MiControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores()
        {
           return await Mediator.Send(new Consulta.Lista());
        }
      
    }
}
