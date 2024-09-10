using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid Id { get; set; }
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
                // Eliminar relaciones de la tabla intermedia
                var instructoresDB = _context.CursoInstructor.Where(x => x.CursoId == request.Id);
                foreach (var instructor in instructoresDB)
                {
                    _context.CursoInstructor.Remove(instructor);
                }

                // Eliminar el curso de la tabla Curso
                var curso = await _context.Curso.FindAsync(request.Id);
                if (curso == null)
                {
                    //throw new Exception("No se encontro el curso");|


                    //implementamos el manejador de excepciones personalizado
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new { mensaje = "No se encontro el curso" });
                }
                _context.Remove(curso);

                var resultado = await _context.SaveChangesAsync();
                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo eliminar el curso");

            }
        }
    }
}
