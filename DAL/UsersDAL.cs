﻿using System;
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
    public class UsersDAL
    {
        public int Login (string UserName, string Password)
        {
            int role = 0;

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[adm].[uspLogin]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParUserName = new SqlParameter
                    {
                        ParameterName = "@UserName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = UserName
                    };
                    SqlCmd.Parameters.Add(ParUserName);

                    SqlParameter ParPassword = new SqlParameter
                    {
                        ParameterName = "@Password",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = Password
                    };
                    SqlCmd.Parameters.Add(ParPassword);

                    //EXEC Command
                    role = Convert.ToInt32(SqlCmd.ExecuteScalar());

                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return role;
        }

        public AuthorizationCode AuthCode(string Email)
        {
            AuthorizationCode code = new AuthorizationCode();

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[usr].[uspGenerateGUIDResetPassword]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParEmail = new SqlParameter
                    {
                        ParameterName = "@Email",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = Email
                    };
                    SqlCmd.Parameters.Add(ParEmail);

                    using (var dr = SqlCmd.ExecuteReader())
                    {
                        dr.Read();
                        if(dr.HasRows)
                        {
                            code.GUID = dr["GUID"].ToString();
                            code.UserID = Convert.ToInt32(dr["UserID"]);
                            code.FullName = dr["FullName"].ToString();
                        }                                             
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return code;
        }

        public int ValidateGUID(string GUID)
        {
            int ValidCode = 0;

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[usr].[uspValidateGUIDResetPassword]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParGUID = new SqlParameter
                    {
                        ParameterName = "@GUID",
                        SqlDbType = SqlDbType.VarChar,
                        Value = GUID
                    };
                    SqlCmd.Parameters.Add(ParGUID);

                    //EXEC Command
                    ValidCode = Convert.ToInt32(SqlCmd.ExecuteScalar());

                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ValidCode;
        }

        public bool ResetPassword(string GUID, string Password)
        {
            bool rpta = false;
            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[usr].[uspResetPassword]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParGUID = new SqlParameter
                    {
                        ParameterName = "@GUID",
                        SqlDbType = SqlDbType.VarChar,
                        Value = GUID
                    };
                    SqlCmd.Parameters.Add(ParGUID);

                    SqlParameter ParPassword = new SqlParameter
                    {
                        ParameterName = "@Password",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = Password
                    };
                    SqlCmd.Parameters.Add(ParPassword);

                    //EXEC Command
                    SqlCmd.ExecuteNonQuery();

                    rpta = true;

                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();

                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return rpta;
        }

        public List<Users> CheckUserNameAvailability(string UserName)
        {
            var UserList = new List<Users>();

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[usr].[uspCheckAvailabilityUserName]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
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
                            var User = new Users
                            {
                                UserID = Convert.ToInt32(dr["UserID"]),
                                UserName = dr["UserName"].ToString(),
                                FullName = dr["FullName"].ToString(),
                                Email = dr["Email"].ToString()
                            };

                            UserList.Add(User);

                        }
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return UserList;
        }

        public List<Users> CheckEmailAvailability(string Email)
        {
            var UserList = new List<Users>();

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[usr].[uspCheckAvailabilityEmail]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParEmail = new SqlParameter
                    {
                        ParameterName = "@Email",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = Email
                    };
                    SqlCmd.Parameters.Add(ParEmail);

                    using (var dr = SqlCmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var User = new Users
                            {
                                UserID = Convert.ToInt32(dr["UserID"]),
                                UserName = dr["UserName"].ToString(),
                                FullName = dr["FullName"].ToString(),
                                Email = dr["Email"].ToString()
                            };

                            UserList.Add(User);

                        }
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return UserList;
        }

        public bool AddNewUser(Users User, string InsertUser)
        {
            bool rpta = false;
            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[adm].[uspAddUser]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParInsertUser = new SqlParameter
                    {
                        ParameterName = "@InsertUser",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = InsertUser
                    };
                    SqlCmd.Parameters.Add(ParInsertUser);

                    SqlParameter ParFullName = new SqlParameter
                    {
                        ParameterName = "@FullName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = User.FullName
                    };
                    SqlCmd.Parameters.Add(ParFullName);

                    SqlParameter ParUserName = new SqlParameter
                    {
                        ParameterName = "@UserName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = User.UserName
                    };
                    SqlCmd.Parameters.Add(ParUserName);

                    SqlParameter ParEmail = new SqlParameter
                    {
                        ParameterName = "@Email",
                        SqlDbType = SqlDbType.VarChar,
                        Value = User.Email
                    };
                    SqlCmd.Parameters.Add(ParEmail);

                    SqlParameter ParRoleID = new SqlParameter
                    {
                        ParameterName = "@RoleID",
                        SqlDbType = SqlDbType.VarChar,
                        Value = User.RoleID
                    };
                    SqlCmd.Parameters.Add(ParRoleID);

                    SqlParameter ParPassword = new SqlParameter
                    {
                        ParameterName = "@Password",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = User.Password
                    };
                    SqlCmd.Parameters.Add(ParPassword);

                    //EXEC Command
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

        public List<Users> UsersList()
        {
            var UserList = new List<Users>();

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[adm].[uspReadUsers]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    using (var dr = SqlCmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var User = new Users
                            {
                                UserID = Convert.ToInt32(dr["UserID"]),
                                UserName = dr["UserName"].ToString(),
                                FullName = dr["FullName"].ToString(),
                                Email = dr["Email"].ToString(),
                                ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]),
                                AuthorizationFlag = Convert.ToBoolean(dr["AuthorizationFlag"])
                            };
                            if (!Convert.IsDBNull(dr["RoleID"]))
                            {
                                User.RoleID = Convert.ToInt32(dr["RoleID"]);
                            }
                            else
                            {
                                User.RoleID = null;
                            }

                            UserList.Add(User);
                        }
                    }
                    foreach (var u in UserList)
                    {
                        if (u.RoleID >= 1)
                        {
                            SqlCmd = new SqlCommand("[adm].[uspSearchRole]", SqlCon)
                            {
                                CommandType = CommandType.StoredProcedure
                            };

                            //Insert Parameters
                            SqlCmd.Parameters.AddWithValue("@RoleID", u.RoleID);

                            using (var dr = SqlCmd.ExecuteReader())
                            {
                                dr.Read();
                                if (dr.HasRows)
                                {
                                    u.RolesData.RoleID = Convert.ToInt32(dr["RoleID"]);
                                    u.RolesData.RoleName = dr["RoleName"].ToString();
                                }
                            }
                        }
                        else
                        {
                            u.RolesData = new Roles();
                        }
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return UserList;
        }

        // METHOD DETAILS
        public Users Details(int id)
        {
            var Detail = new Users();

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[adm].[uspSearchUser]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParUserID = new SqlParameter
                    {
                        ParameterName = "@UserID",
                        SqlDbType = SqlDbType.Int,
                        Value = id
                    };
                    SqlCmd.Parameters.Add(ParUserID);

                    using (var dr = SqlCmd.ExecuteReader())
                    {
                        dr.Read();
                        if (dr.HasRows)
                        {
                            Detail.UserID = Convert.ToInt32(dr["UserID"]);
                            Detail.FullName = dr["FullName"].ToString();
                            Detail.UserName = dr["UserName"].ToString();
                            Detail.Email = dr["Email"].ToString();
                            Detail.ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"]);
                            Detail.AuthorizationFlag = Convert.ToBoolean(dr["AuthorizationFlag"]);

                            if (!Convert.IsDBNull(dr["RoleID"]))
                            {
                                Detail.RoleID = Convert.ToInt32(dr["RoleID"]);
                            }
                            else
                            {
                                Detail.RoleID = null;
                            }
                        }
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Detail;
        }

        public bool UpdateUser(Users User, string InsertUser)
        {
            bool rpta = false;
            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[adm].[uspUpdateUser]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParActionType = new SqlParameter
                    {
                        ParameterName = "@ActionType",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 6,
                        Value = User.ActionType
                    };
                    SqlCmd.Parameters.Add(ParActionType);

                    SqlParameter ParInsertUser = new SqlParameter
                    {
                        ParameterName = "@InsertUser",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = InsertUser
                    };
                    SqlCmd.Parameters.Add(ParInsertUser);

                    SqlParameter ParUserID = new SqlParameter
                    {
                        ParameterName = "@UserID",
                        SqlDbType = SqlDbType.Int,
                        Value = User.UserID
                    };
                    SqlCmd.Parameters.Add(ParUserID);

                    SqlParameter ParFullName = new SqlParameter
                    {
                        ParameterName = "@FullName",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = User.FullName
                    };
                    SqlCmd.Parameters.Add(ParFullName);

                    SqlParameter ParRoleID = new SqlParameter
                    {
                        ParameterName = "@RoleID",
                        SqlDbType = SqlDbType.Int,
                        Value = User.RoleID
                    };
                    SqlCmd.Parameters.Add(ParRoleID);

                    SqlParameter ParActiveFlag = new SqlParameter
                    {
                        ParameterName = "@ActiveFlag",
                        SqlDbType = SqlDbType.Bit,
                        Value = User.ActiveFlag
                    };
                    SqlCmd.Parameters.Add(ParActiveFlag);

                    //EXEC Command
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
