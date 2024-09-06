using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }

            //sera una lista de Guid que contendrá los instructores
            public List<Guid> ListaInstructor { get; set; }
        }

        //aplicamos validaciones con FluentValidation
        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }

        //usamos mediaTR para manejar la lógica de negocio
        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
                
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //generamos un nuevo Guid para el curso
                Guid _cursoId = Guid.NewGuid();

                var curso = new Curso
                {
                    CursoId = _cursoId,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion
                };
                _context.Curso.Add(curso);

                //agregamos los instructores al curso
                if(request.ListaInstructor != null)
                {
                    foreach (var id in request.ListaInstructor)
                    {
                        var cursoInstructor = new CursoInstructor
                        {
                            CursoId = _cursoId,
                            InstructorId = id
                        };
                        _context.CursoInstructor.Add(cursoInstructor);
                    }
                }   

                var valor = await _context.SaveChangesAsync();
                if(valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el curso");
            }
        }
    }
}
