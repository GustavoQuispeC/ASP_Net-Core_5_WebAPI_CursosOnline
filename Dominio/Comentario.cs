

using System;

namespace Dominio
{
    public class Comentario
    {
        public Guid ComentarioId { get; set; }
        public string Alumno { get; set; }
        public int Puntaje { get; set; }
        public string ComentarioTexto { get; set; }
        public Guid CursoId { get; set; }

        //ancla a la tabla Curso; relación uno a uno
        public Curso Curso { get; set; }

       
        
    }
}
