using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibGenerateInfo.Models;
using LibTimeCreator;
using LibInfoBook;
using System.Collections.Concurrent;
using LibQRDAL.Models;

namespace LibGenerateInfo.Controllers
{
    
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            
            if (!System.IO.File.Exists("data.sqlite3"))
            {
            try{
                await Program.GetDatabase();   
                }
                catch(Exception ex){
                return Content( ex.Message);
                }
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> AllBook([FromServices]QRContext context, string id)
        {
            var b = context.Book.FirstOrDefault(it => it.UniqueLink == id);
            if(b== null)
            {
                return Content("no book");
            }
            return View(b);
        }
        public async Task<ActionResult> Book(string id)
        {
            Guid g = Guid.Parse(id);
            //TODO: replace with database
            if(!dict.ContainsKey(g))
                return Content("not found");

            string base64 = dict[g];
            //var lines = await System.IO.File.ReadAllLinesAsync("a.txt");
            //foreach(var line in lines)
            //{
            //    if (line.StartsWith(id + "-"))
            //    {
            //        base64 = line.Replace(id + "-", "");
            //        break;
            //    }
            //}
            //if (base64 == "")
            //    return Content("not found");

            var timeInfo = LibTimeInfo.FromString(base64);
            if (!timeInfo.IsValid())
            {
                return Content("nu aveti dreptul sa vedeti cartea");
            }
            string time = "";
            var ts = timeInfo.Diff();
            if(ts.TotalMinutes<1)
            {
                time = "secunde ramase " + ts.TotalSeconds.ToString("0#");
            }
            else
            {
                time = "minute ramase " + ts.TotalMinutes.ToString("0#");
            }
            var b = new InfoBook(timeInfo.Info);
            await b.GetInfoFromId();
            
            return Content(" aici apare cartea " + b.Title + ";"+time);
        }
        static ConcurrentDictionary<Guid, string> dict = new ConcurrentDictionary<Guid, string>();
        public async Task<ActionResult> GenerateCode(int id, int minutes)
        {
            
            var li = new LibTimeInfo(minutes);
            li.Info = id.ToString();
            //TODO: replace with database
            var t = Guid.NewGuid();//.ToString("N");
            //await System.IO.File.AppendAllTextAsync("a.txt", t + "-" + li.Generate());
            dict[t] = li.Generate();
            li.Info = t.ToString("N");
            var b = new InfoBook(id.ToString());
            await b.GetInfoFromId();
            
            return View(new Tuple<LibTimeInfo,InfoBook>(li,b));


        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
