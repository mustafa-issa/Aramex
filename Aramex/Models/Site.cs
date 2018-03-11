using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aramex.Models
{
    public class Site
    {
        public int NO { get; set; }
        public int FCUAddress { get; set; }
        public string ServedArea { get; set; }
        public string RunHoursWork { get; set; }
        public string PreventiveMantainanceRun { get; set; }
        public string PreventiveMantainanceOverdue { get; set; }
    }
}