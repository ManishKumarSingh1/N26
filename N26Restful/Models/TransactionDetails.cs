using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace N26Restful.Models
{
    public class TransactionDetails
    {
        public double Amount { get; set; }
        public long TimeStamp { get; set; }
    }   
}