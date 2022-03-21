using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Timers;

namespace BinanceSimbols.Models
{
    public class BinanceConstModel
    {
        public List<Const> PublicConsts { set; get; }
        public List<Const> PrivateConsts { set; get; }
        public List<Symbol> Symbols { get; set; }

        private System.Timers.Timer checkForTime;

        const double interval = 5 * 60 * 1000; // milliseconds 
        public BinanceConstModel()
        {

            PublicConsts = new List<Const> {
                new Const { Name = "BINANCE_BASE_URL", Value = @"https://api.binance.com" },
                new Const { Name = "DATA_STREAM_ENDPOINT", Value = @"wss://stream.binance.com:9443/ws" },
                new Const { Name = "DATA_STREAM_ENDPOINT_COMBINED", Value = @"wss://stream.binance.com:9443/stream?streams=" },
                new Const { Name = "PUBLIC_API_KEY", Value = @"1IYHxEIZgOMe552s9bW1sYlcb5tWalnc360A0qcCQ5M2UtHQvQYUmmty5rhKOCOg" }
            };

            Symbols = new List<Symbol>();


            checkForTime = new System.Timers.Timer(interval);
            checkForTime.Elapsed += new ElapsedEventHandler(checkForTime_Elapsed);
            checkForTime.Enabled = true;

            UpdateSymbols();
        }


        private void checkForTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateSymbols();
        }

        private void UpdateSymbols()
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(PublicConsts.Find(x => x.Name == "BINANCE_BASE_URL").Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("api/v1/exchangeInfo").Result;
                if (response.IsSuccessStatusCode)
                {
                    var responeSymbols = response.Content.ReadAsStringAsync().Result;
                    JObject objSymbols = (JObject)JsonConvert.DeserializeObject(responeSymbols);
                    var tokenSymbols = objSymbols.SelectToken("symbols");
                    var symbolsList = tokenSymbols.ToList();

                    Symbols.Clear();

                    for (int i = 0; i < symbolsList.Count; i++)
                    {
                        JToken symbolToken = symbolsList[i];
                        Symbols.Add(new Symbol { Name = symbolToken.SelectToken("symbol").ToString().ToLower(), Status = symbolToken.SelectToken("status").ToString().ToLower() });
                    }

                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }

        }



    }
}
