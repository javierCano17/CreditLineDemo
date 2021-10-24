using CreditLineDemoAPI.BL;
using CreditLineDemoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CreditLineDemoAPI.Services
{
    public class CreditLineManager
    {
        public int ApproveCreditLine(CreditLine creditLine) {
            int result = 0;
            ICreditLineBL creditLineBl = new CreditLineBL(creditLine);
            ///validate if the values came in the request
            if (creditLineBl.ValidateCreditLineRequest(creditLine))
            {
                CreditLineDALC dalc = new CreditLineDALC();
               ///get the saved value
                CreditLine creditLineSaved = dalc.GetCreditLine(creditLine.CustomerId.ToString());
                if (creditLineSaved == null)
                {
                    //check if the credit line requested satisfy the business rules
                    if (creditLineBl.CalculateRisk())
                    {
                        result = Constants.SUCCESSCREDITLINECODE;                                                                  
                    }
                    else
                    {
                        ///if not satisfy the rules reject the request
                        result = Constants.CREDITLINENOTVALIDERRORCODE;
                        creditLine.RequesteCreditLine = 0;                    
                        creditLine.RejectedCount++;
                    }
                    //save the information 
                    creditLine.CountTry++;
                    dalc.SaveCreditLine(creditLine);
                }
                else {
                    int secondsDifferenceLastRequest = creditLineBl.GetDifferenceTime(creditLine.RequestedDate, creditLineSaved.RequestedDate);
                    ///check if creditline is already accepted 
                    if (creditLineSaved.RequesteCreditLine > 0)
                    {                        
                                           
                        creditLineSaved.RequestedDate = creditLine.RequestedDate;
                        //check if the request is after two minutes or less than the previous request
                        if (secondsDifferenceLastRequest <= 120)
                        {
                            //check if is the third or more request within the 3 min. return error 429 code
                            if (creditLineSaved.CountTry >= 3)
                            {
                                creditLineSaved.ErrorCount++;
                                result = Constants.CREDITLINETWOMINUTESERRORCODE;
                            }
                            else {
                                ///if not return the same credite line already accepted and success code
                                creditLine.RequesteCreditLine = creditLineSaved.RequesteCreditLine;
                                result = Constants.SUCCESSCREDITLINECODE;
                            }
                            //increment the counter of request and save the values
                            creditLineSaved.CountTry++;
                            dalc.SaveCreditLine(creditLineSaved);
                        }
                        else {
                            //if the request happened after two minutes of the previouse one return success
                            creditLine.RequesteCreditLine = creditLineSaved.RequesteCreditLine;
                            result = Constants.SUCCESSCREDITLINECODE;
                            creditLineSaved.CountTry++;
                            dalc.SaveCreditLine(creditLineSaved);
                        }
                        
                    }
                    else {
                        ///not acepted credit line before

                        //check if the previous request was rejected and the previous request time was 30 seconds or less before
                        if (creditLineSaved.RejectedCount > 0 && creditLineSaved.RequesteCreditLine == 0 
                               && secondsDifferenceLastRequest <= 30)
                        {
                            //return error code and increment the error counter
                            result = Constants.CREDITLINE30SECONDSERRORCODE;
                            //set the information to be saved
                            creditLine.ErrorCount = creditLineSaved.ErrorCount;
                            creditLine.CountTry = creditLineSaved.CountTry;
                            creditLine.RequesteCreditLine = 0;
                        }
                        else {                            

                            //check if the credit line requested satisfy the business rules
                            if (creditLineBl.CalculateRisk())
                            {
                                result = Constants.SUCCESSCREDITLINECODE;
                                //set the information to be saved
                                creditLine.ErrorCount = creditLineSaved.ErrorCount;
                                //reset count because it was approved
                                creditLine.CountTry = 1;
                            }
                            else
                            {
                                ///if not satisfy the rules reject the request
                                //check if is the third time or more that the request is rejected
                                if (creditLineSaved.RejectedCount >= 2)
                                {
                                    //return the code that a sales agent will contact 
                                    result = Constants.CREDITLINETHIRDREJECTRRORCODE;
                                    ///set previouse rejected values and error counts;
                                    creditLine.RejectedCount = creditLineSaved.RejectedCount;
                                    creditLine.CountTry = creditLineSaved.CountTry;
                                    creditLine.RequesteCreditLine = 0;
                                    creditLine.RejectedCount++;
                                }
                                else {
                                    //is a normal reject                                 
                                    result = Constants.CREDITLINENOTVALIDERRORCODE;
                                    ///set previouse rejected values and error counts;
                                    creditLine.RejectedCount = creditLineSaved.RejectedCount;
                                    creditLine.CountTry = creditLineSaved.CountTry;
                                    creditLine.RequesteCreditLine = 0;
                                    creditLine.RejectedCount++;
                                }                                
                            }
                        }                        
                        //save the information 
                        creditLine.CountTry++;
                        dalc.SaveCreditLine(creditLine);
                    }                  
                } 
            }
            else {
                result = Constants.PARAMETERSINCOMPLETEERRORCODE; 
            }
            return result;
        }
    }
}