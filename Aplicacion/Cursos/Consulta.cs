using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDto>> 
        {
        }
        public class Manejador : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            //Inyección de dependencias para el contexto de la base de datos
            private readonly CursosOnlineContext _context;
            private readonly IMapper _mapper;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await _context.Curso
                    .Include(x=>x.InstructoresLink)
                    .ThenInclude(x=>x.Instructor)
                    .ToListAsync();

                //Mapeo de la lista de cursos a una lista de CursoDto
                var cursosDto = _mapper.Map<List<Curso>, List<CursoDto>>(cursos);

                return cursosDto;
            }
        }


    }
}
