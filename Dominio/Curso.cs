using System;
using System.Collections.Generic;


namespace Dominio
{
    public class Curso
    {
        public Guid CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public byte[] FotoPortada { get; set; }

        //ancla a la tabla Precio; relación uno a uno
        public Precio PrecioPromocion { get; set; }

        //ancla a la tabla Comentario; relación uno a muchos
        public ICollection<Comentario> ComentarioLista { get; set; }

        //ancla a la tabla CursoInstructor; relación muchos a muchos
        public ICollection<CursoInstructor> InstructoresLink { get; set; }
    }
}
