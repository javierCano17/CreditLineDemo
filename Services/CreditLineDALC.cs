using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CreditLineDemoAPI.Models;
using System.Configuration;
using System.Web;
using CreditLineDemoAPI.CommonBase.Logging;

namespace CreditLineDemoAPI.Services
{
    /// <summary>
    /// data will be saved in a file to emulate a database. the file will have the next values
    /// customerId,requestTime,CreditApproved,countTry,errorCount,rejectedcount 
    /// </summary>
    public class CreditLineDALC
    {
        public CreditLine GetCreditLine(string customer) {
            CreditLine creditLine = new CreditLine();
            
            try
            {
                string line = ReadFileValue(customer);
                if (line != null && !line.Equals(string.Empty))
                {
                    creditLine.CustomerId = line.Split(',')[0];
                    creditLine.RequestedDate = DateTime.Parse(line.Split(',')[1]);
                    creditLine.RequesteCreditLine = decimal.Parse(line.Split(',')[2]);
                    creditLine.CountTry = int.Parse(line.Split(',')[3]);
                    creditLine.ErrorCount = int.Parse(line.Split(',')[4]);
                    creditLine.RejectedCount = int.Parse(line.Split(',')[5]);
                }
                else {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return creditLine;
        }

        public void SaveCreditLine(CreditLine creditLine) {
            string pathFile = HttpRuntime.AppDomainAppPath + ConfigurationManager.AppSettings["FileName"];
            Logger.Info();
            string line = string.Empty;
            try
            {
                string text = File.ReadAllText(pathFile);
                string lineExist = ReadFileValue(creditLine.CustomerId.ToString());
                string newLine = creditLine.CustomerId.ToString() + "," + creditLine.RequestedDate.ToString() + "," +
                        creditLine.RequesteCreditLine.ToString() + "," + creditLine.CountTry.ToString()
                        + "," + creditLine.ErrorCount.ToString() + "," + creditLine.RejectedCount.ToString();
                //remove existing line before save it
                if (lineExist!= null && text.Contains(lineExist))
                {
                    text = text.Replace(lineExist, newLine);
                    File.WriteAllText(pathFile,text);
                }
                else {
                    //if not exist save the value
                    File.AppendAllText(pathFile, newLine + Environment.NewLine);
                    text = text + "";
                    
                }                
            }
            catch (Exception ex)
            {
                Logger.Error("Error writing the file, error message:" + ex.Message);
            }
         
        }

        private string ReadFileValue(string customer) {
            string pathFile = HttpRuntime.AppDomainAppPath  + ConfigurationManager.AppSettings["FileName"];
            Logger.Info();
            string line = string.Empty;
            try {
                StreamReader sr = new StreamReader(pathFile);
                line = sr.ReadLine();
                while (line != null)
                {
                    string customerName = line.Split(',')[0];
                    if (customer.ToLower().Equals(customerName.ToLower()))
                    {
                        break;
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex) {
                Logger.Error("Error reading the file, error message:" +  ex.Message);
            }
            return line;
        }
    }
}