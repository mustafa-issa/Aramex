using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Aramex.Models
{

    public class DAL
    {
        private const string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public static T GetValue<T>(object readerValue, T defaultValue = default(T))
        {
            if (readerValue == DBNull.Value)
                return defaultValue;
            else
                return (T)Convert.ChangeType(readerValue, typeof(T));
        }

        //string connectionString = "Server=.\\SQLEXPRESS;Initial Catalog=AramexReportDB;User Id=selwade; Password=123; Integrated Security=SSPI;Trusted_Connection=False";
        

        public IList<Site> GetAllSites()
        {
            List<Site> sites = new List<Site>();
            string query = @" DECLARE @Table TABLE
                            (
                              NO int,
                              FCUAddress int,
                              ServedArea nvarchar(15),
                              RunHoursWork float,
                              PreventiveMaintainanceRun float,
                              PreventiveMaintainanceOverdue float,
                              RunName nvarchar(30),
                              OverdueName nvarchar(30)
                            )
                            insert into @Table(NO, FCUAddress, ServedArea, PreventiveMaintainanceRun, RunName, OverdueName)
                            SELECT cast(replace(TABLE_NAME,'Aramex_100_TL', '') as int), 
                            cast(replace(TABLE_NAME,'Aramex_100_TL', '') as int) + 1601, 
                            'Warehouse', 2000, TABLE_NAME, 'Aramex_100_TL' + CAST((cast(replace(TABLE_NAME,'Aramex_100_TL', '') as int) + 44) as nvarchar(30))
                            FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE '%100%' and table_name not like '%err%'
                            select * from @Table where no < 44 order by no";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Site site = new Site();

                    site.NO = GetValue(reader["NO"], 0);
                    site.FCUAddress = GetValue(reader["FCUAddress"], 0);
                    site.ServedArea = reader["ServedArea"].ToString();
                    site.RunHoursWork = GetValue(reader["RunHoursWork"], 0);
                    site.PreventiveMaintainanceRun = GetValue(reader["PreventiveMaintainanceRun"], 0);
                    site.PreventiveMaintainanceOverdue = GetValue(reader["PreventiveMaintainanceOverdue"], 0);
                    site.RunName = reader["RunName"].ToString();
                    site.OverdueName = reader["OverdueName"].ToString();
                    sites.Add(site);
                }
                con.Close();
            }
            return sites;
        }

        public double GetPreventiveMaintainanceRun(string table)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"declare @maxDate as datetime
                    set @maxDate = (select max(ts) from {0})
                    SELECT value FROM {0} where ts = @maxDate";
                string formattedQuery = String.Format(query, table);
                SqlCommand cmd = new SqlCommand(formattedQuery, conn);
                cmd.CommandType = CommandType.Text;
                try
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    return double.Parse(dt.Rows[0]["value"].ToString());
                }
                catch (Exception ex)
                {
                    return 0;
                }
                finally
                {
                    conn.Close();
                }
                
            }
        }
    }
}