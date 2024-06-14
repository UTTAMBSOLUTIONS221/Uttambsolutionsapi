using Dapper;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
