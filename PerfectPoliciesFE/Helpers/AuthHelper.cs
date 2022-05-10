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
