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

/// <summary>
/// contrller that handles user login & session and token generation for logged in user.
/// </summary>
namespace PerfectPoliciesFE.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _client;
       
        /// <summary>
        /// inject API connection
        /// </summary>
        /// <param name="httpClientFactory"> http client factory object</param>
        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("APIClient");
         
        }

        /// <summary>
        /// method that loads login form view
        /// </summary>
        /// <returns> login form view</returns>
        public IActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// method that authenticate user and create sessoin for him.
        /// </summary>
        /// <param name="userInfo">User object</param>
        /// <param name="url"> url of page that user already in </param>
        /// <returns> page </returns>
        [HttpPost]
        public IActionResult Login(UserInfo userInfo,string url)
        {
            // return success / failure status code for user login.
            HttpResponseMessage result = _client.PostAsJsonAsync("Token/GenerateToken",userInfo).Result;

            // if success login, then add token and loggein username to session information
            if (result.IsSuccessStatusCode)
            {
              
                var token = result.Content.ReadAsStringAsync().Result;

                HttpContext.Session.SetString("Token", token.Trim('"'));

                HttpContext.Session.SetString("organizationName", userInfo.UserName);

                // if user URL is not empty, redirect to page he is navigating from , else reditect to index page.

                if (!String.IsNullOrEmpty (HttpContext.Session.GetString("url")))
                {
                    return Redirect(HttpContext.Session.GetString("url"));
                }

                return RedirectToAction("Index", "Home");
               
            }

            return View();
        }

        /// <summary>
        /// method tha cleas session after loggin out.
        /// </summary>
        /// <returns> index page</returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
