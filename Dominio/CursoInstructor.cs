
namespace Dominio
{
    public class CursoInstructor
    {
        public int CursoId { get; set; }

        //ancla a la tabla Curso; relación uno a uno
        public Curso Curso { get; set; }
        

        public int InstructorId { get; set; }

        //ancla a la tabla Instructor; relación uno a uno
        public Instructor Instructor { get; set; }
        
    }
}
