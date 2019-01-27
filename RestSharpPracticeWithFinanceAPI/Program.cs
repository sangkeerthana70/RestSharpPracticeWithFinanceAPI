using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RestSharpPracticeWithFinanceAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // FetchDataFromIEXTradingAPI.FetchIEXT
            Console.WriteLine("making a call to api.iextrading.com");
            FetchDataFromIEXTradingAPI.FetchIEXT();

            // FetchDataFromAlphavantageAPI.FetchAlphaVantage()
            Console.WriteLine("making a call to alphavantage.co");
            FetchDataFromAlphaVantageAPI.FetchAlphaVantage();
        }
    }
}
