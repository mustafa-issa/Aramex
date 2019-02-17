using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aramex.Models
{
    public class FcuSite
    {
        public int NO { get; set; }
        public int FCUAddress { get; set; }
        public string ServedArea { get; set; }
        public int? RunHoursWork { get; set; }
        public int? PreventiveMaintainanceRun { get; set; }
        public int? PreventiveMaintainanceOverdue { get; set; }
        public string RunName { get; set; }
        public string OverdueName { get; set; }
    }
}