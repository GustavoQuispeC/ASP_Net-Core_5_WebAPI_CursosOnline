using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            //Contructor : Inyección de dependencias para el contexto de la base de datos
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

          

            public async Task<CursoDto>Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await _context.Curso
                    .Include(x => x.InstructoresLink)
                    .ThenInclude(x => x.Instructor)
                    .FirstOrDefaultAsync(a => a.CursoId == request.Id);

                if (curso == null)
                {
                    //throw new Exception("No se encontro el curso");

                    //implementamos el manejador de excepciones personalizado
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }
                var cursoDto = _mapper.Map<Curso, CursoDto>(curso);

                return cursoDto;
            }
        }
    }
}
