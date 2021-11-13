using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GAWorker
{
    public class WebClient
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task SendValue(double value)
        {
            var uri = new Uri("https://www.google-analytics.com/collect");
            var payLoad = $"v=1&t=event&tid=UA-212236883-1&cid=101508502472913349913&ec=CurrencyExchange&ea=CurrentRate&el={value}&ev={value * 1000:00000}";
            var response = await client.PostAsync(uri, new StringContent(payLoad, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ToString());
            }

            var payLoadCommerce = $"v=1&t=pageview&tid=UA-212236883-1&cid=101508502472913349913&dp=%2Ftest&c1=33%2C465&ti=testtransaction01&in=itemName01&ip={value}";
            var responseCommerce = await client.PostAsync(uri, new StringContent(payLoadCommerce, Encoding.UTF8, "application/json"));
            if (!responseCommerce.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ToString());
            }
        }

        public async Task<double> GetValue()
        {
            var uri = new Uri("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
            var streamTask = client.GetStreamAsync(uri);
            var responseResult = await JsonSerializer.DeserializeAsync<List<ExchangeRateModel>>(await streamTask);
            return responseResult.SingleOrDefault(x => x.cc == "USD").rate;
        }
    }

    public class ExchangeRateModel
    {
        public int r030 { get; set; }
        public string txt { get; set; }
        public double rate { get; set; }
        public string cc { get; set; }
        public string exchangedate { get; set; }
    }
}
