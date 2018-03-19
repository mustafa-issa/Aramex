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
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        public static T GetValue<T>(object readerValue, T defaultValue = default(T))
        {
            if (readerValue == DBNull.Value)
                return defaultValue;
            else
                return (T)Convert.ChangeType(readerValue, typeof(T));
        }

        private string[] componentNames = {"","Booster Pump 1", "Booster Pump 2", "Smoke Exhaust Fan 1", "Smoke Exhaust Fan 2", "Smoke Exhaust Fan 3", "Smoke Exhaust Fan 4",
                                              "Smoke Exhaust Fan 5", "Smoke Exhaust Fan 6", "Smoke Exhaust Fan 7",	 "Smoke Exhaust Fan 8", "FAHU 1 Supply Fan", "FAHU 1 Exhaust Fan",
                                              "FAHU 1 Heat Recovery Wheel", "FAHU 1 Pre Filter", "FAHU 1 Bag Filter", "FAHU 2 Supply Fan", "FAHU 2 Exhaust Fan", 
                                              "FAHU 2 Heat Recovery Wheel", "FAHU 2 Pre Filter", "FAHU 2 Bag Filter", "FAHU 3 Supply Fan", "FAHU 3 Exhaust Fan", 
                                              "FAHU 3 Heat Recovery Wheel", "FAHU 3 Pre Filter", "FAHU 3 Bag Filter", "FAHU 4 Supply Fan", "FAHU 4 Exhaust Fan", 
                                              "FAHU 4 Heat Recovery Wheel", "FAHU 4 Pre Filter", "FAHU 4 Bag Filter", "FAHU 5 Supply Fan", "FAHU 5 Exhaust Fan", 
                                              "FAHU 5 Heat Recovery Wheel", "FAHU 5 Pre Filter", "FAHU 5 Bag Filter", "FAHU 6 Supply Fan", "FAHU 6 Exhaust Fan", 
                                              "FAHU 6 Heat Recovery Wheel", "FAHU 6 Pre Filter", "FAHU 6 Bag Filter", "Hot Water Pump 1", "Hot Water Pump 2", 
                                              "FAHU 1 Supply Fan", "FAHU 1 Exhaust Fan", "FAHU 1 Heat Recovery Wheel", "FAHU 1 Pre Filter", "FAHU 1 Bag Filter", 
                                              "FAHU 2 Supply Fan", "FAHU 2 Exhaust Fan", "FAHU 2 Heat Recovery Wheel", "FAHU 2 Pre Filter", "FAHU 2 Bag Filter", 
                                              "Booster Pump 1", "Booster Pump 2	", "Exhaust Air Fan", "Hot Water Pump 1", "Hot Water Pump 2", "Booster Pump 1", 
                                              "Booster Pump 2", "Electrical Fire Fighting Pump",	 "Jockey Fire Fighting Pump",	 "Deisel Fire Fighting Pump",
                                              "Exhaust Air Fan", "Fresh Air Fan", "Submersible Pump 1", "Submersible Pump 2"};
        //string connectionString = "Server=.\\SQLEXPRESS;Initial Catalog=AramexReportDB;User Id=selwade; Password=123; Integrated Security=SSPI;Trusted_Connection=False";

        

        public IList<FcuSite> GetAllFcuSites()
        {
            List<FcuSite> sites = new List<FcuSite>();
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
                    FcuSite site = new FcuSite();

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

        public IList<MechnicalSite> GetAllMechnicalSites()
        {
            List<MechnicalSite> sites = new List<MechnicalSite>();
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
                MechnicalSite site = null;
                SqlDataReader reader = cmd.ExecuteReader();
                for (int i = 1; i <componentNames.Length; i++)
                {
                    site = new MechnicalSite();
                    site.NO = i;
                    site.ComponentName = componentNames[i];
                    site.ServedArea = "Warehouse";
                    sites.Add(site);
                }
                
                con.Close();
            }
            return sites;
        }

    }
}