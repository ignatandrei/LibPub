using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LibGenerateInfo
{
    public class Program
    {
        public static async Task<bool> GetDatabase()
        {
            var url = "https://raw.githubusercontent.com/ignatandrei/LibPub/master/data/data.sqlite3";
            using (var client = new HttpClient())
            {

                using (var result = await client.GetAsync(url))
                {
                    if (result.IsSuccessStatusCode)
                    {
                        var bytes = await result.Content.ReadAsByteArrayAsync();
                        File.WriteAllBytes("data.sqlite3", bytes);
                    }

                }
            }
            return true;
        }
        public static void Main(string[] args)
        {
            if (!File.Exists("data.sqlite3"))
            {
                var q =GetDatabase().GetAwaiter().GetResult();   
            }
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
