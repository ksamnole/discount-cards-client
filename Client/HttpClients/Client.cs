using System;
using System.Net.Http;

namespace Client.HttpClients
{
    public static class Client
    {
        public static HttpClient HttpClient;

        static Client()
        {
            HttpClient = new HttpClient()
            {
                // BaseAddress = new Uri("http://10.0.2.2:5000/")
                BaseAddress = new Uri("http://130.193.52.4:5000/")
            };
        }
    }
}