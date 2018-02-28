using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibGenerateInfo.Views.Home
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public IActionResult Login()
        {
            return RedirectToAction("Register", "Home");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return RedirectToAction("Register", "Home");
        }
    }
}