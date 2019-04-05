using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using ET;

namespace DAL
{
    public class AppDirectoryDAL
    {
        // Application Profile by Role
        public List<AppDirectory> AppProfilebyRole(int RoleID)
        {
            var Profile = new List<AppDirectory>();

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[adm].[uspReadProfilebyRole]", SqlCon);
                    SqlCmd.CommandType = CommandType.StoredProcedure;

                    //Insert Parameters 
                    SqlParameter ParMainAppName = new SqlParameter
                    {
                        ParameterName = "@MainAppName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = "MMOA"
                    };
                    SqlCmd.Parameters.Add(ParMainAppName);

                    SqlParameter ParRoleID = new SqlParameter
                    {
                        ParameterName = "@RoleID",
                        SqlDbType = SqlDbType.Int,
                        Value = RoleID
                    };
                    SqlCmd.Parameters.Add(ParRoleID);

                    using (var dr = SqlCmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var app = new AppDirectory
                            {
                                ApplicationID = Convert.ToInt32(dr["ApplicationID"]),
                                RARProfileID = Convert.ToInt32(dr["ProfileID"]),
                                MainClass = dr["MainClass"].ToString(),
                                AppName = dr["AppName"].ToString(),
                                Status = Convert.ToBoolean(dr["Status"])
                            };

                            Profile.Add(app);

                        }
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return Profile;
        }

        // Application Profile by User
        public List<AppDirectory> AppProfilebyUser(string UserName)
        {
            var Profile = new List<AppDirectory>();

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[adm].[uspReadAppDirectorybyUser]", SqlCon);
                    SqlCmd.CommandType = CommandType.StoredProcedure;

                    //Insert Parameters 
                    SqlParameter ParMainAppName = new SqlParameter
                    {
                        ParameterName = "@MainAppName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = "MMOA"
                    };
                    SqlCmd.Parameters.Add(ParMainAppName);

                    SqlParameter ParUserName = new SqlParameter
                    {
                        ParameterName = "@UserName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = UserName
                    };
                    SqlCmd.Parameters.Add(ParUserName);

                    using (var dr = SqlCmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var app = new AppDirectory
                            {
                                RARProfileID = Convert.ToInt32(dr["RARProfileID"]),
                                MainClass = dr["MainClass"].ToString(),
                                AppName = dr["AppName"].ToString(),
                                Controller = dr["Controller"].ToString(),
                                ViewPage = dr["ViewPage"].ToString(),
                                Parameter = dr["Parameter"].ToString()
                            };

                            Profile.Add(app);

                        }
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Profile;
        }

        
        // Add and Update App Profiles
        public bool UpdateAppProfile (AppDirectory App, int RoleID, string InsertUser)
        {
            bool rpta = false;

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand(null, SqlCon);

                    if(App.RARProfileID == 0)
                    {
                        SqlCmd = new SqlCommand("[adm].[uspInsertRolesApplicationRights]", SqlCon);
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        //Insert Parameters
                        SqlParameter ParInsertUser = new SqlParameter
                        {
                            ParameterName = "@InsertUser",
                            SqlDbType = SqlDbType.VarChar,
                            Size = 50,
                            Value = InsertUser
                        };
                        SqlCmd.Parameters.Add(ParInsertUser);

                        SqlParameter ParRoleID = new SqlParameter
                        {
                            ParameterName = "@RoleID",
                            SqlDbType = SqlDbType.Int,
                            Value = RoleID
                        };
                        SqlCmd.Parameters.Add(ParRoleID);

                        SqlParameter ParAppID = new SqlParameter
                        {
                            ParameterName = "@AppID",
                            SqlDbType = SqlDbType.Int,
                            Value = App.ApplicationID
                        };
                        SqlCmd.Parameters.Add(ParAppID);
                    }
                    else
                    {
                        SqlCmd = new SqlCommand("[adm].[uspUpdateRolesApplicationRights]", SqlCon);
                        SqlCmd.CommandType = CommandType.StoredProcedure;

                        //Insert Parameters
                        SqlParameter ParInsertUser = new SqlParameter
                        {
                            ParameterName = "@InsertUser",
                            SqlDbType = SqlDbType.VarChar,
                            Size = 50,
                            Value = InsertUser
                        };
                        SqlCmd.Parameters.Add(ParInsertUser);

                        SqlParameter ParRARProfileID = new SqlParameter
                        {
                            ParameterName = "@RARProfileID",
                            SqlDbType = SqlDbType.Int,
                            Value = App.RARProfileID
                        };
                        SqlCmd.Parameters.Add(ParRARProfileID);

                        SqlParameter ParStatus = new SqlParameter
                        {
                            ParameterName = "@Status",
                            SqlDbType = SqlDbType.Bit,
                            Value = App.Status
                        };
                        SqlCmd.Parameters.Add(ParStatus);  
                    }

                    //Exec Command
                    SqlCmd.ExecuteNonQuery();

                    rpta = true;

                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return rpta;
        }
    }
}
