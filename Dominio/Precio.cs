

namespace Dominio
{
    public class Precio
    {
        public int PrecioId { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal Promocion { get; set; }
        public int CursoId { get; set; }


        //ancla a la tabla Curso; relación uno a uno
        public Curso Curso { get; set; }


    }
}
