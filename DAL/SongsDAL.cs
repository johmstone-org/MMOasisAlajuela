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
    public class SongsDAL
    {
        public List<Songs> Repertoires (int AuthorID)
        {
            List<Songs> Repertoire = new List<Songs>();

            try
            {
                using (var SqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB_MUSIC_CR_OA_Connection"].ToString()))
                {
                    SqlCon.Open();
                    var SqlCmd = new SqlCommand("[usr].[uspSongsbyAuthor]", SqlCon)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    //Insert Parameters
                    SqlParameter ParAuthorID = new SqlParameter
                    {
                        ParameterName = "@AuthorID",
                        SqlDbType = SqlDbType.Int,
                        Value = AuthorID
                    };
                    SqlCmd.Parameters.Add(ParAuthorID);

                    using (var dr = SqlCmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var song = new Songs
                            {
                                SongID = Convert.ToInt32(dr["AuthorID"]),
                                SongName = dr["SongName"].ToString(),
                                AuthorID = Convert.ToInt32(dr["AuthorID"])
                            };

                            Repertoire.Add(song);

                        }
                    }
                    if (SqlCon.State == ConnectionState.Open) SqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Repertoire;
        }
    }
}
