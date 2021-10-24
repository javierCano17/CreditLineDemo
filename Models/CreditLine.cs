using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreditLineDemoAPI.Models
{
    public class CreditLine
    {
        public string CustomerId { get; set; }
        public string FoundingType{ get; set; }
        public decimal CashBalance { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal RequesteCreditLine { get; set; }
        public decimal CreditLineRecommended { get; set; }
        public DateTime RequestedDate { get; set; }        
        public int CountTry { get; set; }
        public int ErrorCount { get; set; }
        public int RejectedCount { get; set; }
    }
}