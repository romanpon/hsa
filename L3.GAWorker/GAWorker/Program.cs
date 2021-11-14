using System;
using System.Threading;

namespace GAWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var webClient = new WebClient();
            while (true)
            {
                var value = webClient.GetValue().Result;
                _ = webClient.SendValue(value);

                Thread.Sleep(60 * 1000);
            }
        }
    }
}
