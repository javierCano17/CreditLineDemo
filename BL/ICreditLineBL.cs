using CreditLineDemoAPI.Models;
using System;

namespace CreditLineDemoAPI.BL
{
    public interface ICreditLineBL
    {
        bool ValidateCreditLineRequest(CreditLine creditLine);
        bool CalculateRisk();
        int GetDifferenceTime(DateTime creditLine, DateTime creditSaved);
    }
}
