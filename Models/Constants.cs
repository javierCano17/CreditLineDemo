using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreditLineDemoAPI.Models
{
    public static class Constants
    {
        public const string PARAMETERSINCOMPLETESMSG = "Parameters incompletes";
        public const int PARAMETERSINCOMPLETEERRORCODE = 5000;
        public const int CREDITLINENOTVALIDERRORCODE = 5001;
        public const string CREDITLINENOTVALIDMSG = "The credit line has been rejected";
        public const string CREDITLINEVALIDMSG = "The credit line has been approved";
        public const string AGENTWILLCONTACTMSG = "A sales agent will contact you";
        public const int SUCCESSCREDITLINECODE = 200;
        public const int CREDITLINETWOMINUTESERRORCODE = 5002;        
        public const int CREDITLINE30SECONDSERRORCODE = 5003;
        public const int CREDITLINETHIRDREJECTRRORCODE = 5004;
    }
}