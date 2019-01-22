using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpPracticeWithFinanceAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = ApiKey.apiKey;
            Console.WriteLine("key: " + key);
            RestClient client = new RestClient("https://www.alphavantage.co");

            // create a new request
            var request = new RestRequest("/query?function=GLOBAL_QUOTE&symbol=googl&apikey=1PO9VNXTLC4Z3GJR" + key);
            Console.WriteLine("request: " + request);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = (response.Content).GetType(); // outputs raw content as string
            var finalContent = response.Content;
            Console.WriteLine("Output: " + finalContent);

            //Deserialize will convert the raw string into Json format
            // Take the Json and convert it into a Csharp Object after instantiation
            StockQuote stockQuote = JsonConvert.DeserializeObject<StockQuote>(finalContent);

            stockQuote.parseOutput();
            
            Console.WriteLine(stockQuote.symbol);
            Console.WriteLine(stockQuote.high);
            Console.WriteLine(stockQuote.low);
            Console.WriteLine(stockQuote.price);
            Console.WriteLine(stockQuote.volume);
            Console.WriteLine(stockQuote.latestTradingDay);
            Console.WriteLine(stockQuote.previousClose);
            Console.WriteLine(stockQuote.change);
            Console.WriteLine(stockQuote.changePercent);
        }
    }
}
