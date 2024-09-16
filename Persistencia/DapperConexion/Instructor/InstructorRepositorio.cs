using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection _factoryConnection;

        public InstructorRepositorio(IFactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public Task<int> Actualizar(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public Task<int> Eliminar(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Nuevo(InstructorModel parametros)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storeProcedure = "usp_Obtener_Instructores";

            try
            {
                var connection = _factoryConnection.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType:
                    CommandType.StoredProcedure);
                
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo encontrar los instructores", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return instructorList;


        }

        public Task<InstructorModel> ObtenerPorId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
