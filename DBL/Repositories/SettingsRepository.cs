using Dapper;
using DBL.Models;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        public SettingsRepository(string connectionString) : base(connectionString)
        {
        }

        #region Communication Templates
        public CommunicationTemplateModel Getsystemcommunicationtemplatedatabyname(bool Isemail, string Templatename)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Isemail", Isemail);
                parameters.Add("@Templatename", Templatename);
                return connection.Query<CommunicationTemplateModel>("Usp_Getcommunicationtemplatedbyname", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion
    }
}
