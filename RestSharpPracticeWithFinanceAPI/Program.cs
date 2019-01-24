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
            Console.WriteLine("Enter a symbol");
            var symbol = Console.ReadLine();
            string key = ApiKey.apiKey;
            Console.WriteLine("key: " + key);
            RestClient client = new RestClient("https://www.alphavantage.co");

            // create a new request
            var request = new RestRequest("/query?function=GLOBAL_QUOTE&symbol=" + symbol + "&apikey=1PO9VNXTLC4Z3GJR" + key);
            Console.WriteLine("request: " + request);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = (response.Content).GetType(); // outputs raw content as string
            var finalContent = response.Content;
            Console.WriteLine("Output: " + finalContent);

            //Deserialize will convert the raw string into Json format
            // Take the Json and convert it into a Csharp Object after instantiation
            StockQuote stockQuote = JsonConvert.DeserializeObject<StockQuote>(finalContent);
            foreach (KeyValuePair<string, string> item in stockQuote.Output)
            {
                Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
            }
            stockQuote.parseOutput();

            Console.WriteLine("symbol: " + stockQuote.symbol);
            Console.WriteLine("parsed open: " + stockQuote.open);
            Console.WriteLine(stockQuote.high);
            Console.WriteLine(stockQuote.low);
            Console.WriteLine(stockQuote.price);
            Console.WriteLine(stockQuote.volume);
            Console.WriteLine(stockQuote.latestTradingDay);
            Console.WriteLine(stockQuote.previousClose);
            Console.WriteLine(stockQuote.change);
            Console.WriteLine(stockQuote.changePercent);

            // connect to the db
            var connstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RestSharpPracticeWithFinanceAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                connection.Open();

                SqlCommand insCommand = new SqlCommand("INSERT INTO [StockQuote] (symbol, \"open\", high, low, price, volume, latestTradingDay, previousClose, change, changePercent) VALUES (@symbol, @open, @high, @low, @price, @volume, @latestTradingDay, @previousClose, @change, @changePercent)", connection);

                insCommand.Parameters.AddWithValue("@symbol", stockQuote.symbol);
                insCommand.Parameters.AddWithValue("@open", stockQuote.open);
                insCommand.Parameters.AddWithValue("@high", stockQuote.high);
                insCommand.Parameters.AddWithValue("@low", stockQuote.low);
                insCommand.Parameters.AddWithValue("@price", stockQuote.price);
                insCommand.Parameters.AddWithValue("@volume", stockQuote.volume);
                insCommand.Parameters.AddWithValue("@latestTradingDay", stockQuote.latestTradingDay);
                insCommand.Parameters.AddWithValue("@previousClose", stockQuote.previousClose);
                insCommand.Parameters.AddWithValue("@change", stockQuote.change);
                insCommand.Parameters.AddWithValue("@changePercent", stockQuote.changePercent);

                insCommand.ExecuteNonQuery();
                Console.WriteLine("DB updated");
                connection.Close();
            }
        }
    }
}
