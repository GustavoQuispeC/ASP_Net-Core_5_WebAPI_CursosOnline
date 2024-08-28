
using System;

namespace Dominio
{
    public class CursoInstructor
    {
        public Guid CursoId { get; set; }

        //ancla a la tabla Curso; relación uno a uno
        public Curso Curso { get; set; }
        

        public Guid InstructorId { get; set; }

        //ancla a la tabla Instructor; relación uno a uno
        public Instructor Instructor { get; set; }
        
    }
}
