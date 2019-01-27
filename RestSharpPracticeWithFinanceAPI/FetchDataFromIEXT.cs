using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpPracticeWithFinanceAPI
{
    class FetchDataFromIEXTradingAPI
    {
        public static void FetchIEXT()
        {
            Console.WriteLine("Making request to IEXTrading API");
            Console.WriteLine("Enter a symbol");
            var symbol = Console.ReadLine();
            
            RestClient client = new RestClient("https://api.iextrading.com/1.0");

            // create a new request
            RestRequest request = new RestRequest("/stock/" + symbol + "/quote");
            Console.WriteLine(request);

            // execute the request
            IRestResponse response = client.Execute(request);
            //var content = (response.Content).GetType(); // outputs raw content as string
            var finalContent = response.Content;
            Console.WriteLine("Output: " + finalContent);

            // parse incoming Json into a  Csharp object without any class needed
            JObject jAPIResult = JObject.Parse(finalContent);

            // import StockQuote class from RestSharpPracticeWithFinanceAPI project in the same solution
            StockQuote stockQuote = new StockQuote();
            // parse the Object's property into string
            stockQuote.symbol = jAPIResult["symbol"].ToString();
            stockQuote.open = float.Parse(jAPIResult["open"].ToString());
            stockQuote.high = float.Parse(jAPIResult["high"].ToString());
            stockQuote.low = float.Parse(jAPIResult["low"].ToString());
            stockQuote.price = float.Parse(jAPIResult["latestPrice"].ToString());
            stockQuote.volume = float.Parse(jAPIResult["latestVolume"].ToString());
            stockQuote.previousClose = float.Parse(jAPIResult["previousClose"].ToString());
            stockQuote.change = float.Parse(jAPIResult["change"].ToString());
            stockQuote.changePercent = float.Parse(jAPIResult["changePercent"].ToString());
            stockQuote.dataSource = "iextrading";
            stockQuote.createDate = DateTime.Now;

            // connect to the db
            var connstring = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=RestSharpPracticeWithFinanceAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                connection.Open();

                SqlCommand insCommand = new SqlCommand("INSERT INTO [StockQuote] (symbol, \"open\", high, low, price, volume, latestTradingDay, previousClose, change, changePercent, dataSource, createDate) VALUES (@symbol, @open, @high, @low, @price, @volume, @latestTradingDay, @previousClose, @change, @changePercent, @dataSource, @createDate)", connection);

                insCommand.Parameters.AddWithValue("@symbol", stockQuote.symbol);
                insCommand.Parameters.AddWithValue("@open", stockQuote.open);
                insCommand.Parameters.AddWithValue("@high", stockQuote.high);
                insCommand.Parameters.AddWithValue("@low", stockQuote.low);
                insCommand.Parameters.AddWithValue("@price", stockQuote.price);
                insCommand.Parameters.AddWithValue("@volume", stockQuote.volume);
                insCommand.Parameters.AddWithValue("@latestTradingDay", DateTime.Today);
                insCommand.Parameters.AddWithValue("@previousClose", stockQuote.previousClose);
                insCommand.Parameters.AddWithValue("@change", stockQuote.change);
                insCommand.Parameters.AddWithValue("@changePercent", stockQuote.changePercent);
                insCommand.Parameters.AddWithValue("@dataSource", stockQuote.dataSource);
                insCommand.Parameters.AddWithValue("@createDate", stockQuote.createDate);

                insCommand.ExecuteNonQuery();
                Console.WriteLine("DB updated");
                connection.Close();
            }
        }

    }
}
