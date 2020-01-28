using CacheCow.Client;
using CacheCow.Client.RedisCacheStore;
using System;
using System.Net.Http;

namespace ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Clear();
            var client = ClientExtensions.CreateClient(new RedisStore("localhost:6379"));
            while (true)
            {
                var response = client.GetAsync("http://localhost:1337/serverstatus").Result;
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(content);
                if (Console.ReadLine() == "done") break;
            }
        }
    }
}
