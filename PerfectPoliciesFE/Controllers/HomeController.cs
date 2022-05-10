using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAPIRequest<Quiz> _quizService;
       
        public HomeController(ILogger<HomeController> logger,IAPIRequest<Quiz> quizService)
        {
            _logger = logger;
         
            _quizService = quizService;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Create()
        {
            return View();
        }
      
        public IActionResult GetQuizzesByOrganizationName(IFormCollection collection)   
        {
            var orgName = collection["orgNametxtInput"].ToString();  /// get organization name from view 
            HttpContext.Session.SetString("organizationName",orgName);

         
            var QuizList = _quizService.GetChilrenforParentName("Quiz", "GetQuizzesByOrganizationName", orgName);
            return View(QuizList);
          
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Help()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
