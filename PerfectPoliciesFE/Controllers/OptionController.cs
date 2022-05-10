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

namespace PerfectPoliciesFE.Controllers
{
    public class OptionController : Controller
    {
        // private static OptionServices _service;

        private readonly IAPIRequest<Option> _optionService;
        private readonly IAPIRequest<Question> _questionService;
        private string controllerName = "Options";
        private string endpoint;


        private static List<Question> qList;
   
      

        public OptionController(IAPIRequest<Option> optionService, IAPIRequest<Question> questionService)
        {
            _optionService = optionService;
            _questionService = questionService;
            qList = _questionService.getAll("Question");
        }

        private void createViewBag()
        {
            var options = _optionService.getAll("Options");
           
           // var questions = _questionService.getAll("Question");


            var latestOption = options.Select(c => c.OptionNumber).LastOrDefault();
            latestOption = latestOption + 1;
        
         
            /// Questions that will be loaded in the drop dow list.
            var questionList = qList.Select(c=>new SelectListItem { 
            Value=c.QuestionID.ToString(),
            Text=c.QuestionText
            
            }).ToList();

           ViewBag.latestoptionNumber = latestOption;

            ViewBag.allQuestions = questionList;
         
        }

        // GET: OptionController
        public ActionResult Index()
        {
            var OptionsList = _optionService.getAll(controllerName);
            return View(OptionsList);
        }

        public ActionResult optionsForQuestion(int id)
        {
            var optionsList = _optionService.GetChildrenforParentID(controllerName, "QuestionOptions", id);

            var questionName = qList.Where(c => c.QuestionID == id).Select(c => c.QuestionText).FirstOrDefault();

            ViewBag.questioName = questionName;
         
            return View(optionsList);
        }


        // GET: OptionController/Details/5
        public ActionResult Details(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login","Auth");
            }
            var Option=_optionService.getSingle(controllerName, id);
            return View(Option);
        }

        // GET: OptionController/Create
        public ActionResult Create()
        {
            createViewBag();
            return View();
        }

        // POST: OptionController/Create
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

        // GET: OptionController/Edit/5
        public ActionResult Edit(int id)
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

        // POST: OptionController/Edit/5
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

        // GET: OptionController/Delete/5
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

        // POST: OptionController/Delete/5
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
