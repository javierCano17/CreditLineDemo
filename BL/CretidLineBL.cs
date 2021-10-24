using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CreditLineDemoAPI.Models;

namespace CreditLineDemoAPI.BL
{
    public class CreditLineBL : ICreditLineBL
    {
        private CreditLine crediteLine;

        public CreditLineBL(CreditLine CreditLine) {
            crediteLine = CreditLine;            
        }

        public bool CalculateRisk( )
        {
            CalculateCreditCardRecommended();
            return crediteLine.CreditLineRecommended > crediteLine.RequesteCreditLine;            
        }

        public int GetDifferenceTime(DateTime creditLineDate, DateTime creditSavedDate) {            
            if (creditSavedDate == DateTime.MinValue)
            {
                return 121;
            }
            TimeSpan ts = creditLineDate - creditSavedDate;
            return ts.Seconds + ts.Minutes * 60;                       
        } 

        public bool ValidateCreditLineRequest(CreditLine creditLine)
        {
            return ValidateCreditLineValues();
        }

        private void CalculateCreditCardRecommended() {
            Enum.TryParse(crediteLine.FoundingType, out FoundingType founding);
            decimal recommendedMonthlyFactor, recommendedCashFactor;
            string factorRecommendedMonthlyCredit =ConfigurationManager.AppSettings["BalanceFactorMonthlyRevenue"];
            string factorRecommendedCashCredit = ConfigurationManager.AppSettings["BalanceFactorCashBalance"];
            switch (founding) {
                case FoundingType.SME:
                    
                    decimal.TryParse(factorRecommendedMonthlyCredit, out recommendedMonthlyFactor);
                    if (recommendedMonthlyFactor > 0)
                    {
                        crediteLine.CreditLineRecommended = crediteLine.MonthlyRevenue / recommendedMonthlyFactor;
                    }

                    break;
                case FoundingType.Startup :
               
                    decimal.TryParse(factorRecommendedMonthlyCredit, out recommendedMonthlyFactor);
                    decimal.TryParse(factorRecommendedCashCredit, out recommendedCashFactor);

                    if (recommendedMonthlyFactor > 0 && recommendedCashFactor > 0)
                    {
                        //Get the max value from the monthly revenue and the cash balance
                        crediteLine.CreditLineRecommended =
                            Math.Max(crediteLine.MonthlyRevenue / recommendedMonthlyFactor
                            , crediteLine.CashBalance / recommendedCashFactor);
                    }
                    else if (recommendedMonthlyFactor > 0)
                    {
                        //if only has montlyfactor
                        crediteLine.CreditLineRecommended = crediteLine.MonthlyRevenue / recommendedMonthlyFactor;
                    }
                    else if (recommendedCashFactor > 0)
                    {
                        //if only has cash factor
                        crediteLine.CreditLineRecommended = crediteLine.CashBalance / recommendedCashFactor;
                    }
                    break;                    
            }

            
        }

        private bool ValidateCreditLineValues() {
            if (crediteLine.CustomerId == null || crediteLine.CustomerId == string.Empty) {
                return false;
            }
            if (crediteLine.FoundingType == null || crediteLine.FoundingType.Equals(string.Empty))
            {
                return false;
            }
            if (crediteLine.CashBalance <= 0)
            {
                return false;
            }
            if (crediteLine.MonthlyRevenue <= 0)
            {
                return false;
            }
            if (crediteLine.RequesteCreditLine <= 0)
            {
                return false;
            }
            
            return true;
        }
    }
}