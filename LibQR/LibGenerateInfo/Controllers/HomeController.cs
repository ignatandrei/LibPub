using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibGenerateInfo.Models;
using LibTimeCreator;
using LibInfoBook;

namespace LibGenerateInfo.Controllers
{
    
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        public async Task<ActionResult> Book(string id)
        {
            var book = LibTimeInfo.FromString(id);
            var b = new InfoBook(id.ToString());
            await b.GetInfoFromId();
            return Content(" aici apare cartea " + b.Title);
        }
        public async Task<ActionResult> GenerateCode(int id, int minutes)
        {
            var li = new LibTimeInfo(minutes);
            li.Info = id.ToString();
            
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
