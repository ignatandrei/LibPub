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
using ReadEpub;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Text;
using HashidsNet;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace LibGenerateInfo.Controllers
{
    
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index([FromServices] QRContext context)
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
            var b = context.Book.Where(it => it.IsCorrect).ToArray();

            return View(b);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            //TODO: remember the book that he wants to see
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromServices]QRContext context,string emailUser, string password)
        {
            //TODO: make a better way to register users
            //TODO: use Google / Facebook authentication
            //TODO: email valid
            emailUser = emailUser?.ToLower();
            var hash = getHash(password);
            if (hash.Length > 500)
                hash=hash.Substring(0,500);

            var user=context.SimpleUser.FirstOrDefault(it => it.Email.ToLower() == emailUser);
            if(user != null)
            {
                if(!user.ConfirmedByEmail)
                    return Content($"email {emailUser} does exists . Check your email");

                if(user.Password == hash)
                {
                    await SignUser(user);
                    return RedirectToAction("Index");
                }
                else
                {
                    return Content("incorrect password. Press back and try again");
                }
            }
            user = new SimpleUser();
            user.Email = emailUser;
            user.Name = emailUser;
            user.Password = hash;
            context.Add(user);  
            await context.SaveChangesAsync();
            var strSalt=Environment.GetEnvironmentVariable("deploy");
            
            var hashids = new Hashids(strSalt);
            var id = hashids.EncodeLong(user.Iduser);
            string url = "http://fsq.apphb.com/Home/GeneratedEmail/" + id;
            var apiKey = Environment.GetEnvironmentVariable("SendGridKey");
            
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("cursval@infovalutar.ro","Ignat Andrei - QR");
            List<EmailAddress> tos = new List<EmailAddress>
            {
              new EmailAddress(emailUser),
              new EmailAddress("ignatandrei@yahoo.com")
            };
            
            var subject = "Confirmati inregistrarea la QR Code Library ";
            var htmlContent = $"Va rog <strong>confirmati </strong> inscrierea la QR Code library apasand <a href='{url}'>{url}</a>. <br />Multumim!";
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, false);
            var response = await client.SendEmailAsync(msg);
            
            return Content("Va rugam verificat emailul( inclusiv spam/ junk) pentru a confirma adresa de email !");
        }
        [AllowAnonymous]
        public async Task<ActionResult> GeneratedEmail([FromServices]QRContext context,string id)
        {
            var strSalt = Environment.GetEnvironmentVariable("deploy");
            
            var hashids = new Hashids(strSalt);
            long idUser = hashids.DecodeLong(id)[0];
            var user = context.SimpleUser.FirstOrDefault(it => it.Iduser == idUser);
            if(user == null)
            {
                return Content("user does not exists");
            }
            user.ConfirmedByEmail = true;
            await context.SaveChangesAsync();

            await SignUser(user);
            return RedirectToAction("index");

        }
        [Authorize]
        public ActionResult MyPage()
        {
            return View();
        }
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
        private async Task SignUser(SimpleUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("FullName", user.Email)
            };
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. Required when setting the 
                // ExpireTimeSpan option of CookieAuthenticationOptions 
                // set with AddCookie. Also required when setting 
                // ExpiresUtc.

                IssuedUtc = DateTimeOffset.UtcNow,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        public async Task<ActionResult> AllBook([FromServices]QRContext context, string id)
        {
            var b = context.Book.FirstOrDefault(it => it.UniqueLink == id);
            
            if(b== null)
            {
                return Content("no book");
            }
            string pathFile = Path.Combine("epubs",$"{b.UniqueLink}_{b.Idbook.ToString("00#")}.epub");
            if (!System.IO.File.Exists(pathFile))
            {
                var pathFileFirst = Path.Combine("epubs", b.UniqueLink);
                if (System.IO.File.Exists(pathFileFirst)){
                    System.IO.File.Copy(pathFileFirst, pathFile);
                    System.IO.File.Delete(pathFileFirst);
                }
                
            }
            if (!System.IO.File.Exists(pathFile))
            {
                return Content($"File for book {id} not exists");
            }

            var read = new ReadEpubFile(pathFile);
            return View(read);

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
        private static string getHash(string text)
        {
            
            using (var sha256 = SHA512.Create())
            {                
                var hashedBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(text));
                
                return BitConverter.ToString(hashedBytes).ToLower();
            }
        }
        public async Task<ActionResult> GenerateCode(int id, int minutes)
        {
            return Content("No longer available");
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
