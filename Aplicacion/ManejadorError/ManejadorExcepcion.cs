using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacion.ManejadorError
{
    public class ManejadorExcepcion : Exception
    {
        public HttpStatusCode Codigo { get; }
        public object Errores { get; }

        public ManejadorExcepcion(HttpStatusCode codigo, object errores)
        {
            Codigo = codigo;
            Errores = errores;
        }
    }
}
