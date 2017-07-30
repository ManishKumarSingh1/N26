using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ApiTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rndAmount = new Random();
            Random rndTimestamp = new Random();
            int count = 0;
            while (count < 30)
            {
                double amount = rndAmount.NextDouble() + rndAmount.Next(1, 100);
                DateTime time = DateTime.UtcNow.AddSeconds(rndTimestamp.Next(-100, 15));
                long timestamp = UtcHelper.ToUnixTime(time);
                string json = @"{
                                'Amount': '" + amount + @"',
                                'TimeStamp': '" + timestamp + @"'
                            }
                           ";
                JObject jobject = JObject.Parse(json);
                PostRequest(count, jobject);
                count++;
                Thread.Sleep(new TimeSpan(0, 0, 1));
                if (count % 3 == 0)
                {
                    GetRequest(count);
                    count++;
                }
            }
            Console.ReadLine();
        }
        static async void PostRequest(int requestId, JObject jObject)
        {
            string url = "http://localhost:55329/api/N26/transactions";
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            Console.WriteLine("RequestId: " + requestId + Environment.NewLine +
                              "Request: " + jObject.ToString() +
                              " Status:" + response.StatusCode.ToString());
        }

        static async void GetRequest(int requestId)
        {
            string url = "http://localhost:55329/api/N26/statistics";
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var contents = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RequestId: " + requestId + Environment.NewLine +
                              "Response: " + contents +
                              " Status:" + response.StatusCode.ToString());
        }
    }
}
