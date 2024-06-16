﻿using Dapper;
using DBL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DBL.Repositories
{
    public class AuthRepository:BaseRepository,IAuthRepository
    {
        public AuthRepository(string connectionString) : base(connectionString)
        {
        }
        #region Verify System Staff
        public UsermodelResponce VerifySystemStaff(string Username)
        {

            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                UsermodelResponce resp = new UsermodelResponce();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Username", Username);
                parameters.Add("@StaffDetails", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Everestfcsverifysystemuser", parameters, commandType: CommandType.StoredProcedure);
                string staffDetailsJson = parameters.Get<string>("@StaffDetails");
                JObject responseJson = JObject.Parse(staffDetailsJson);
                if (Convert.ToInt32(responseJson["RespStatus"]) == 0)
                {
                    string userModelJson = responseJson["Usermodel"].ToString();
                    UsermodeldataResponce userResponse = JsonConvert.DeserializeObject<UsermodeldataResponce>(userModelJson);
                    resp.RespStatus = Convert.ToInt32(responseJson["RespStatus"]);
                    resp.RespMessage = responseJson["RespMessage"].ToString();
                    resp.Usermodel = userResponse;
                    return resp;
                }
                else
                {
                    resp.RespStatus = Convert.ToInt32(responseJson["RespStatus"]);
                    resp.RespMessage = responseJson["RespMessage"].ToString();
                    resp.Usermodel = new UsermodeldataResponce();
                    return resp;
                }
            }
        }
        #endregion
    }
}
