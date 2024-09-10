using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest
        {
            public Guid CursoId { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            
            public List<Guid> ListaInstructor { get; set; }

            public decimal? Precio { get; set; }

            public decimal? Promocion { get; set; }
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


        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext _context;
            public Manejador(CursosOnlineContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Actualizar curso
                var curso = await _context.Curso.FindAsync(request.CursoId);
                if(curso == null)
                {
                    //throw new Exception("No se encontro el curso");

                    //implementamos el manejador de excepciones personalizado
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });

                }
                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;

                //Actualizar precio del curso
                var precioEntidad = _context.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if(precioEntidad != null)
                {
                    precioEntidad.PrecioActual = request.Precio?? precioEntidad.PrecioActual;
                    precioEntidad.Promocion = request.Promocion?? precioEntidad.Promocion;
                }
                else
                {
                    precioEntidad = new Precio
                    {
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio?? 0,
                        Promocion = request.Promocion?? 0,
                        CursoId = curso.CursoId
                    };
                    await _context.Precio.AddAsync(precioEntidad);
                }
               

            
                //Actualizar instructores del curso
                if(request.ListaInstructor != null)
                {
                    if(request.ListaInstructor.Count > 0)
                    {
                        //Eliminar instructores actuales 
                        var instructoresDB = _context.CursoInstructor.Where(x => x.CursoId == request.CursoId);
                        foreach (var instructorEliminar in instructoresDB)
                        {
                            _context.CursoInstructor.Remove(instructorEliminar);
                        }

                        //Agregar los nuevos instructores
                        foreach (var id in request.ListaInstructor)
                        {
                            _context.CursoInstructor.Add(new CursoInstructor
                            {
                                CursoId = request.CursoId,
                                InstructorId = id
                            });
                        }
                        
                    }   
                }   

                var resultado = await  _context.SaveChangesAsync();
                if(resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se guardaron los cambios en el curso");
            }
        }   
    }
}
