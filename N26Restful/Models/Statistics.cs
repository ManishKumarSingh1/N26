using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N26Restful.Models
{
    public class Statistics
    {
        public double Sum { get; set; }
        public double Avg { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public int Count { get; set; }
    }
}