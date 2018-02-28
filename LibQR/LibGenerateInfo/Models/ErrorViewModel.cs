using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LibGenerateInfo.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class CustomAuth 
    {
        public CustomAuth(RequestDelegate next)
        {
            Next = next;
        }

        RequestDelegate Next { get; }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.ToLower().Contains("admin"))
            {
                context.Response.Redirect("/home/register");
                return;
            }
            await Next(context);
        }


    }
}