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
    public class ProgramsDAL
    {
        public List<Programs> ProgramList()
        {
            List<Programs> List = new List<Programs>();
            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[usr].[uspReadPrograms]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    using (var dr = SqlCmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var Program = new Programs
                            {
                                ProgramID = Convert.ToInt32(dr["ProgramID"]),
                                ProgramDate = Convert.ToDateTime(dr["ProgramDate"]),
                                ProgramSchedule = dr["ProgramSchedule"].ToString(),
                                CompleteFlag = Convert.ToBoolean(dr["CompleteFlag"]),
                                ActiveFlag = Convert.ToBoolean(dr["ActiveFlag"])
                            };

                            List.Add(Program);

                        }
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return List;
        }

        public int AddNew(Programs Program, String InsertUser)
        {
            int ProgramID = 0;
            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[usr].[uspAddProgram]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParDate = new SqlParameter
                    {
                        ParameterName = "@PDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = Program.ProgramDate
                    };
                    SqlCmd.Parameters.Add(ParDate);

                    SqlParameter ParSchedule = new SqlParameter
                    {
                        ParameterName = "@PSchedule",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 20,
                        Value = Program.ProgramSchedule
                    };
                    SqlCmd.Parameters.Add(ParSchedule);

                    SqlParameter ParInsertUser = new SqlParameter
                    {
                        ParameterName = "@InsertUser",
                        SqlDbType = SqlDbType.VarChar,
                        Size = 50,
                        Value = InsertUser
                    };
                    SqlCmd.Parameters.Add(ParInsertUser);

                    SqlParameter ParProgramID = new SqlParameter
                    {
                        ParameterName = "@ProgramID",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    SqlCmd.Parameters.Add(ParProgramID);

                    //Exec Command
                    SqlCmd.ExecuteNonQuery();

                    ProgramID = Convert.ToInt32(SqlCmd.Parameters["@ProgramID"].Value);

                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
            return ProgramID;
        }
    }
}
