using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PerfectPoliciesFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _client;
       

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("APIClient");
         
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserInfo userInfo,string url)
        {
            HttpResponseMessage result = _client.PostAsJsonAsync("Token/GenerateToken",userInfo).Result;

            if (result.IsSuccessStatusCode)
            {
              
                var token = result.Content.ReadAsStringAsync().Result;

                HttpContext.Session.SetString("Token", token.Trim('"'));

                HttpContext.Session.SetString("organizationName", userInfo.UserName);

                if (!String.IsNullOrEmpty (HttpContext.Session.GetString("url")))
                {
                    return Redirect(HttpContext.Session.GetString("url"));
                }

                return RedirectToAction("Index", "Home");
               
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
