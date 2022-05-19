using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


/// <summary>
/// controller that handles methods of option module
/// </summary>
namespace PerfectPoliciesFE.Controllers
{
    public class OptionController : Controller
    {
        // private static OptionServices _service;

        private readonly IAPIRequest<Option> _optionService;
        private readonly IAPIRequest<Question> _questionService;
        private string controllerName = "Options";
        private string endpoint;


        private static List<Question> questionList;
   
      
        /// <summary>
        /// inject option and question service.
        /// </summary>
        /// <param name="optionService"> option service</param>
        /// <param name="questionService"> question service</param>
        public OptionController(IAPIRequest<Option> optionService, IAPIRequest<Question> questionService)
        {
            _optionService = optionService;
            _questionService = questionService;
            questionList = _questionService.getAll("Question");
        }

        /// <summary>
        /// Method that retrieves all options to calculate latest option number will be set during new option creation
        /// creates select list of questions that will be loaded in the drop down list during new option creation
        /// </summary>
        private void createViewBag()
        {
            var options = _optionService.getAll("Options");
           
            var questions = _questionService.getAll("Question");

            // get laetst option number and increment by 1.
            var latestOption = options.Select(c => c.OptionNumber).LastOrDefault();
            latestOption = latestOption + 1;
        
         
            /// Questions that will be loaded in the drop dow list.
            var tempList = questions.Select(c=>new SelectListItem { 
            Value=c.QuestionID.ToString(),
            Text=c.QuestionText
            
            }).ToList();

           // send latest option number to the view to be loaded there.
            ViewBag.latestoptionNumber = latestOption;

            // send questios list the view.
            ViewBag.allQuestions = tempList;
         
        }

        /// <summary>
        /// method to display options index page that contains list of all options
        /// </summary>
        /// <returns> view of options list</returns>
        public ActionResult Index()
        {
            var OptionsList = _optionService.getAll(controllerName);
            return View(OptionsList);
        }

        /// <summary>
        /// Method that retrieves all options for a certain question
        /// </summary>
        /// <param name="id">question id</param>
        /// <returns> view of list of options of a question</returns>
        public ActionResult optionsForQuestion(int id)
        {
            var optionsList = _optionService.GetChildrenforParentID(controllerName, "QuestionOptions", id);

            var questionName = questionList.Where(c => c.QuestionID == id).Select(c => c.QuestionText).FirstOrDefault();

            ViewBag.questioName = questionName;
         
            return View(optionsList);
        }

       
        /// <summary>
        /// method to display details of a certain option
        /// </summary>
        /// <param name="id"> option ID</param>
        /// <returns> option details page</returns>
        public ActionResult Details(int id)
        {
            // checks if user not logged in , redirect  to login page, else open details page for him.
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login","Auth");
            }
            
            var Option=_optionService.getSingle(controllerName, id);
            return View(Option);
        }

        /// <summary>
        /// method to return view of create option page.
        /// </summary>
        /// <returns> options list</returns>
        public ActionResult Create()
        {
            createViewBag();
            return View();
        }

        /// <summary>
        /// method to create new option into database.
        /// </summary>
        /// <param name="newOption">new option object</param>
        /// <returns>updated option index view page</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Option newOption )
        {
            try
            {
                _optionService.Create(controllerName, newOption);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// method that loads option edit page
        /// </summary>
        /// <param name="id"> option ID</param>
        /// <returns> option edit page </returns>
        public ActionResult Edit(int id)
        {
            // checks if user not logged in , redirect  to login page, else open Edit page for him.
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }

            else
            {
                var option = _optionService.getSingle(controllerName, id);
                return View(option);
            }
           
        }

       /// <summary>
       /// method that saves edited option in the database
       /// </summary>
       /// <param name="id">option ID</param>
       /// <param name="newOption"> new option data</param>
       /// <returns> updated option</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Option newOption)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                try
                {
                    _optionService.Edit(controllerName, id, newOption);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
        }

      /// <summary>
      /// method that return delete option view page.
      /// </summary>
      /// <param name="id"> option ID</param>
      /// <returns>delete page</returns>
        public ActionResult Delete(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
               
   
                return RedirectToAction("Login", "Auth");
           }
            else
            {
             
                var option = _optionService.getSingle(controllerName, id);
                return View(option);
            }
        }

      /// <summary>
      /// method that delete option from the database
      /// </summary>
      /// <param name="id"> optio ID</param>
      /// <param name="option">option to be deleted</param>
      /// <returns>updated option index page afteroption deletion</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Option option)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                
                return RedirectToAction("Login","Auth");
            }
            try
            {
                _optionService.Delete(controllerName,id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
