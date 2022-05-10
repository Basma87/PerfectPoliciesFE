using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Controllers
{
    public class QuestionController : Controller
    {


        private readonly IAPIRequest<Quiz> _quizService;
        private readonly IAPIRequest<Question> _questionService;
        private string controllerName = "Question";
        private IWebHostEnvironment _hostingEnvironment;



        public QuestionController(IAPIRequest<Question> questionService, IAPIRequest<Quiz> quizService, IWebHostEnvironment hostEnvironment)
        {
            _questionService = questionService;
            _quizService = quizService;
            _hostingEnvironment = hostEnvironment;  

        }

      

        private void CreateViewBag()
        {
            var allQuizes = _quizService.getAll("Quiz");

            var QuizesSelect = allQuizes.Select(c => new SelectListItem
            {
                Value = c.QuizID.ToString(),
                Text = c.QuizTitle

            }).ToList();

            ViewBag.AllQuizes = QuizesSelect;
        }

        //GET: QuestionController
        public ActionResult Index()
        {
            var questionList = _questionService.getAll(controllerName);
            return View(questionList);
        }

        // GET: QuestionController/Details/5
        public ActionResult Details(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login","Auth");
            }
            var question = _questionService.getSingle(controllerName, id);
            
            return View(question);
        }

        public ActionResult GetAllQuestions(int id)
        {
          
            var questionList = _questionService.GetChildrenforParentID(controllerName, "getAllQuestionsOfQuiz", id);
            return View("index",questionList);
        }

        public ActionResult quizQuestions(int id)
        {
            var allQuizes = _quizService.getAll("Quiz");

            var questionsList = _questionService.GetChildrenforParentID(controllerName, "getAllQuestionsOfQuiz", id);
            int quizID = questionsList.Select(c => c.QuizID).FirstOrDefault();
            var quizName = allQuizes.Where(c=> c.QuizID==quizID).Select(c=>c.QuizTitle).FirstOrDefault();

            ViewBag.quizName = quizName;
            return View("quizQuestions", questionsList);

        }

     
        // GET: QuestionController/Create
        public ActionResult Create()
        {
            CreateViewBag();

            return View();
        }

        // POST: QuestionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question newQuestion)
        {
            

            try
            {
                _questionService.Create(controllerName, newQuestion);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionController/Edit/5
        public ActionResult Edit(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                CreateViewBag();
                var question = _questionService.getSingle(controllerName, id);

                return View(question);
            }
         
        }

        

        // POST: QuestionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Question newQuestion)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            else

            try
            {
                _questionService.Edit(controllerName, id, newQuestion);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
              return  RedirectToAction("Login", "Auth");
            }
            var question = _questionService.getSingle(controllerName, id);
            return View(question);
        }

        // POST: QuestionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Question question)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                try
                {
                    _questionService.Delete(controllerName, id);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }   
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string folderRoot = Path.Combine(_hostingEnvironment.ContentRootPath, "WWWroot\\Uploads");
            string filepath = Path.Combine(folderRoot, file.FileName);


            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            return Ok(new { success = true, message = "File Uploaded" });
        }
    }
}
