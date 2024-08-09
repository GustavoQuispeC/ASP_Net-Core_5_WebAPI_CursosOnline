

namespace Dominio
{
    public class Comentario
    {
        public int ComentarioId { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }
        public int CursoId { get; set; }

        //ancla a la tabla Curso; relación uno a uno
        public Curso Curso { get; set; }

       
        
    }
}
