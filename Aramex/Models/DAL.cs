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
        string connectionString = "";
        public IList<Site> GetAllSites()
        {
            List<Site> sites = new List<Site>();
            if (true)
            {
                Site site = new Site();

                site.NO = 1;
                site.FCUAddress = 1234;
                site.ServedArea = "served area";
                site.RunHoursWork = "run hours work";
                site.PreventiveMantainanceRun = "run";
                site.PreventiveMantainanceOverdue = "overdue";

                sites.Add(site);

                return sites;
            }
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from tanble", con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Site site = new Site();

                    site.NO = Convert.ToInt32(reader["EmployeeID"]);
                    site.FCUAddress = Convert.ToInt32(reader["Name"].ToString());
                    site.ServedArea = reader["Gender"].ToString();
                    site.RunHoursWork = reader["Department"].ToString();
                    site.PreventiveMantainanceRun = reader["City"].ToString();
                    site.PreventiveMantainanceOverdue = reader["City"].ToString();

                    sites.Add(site);
                }
                con.Close();
            }
            return sites;
        } 
    }
}