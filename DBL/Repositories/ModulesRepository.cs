using Dapper;
using DBL.Models;
using System.Data.SqlClient;
using System.Data;
using DBL.Entities;

namespace DBL.Repositories
{
    public class ModulesRepository:BaseRepository, IModulesRepository
    {
        public ModulesRepository(string connectionString) : base(connectionString)
        {
        }
        public IEnumerable<Systemmodule> Getsystemmoduledata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Systemmodule>("Usp_Getsystemmoduledata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
