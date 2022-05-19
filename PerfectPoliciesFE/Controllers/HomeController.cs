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


/// <summary>
/// controller that handling home page methods
/// </summary>
namespace PerfectPoliciesFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAPIRequest<Quiz> _quizService;
       
        /// <summary>
        /// inject logger interface instance and quiz service that access API methods.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="quizService"></param>
        public HomeController(ILogger<HomeController> logger,IAPIRequest<Quiz> quizService)
        {
            _logger = logger;
         
            _quizService = quizService;
        }

        /// <summary>
        /// return home page index view
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        ///// <summary>
        ///// retur
        ///// </summary>
        ///// <returns></returns>
        //public IActionResult Create()
        //{
        //    return View();
        //}
      
        /// <summary>
        /// method that retrieves quizzes ralted to a certain organization
        /// </summary>
        /// <param name="collection">collection of form input fields to extract organization name </param>
        /// <returns>list of quizzes</returns>
        public IActionResult GetQuizzesByOrganizationName(IFormCollection collection)   
        {
            var orgName = collection["orgNametxtInput"].ToString();  /// get organization name from view 
            HttpContext.Session.SetString("organizationName",orgName);

         
            var QuizList = _quizService.GetChilrenforParentName("Quiz", "GetQuizzesByOrganizationName", orgName);
            return View(QuizList);
          
        }

        /// <summary>
        /// return view of privacy page
        /// </summary>
        /// <returns>privace page</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// return view of help page
        /// </summary>
        /// <returns> help page</returns>
        public IActionResult Help()
        {
            return View();
        }


        /// <summary>
        /// return view of Error page
        /// </summary>
        /// <returns>Error page</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
