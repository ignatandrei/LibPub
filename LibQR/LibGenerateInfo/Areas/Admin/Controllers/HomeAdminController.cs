using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LibGenerateInfo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DownloadData()
        {
            var b = System.IO.File.ReadAllBytes("data.sqlite3");
            return new FileContentResult(b, "application/octet-stream");
            //return Content(System.IO.File.Exists("data.sqlite3").ToString());
        }
    }
    
}