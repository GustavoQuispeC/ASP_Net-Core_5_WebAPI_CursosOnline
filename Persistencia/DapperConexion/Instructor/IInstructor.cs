﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public interface IInstructor
    {
        Task<IEnumerable<InstructorModel>> ObtenerLista();

        Task<InstructorModel> ObtenerPorId(Guid id);

        Task<int>Nuevo(string nombre, string apellidos, string grado);

        Task<int> Actualizar(Guid instructorId, string nombre, string apellidos, string grado);
        Task<int> Eliminar(Guid id);


    }
}
