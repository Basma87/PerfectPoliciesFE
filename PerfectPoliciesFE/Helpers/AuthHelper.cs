using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Helpers
{
    public static class AuthHelper
    {
        /// <summary>
        /// method that checks if user logged in or not by checking if session contains token
        /// if user not logged, he will be redirected to login page and
        /// the current page he is in will be saved to be redirected to it after he login
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool isNotLoogedIN(HttpContext context)
        {
            if (!context.Session.Keys.Any(c => c.Equals("Token")))
            {
                var url = context.Request.GetEncodedUrl();
                context.Session.SetString("url", url);
                return true;
            }
  
            return false;
        }
    }
}
