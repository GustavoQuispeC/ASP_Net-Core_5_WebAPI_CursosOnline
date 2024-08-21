using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dominio;
using Microsoft.AspNetCore.Mvc;
using Persistencia;


namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CursosOnlineContext context;
        public WeatherForecastController(CursosOnlineContext _context)
        {
            context = _context;
        }
        [HttpGet]
        public IEnumerable<Curso> Get()
        {
           return context.Curso.ToList();
        }
    }
}
