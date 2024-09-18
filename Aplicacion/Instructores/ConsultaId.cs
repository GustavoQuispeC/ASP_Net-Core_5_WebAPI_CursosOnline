using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Persistencia.DapperConexion.Instructor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.Instructores
{
    public class ConsultaId
    {
        public class InstructorUnico : IRequest<InstructorModel>
        {
            public Guid Id { get; set; }
        }

        public class Manejador : IRequestHandler<InstructorUnico, InstructorModel>
        {
            private readonly IInstructor _instructorRepository;

            public Manejador(IInstructor instructorRepository)
            {
                _instructorRepository = instructorRepository;
            }

            public async Task<InstructorModel> Handle(InstructorUnico request, System.Threading.CancellationToken cancellationToken)
            {
                var result = await _instructorRepository.ObtenerPorId(request.Id);
                return result;
            }
        }
    }
}
