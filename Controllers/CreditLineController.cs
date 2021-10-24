using CreditLineDemoAPI.BL;
using CreditLineDemoAPI.CommonBase.Logging;
using CreditLineDemoAPI.Models;
using CreditLineDemoAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace CreditLineDemoAPI.Controllers
{

    /// <summary>
    /// Method to get the credit line approved or denied 
    /// </summary>
    [RoutePrefix("api/creditline")]
    public class CreditLineController : ApiController
    {
        [HttpPost]
        [Route("GetCreditLine")]
        public HttpResponseMessage GetCreditLine([FromBody]CreditLine creditLinePar)
        {
            HttpResponseMessage message = new HttpResponseMessage();
            try
            {
                Logger.Info();


                CreditLine creditLine = creditLinePar; /*new CreditLine() {
                    CashBalance = cashBalance, FoundingType = foundingType,
                    MonthlyRevenue = monthlyRevenue, RequesteCreditLine = requesteCreditLine,
                    RequestedDate = DateTime.Now, CustomerId = customer
                };*/
                creditLine.RequestedDate = DateTime.Now;
                CreditLineManager creditManager = new CreditLineManager();
                int resultCredit = creditManager.ApproveCreditLine(creditLine);
                switch(resultCredit){
                    case Constants.SUCCESSCREDITLINECODE:
                        message = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            ReasonPhrase = Constants.CREDITLINEVALIDMSG + ": " + creditLine.RequesteCreditLine.ToString()
                        };
                        break;
                    case Constants.PARAMETERSINCOMPLETEERRORCODE:
                        message = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            ReasonPhrase = Constants.PARAMETERSINCOMPLETESMSG  
                        };
                        break;
                    case Constants.CREDITLINENOTVALIDERRORCODE:
                        message = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            ReasonPhrase = Constants.CREDITLINENOTVALIDMSG + ": "+ creditLine.RequesteCreditLine.ToString()
                        };
                        break;
                    case Constants.CREDITLINETWOMINUTESERRORCODE:
                        message = new HttpResponseMessage((System.Net.HttpStatusCode)429);
                        break;
                    case Constants.CREDITLINE30SECONDSERRORCODE:
                        message = new HttpResponseMessage((System.Net.HttpStatusCode)429);
                        break;

                    case Constants.CREDITLINETHIRDREJECTRRORCODE:
                        message = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            ReasonPhrase = Constants.AGENTWILLCONTACTMSG
                        };
                        break;
                }              
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = ex.Message
                };
            }
            return message;
        }


    }
}
