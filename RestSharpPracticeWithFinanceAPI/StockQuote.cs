using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace RestSharpPracticeWithFinanceAPI
{
    class StockQuote
    {
        public string symbol { get; set; }
        public float open { get; set; }
        public float high { get; set; }
        public float low { get; set; }
        public float price { get; set; }
        public float volume { get; set; }
        public DateTime latestTradingDay { get; set; }
        public float previousClose { get; set; }
        public float change { get; set; }
        public float changePercent { get; set; }

        [JsonProperty("Global Quote")]
        public Dictionary<string, string> Output { get; set; }

        

    public void parseOutput()
        {
            symbol = Output["01. symbol"];
            open = float.Parse(Output["02. open"]);
            high = float.Parse(Output["03. high"]);
            low = float.Parse(Output["04. low"]);
            price = float.Parse(Output["05. price"]);
            volume = float.Parse(Output["06. volume"]);
            latestTradingDay = DateTime.Parse(Output["07. latest trading day"]);
            previousClose = float.Parse(Output["08. previous close"]);
            change = float.Parse(Output["09. change"]);
            
            changePercent = float.Parse(Output["10. change percent"].Trim('%'));

        }

    }
}
