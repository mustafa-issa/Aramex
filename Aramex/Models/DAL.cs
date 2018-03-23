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

        private string[,] componentNames =  {{"1", "Aramex_100_TL1", "Booster Pump 1", "1000", "Aramex_100_TL54", "Aramex_100_TL13"}, 
                                            {"2", "Aramex_100_TL2", "Booster Pump 2", "1000", "Aramex_100_TL55", "Aramex_100_TL14"},
                                            {"3", "0", "Smoke Exhaust Fan 1", "0", "0", "Aramex_100_TL15"},
                                            {"4", "0", "Smoke Exhaust Fan 2", "0", "0", "Aramex_100_TL16"},
                                            {"5", "0", "Smoke Exhaust Fan 3", "0", "0", "Aramex_100_TL17"},
                                            {"6", "0", "Smoke Exhaust Fan 4", "0", "0", "Aramex_100_TL18"},
                                            {"7", "0", "Smoke Exhaust Fan 5", "0", "0", "Aramex_100_TL19"},
                                            {"8", "0", "Smoke Exhaust Fan 6", "0", "0", "Aramex_100_TL20"},
                                            {"9", "0", "Smoke Exhaust Fan 7", "0", "0", "Aramex_100_TL21"},
                                            {"10", "0", "Smoke Exhaust Fan 8", "0", "0", "Aramex_100_TL22"},
                                            {"11", "Aramex_100_TL3", "FAHU 1 Supply Fan", "2000","Aramex_100_TL56", "Aramex_100_TL23"},
                                            {"12", "0", "FAHU 1 Exhaust Fan", "0", "0", "Aramex_100_TL24"},
                                            {"13", "0", "FAHU 1 Heat Recovery Wheel", "0", "0", "Aramex_100_TL25"},
                                            {"14", "0", "FAHU 1 Pre Filter", "0", "0", "Aramex_100_TL26"},
                                            {"15", "0", "FAHU 1 Bag Filter", "0", "0", "Aramex_100_TL27"},
                                            {"16", "Aramex_100_TL4", "FAHU 2 Supply Fan", "2000", "Aramex_100_TL57", "Aramex_100_TL28"},
                                            {"17", "0", "FAHU 2 Exhaust Fan", "0", "0", "Aramex_100_TL29"},
                                            {"18", "0", "FAHU 2 Heat Recovery Wheel", "0", "0", "Aramex_100_TL30"},
                                            {"19", "0", "FAHU 2 Pre Filter", "0", "0", "Aramex_100_TL31"},
                                            {"20", "0", "FAHU 2 Bag Filter", "0", "0", "Aramex_100_TL32"},
                                            {"21", "Aramex_100_TL5", "FAHU 3 Supply Fan", "2000", "Aramex_100_TL58", "Aramex_100_TL33"},
                                            {"22", "0", "FAHU 3 Exhaust Fan", "0", "0", "Aramex_100_TL34"},
                                            {"23", "0", "FAHU 3 Heat Recovery Wheel", "0", "0", "Aramex_100_TL35"}, 
                                            {"24", "0", "FAHU 3 Pre Filter", "0", "0", "Aramex_100_TL36"}, 
                                            {"25", "0", "FAHU 3 Bag Filter", "0", "0", "Aramex_100_TL37"}, 
                                            {"26", "Aramex_100_TL6", "FAHU 4 Supply Fan", "2000", "Aramex_100_TL59", "Aramex_100_TL38"},
                                            {"27", "0", "FAHU 4 Exhaust Fan", "0", "0", "Aramex_100_TL39"},
                                            {"28", "0", "FAHU 4 Heat Recovery Wheel", "0", "0", "Aramex_100_TL40"},
                                            {"29", "0", "FAHU 4 Pre Filter", "0", "0", "Aramex_100_TL41"}, 
                                            {"30", "0", "FAHU 4 Bag Filter", "0", "0", "Aramex_100_TL42"}, 
                                            {"31", "Aramex_100_TL7", "FAHU 5 Supply Fan", "2000", "Aramex_100_TL60", "Aramex_100_TL43"}, 
                                            {"32", "0", "FAHU 5 Exhaust Fan", "0", "0", "Aramex_100_TL44"},
                                            {"33", "0", "FAHU 5 Heat Recovery Wheel", "0", "0", "Aramex_100_TL45"},
                                            {"34", "0", "FAHU 5 Pre Filter", "0", "0", "Aramex_100_TL46"},
                                            {"35", "0", "FAHU 5 Bag Filter", "0", "0", "Aramex_100_TL47"}, 
                                            {"36", "Aramex_100_TL8", "FAHU 6 Supply Fan", "2000", "Aramex_100_TL61", "Aramex_100_TL48"},
                                            {"37", "0", "FAHU 6 Exhaust Fan", "0", "0", "Aramex_100_TL49"},
                                            {"38", "0", "FAHU 6 Heat Recovery Wheel", "0", "0", "Aramex_100_TL50"},
                                            {"39", "0", "FAHU 6 Pre Filter", "0", "0", "Aramex_100_TL51"}, 
                                            {"40", "0", "FAHU 6 Bag Filter", "0", "0", "Aramex_100_TL52"}, 
                                            {"41", "Aramex_100_TL9", "Hot Water Pump 1", "500", "Aramex_100_TL62", "Aramex_100_TL53"},
                                            {"43", "Aramex_200_TL1", "FAHU 1 Supply Fan", "2000", "Aramex_200_TL23", "Aramex_200_TL38"}, 
                                            {"44", "0", "FAHU 1 Exhaust Fan", "0", "0", "Aramex_200_TL39"}, 
                                            {"45", "0", "FAHU 1 Heat Recovery Wheel" , "0", "0", "Aramex_200_TL40"}, 
                                            {"46", "0","FAHU 1 Pre Filter" , "0", "0", "Aramex_200_TL41"}, 
                                            {"47", "0", "FAHU 1 Bag Filter", "0", "0", "Aramex_200_TL42"},
                                            {"48", "Aramex_200_TL2", "FAHU 2 Supply Fan", "2000", "Aramex_200_TL24", "Aramex_200_TL43"},
                                            {"49", "0", "FAHU 2 Exhaust Fan", "0", "0", "Aramex_200_TL44"}, 
                                            {"50", "0", "FAHU 2 Heat Recovery Wheel" , "0", "0", "Aramex_200_TL45"},
                                            {"51", "0","FAHU 2 Pre Filter" , "0", "0", "Aramex_200_TL46"}, 
                                            {"52", "0", "FAHU 2 Bag Filter", "0", "0", "Aramex_200_TL47"},
                                            {"53", "Aramex_200_TL3", "Booster Pump 1", "1000", "Aramex_200_TL25", "Aramex_200_TL48"}, 
                                            {"54", "Aramex_200_TL4", "Booster Pump 2", "1000", "Aramex_200_TL26", "Aramex_200_TL49"}, 
                                            {"55", "Aramex_200_TL5", "Exhaust Air Fan", "1000", "Aramex_200_TL27", "Aramex_200_TL50"}, 
                                            {"56", "Aramex_200_TL6", "Hot Water Pump 1", "500", "Aramex_200_TL28", "Aramex_200_TL51"}, 
                                            {"58", "Aramex_200_TL8", "Booster Pump 1", "1000", "Aramex_200_TL29", "Aramex_200_TL52"},
                                            {"59", "Aramex_200_TL9", "Booster Pump 2", "1000", "Aramex_200_TL30", "Aramex_200_TL53"}, 
                                            {"60", "Aramex_200_TL10", "Electrical Fire Fighting Pump", "200", "Aramex_200_TL31", "Aramex_200_TL54"}, 
                                            {"61", "Aramex_200_TL11", "Jockey Fire Fighting Pump", "200", "Aramex_200_TL32", "Aramex_200_TL55"},
                                            {"62", "Aramex_200_TL12", "Deisel Fire Fighting Pump", "200", "Aramex_200_TL33", "Aramex_200_TL56"},
                                            {"63", "Aramex_200_TL13", "Exhaust Air Fan", "1000", "Aramex_200_TL34", "Aramex_200_TL57"},
                                            {"64", "Aramex_200_TL14", "Fresh Air Fan", "2000", "Aramex_200_TL35", "Aramex_200_TL58"}, 
                                            {"65", "Aramex_200_TL15", "Submersible Pump 1", "1000", "Aramex_200_TL36", "Aramex_200_TL59"},
                                            {"66", "Aramex_200_TL16", "Submersible Pump 2", "1000", "Aramex_200_TL37", "Aramex_200_TL60"}};
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
                            SELECT cast(replace(TABLE_NAME,'Aramex_400_TL', '') as int), 
                            cast(replace(TABLE_NAME,'Aramex_400_TL', '') as int) + 1601, 
                            'Warehouse', 2000, TABLE_NAME, 'Aramex_400_TL' + CAST((cast(replace(TABLE_NAME,'Aramex_400_TL', '') as int) + 44) as nvarchar(30))
                            FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE '%400%' and table_name not like '%err%'
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
            MechnicalSite site = null;
            for (int i = 0; i < componentNames.GetLength(0); i++)
            {
                site = new MechnicalSite();
                site.NO = int.Parse(componentNames[i, 0]);
                site.ComponentName = componentNames[i, 2];
                if(site.NO <= 35)
                {
                    site.ServedArea = "Warehouse";
                }
                else if(site.NO <= 42)
                {
                    site.ServedArea = "Office Building";
                }
                else if (site.NO <= 57)
                {
                    site.ServedArea = "Driver Building";
                }
                else
                {
                    site.ServedArea = "Ancillary Facilities";
                }
                    if (componentNames[i, 1] != "0")
                {
                    site.RunHoursWork = GetPreventiveMaintainanceRun(componentNames[i, 1]);
                    site.PreventiveMaintainanceRun = double.Parse(componentNames[i, 3]);
                    site.PreventiveMaintainanceOverdue = GetPreventiveMaintainanceRun(componentNames[i, 4]);
                }
                site.TripHours = GetPreventiveMaintainanceRun(componentNames[i, 5]);
                sites.Add(site);
            }
            return sites;
        }

    }
}