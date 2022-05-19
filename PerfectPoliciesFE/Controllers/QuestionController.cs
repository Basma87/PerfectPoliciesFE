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
        private IWebHostEnvironment _hostingEnvironment; // variable that hlds information about hosting evironment that application in it.
        private static List<Quiz> allQuizes;


        /// <summary>
        /// injecting hosting environment, quiz service and question service.
        /// </summary>
        /// <param name="questionService"></param>
        /// <param name="quizService"></param>
        /// <param name="hostEnvironment"></param>
        public QuestionController(IAPIRequest<Question> questionService, IAPIRequest<Quiz> quizService, IWebHostEnvironment hostEnvironment)
        {
            _questionService = questionService;
            _quizService = quizService;
            _hostingEnvironment = hostEnvironment;  

        }

      
        /// <summary>
        /// method that loads all quizzes to be displayed in the view pages.
        /// </summary>
        private void CreateViewBag()
        {
           
            allQuizes = _quizService.getAll("Quiz");

            var QuizesSelect = allQuizes.Select(c => new SelectListItem
            {
                Value = c.QuizID.ToString(),
                Text = c.QuizTitle

            }).ToList();

            ViewBag.AllQuizes = QuizesSelect;
        }

      /// <summary>
      /// method that loads question index page 
      /// </summary>
      /// <returns></returns>
        public ActionResult Index()
        {
            var questionList = _questionService.getAll(controllerName);
            return View(questionList);
        }

        /// <summary>
        /// method that gets details of a certain question
        /// </summary>
        /// <param name="id"> question ID</param>
        /// <returns> view of a question details</returns>
        public ActionResult Details(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login","Auth");
            }
            var question = _questionService.getSingle(controllerName, id);
            
            return View(question);
        }



        /// <summary>
        /// method that gets all questions relatedto a quiz 
        /// </summary>
        /// <param name="id">quiz ID </param>
        /// <returns> view page of questions of a quiz </returns>
        public ActionResult quizQuestions(int id)
        {
        
            allQuizes = _quizService.getAll("Quiz");

            var questionsList = _questionService.GetChildrenforParentID(controllerName, "getAllQuestionsOfQuiz", id);
            int quizID = questionsList.Select(c => c.QuizID).FirstOrDefault();
            var quizName = allQuizes.Where(c=> c.QuizID==quizID).Select(c=>c.QuizTitle).FirstOrDefault();

            ViewBag.quizName = quizName;
            return View("quizQuestions", questionsList);

        }


        /// <summary>
        /// method that renders create question page.
        /// </summary>
        /// <returns> create question page</returns>
        public ActionResult Create()
        {
            CreateViewBag();  // loads quizzes in the select list

            return View();
        }

        /// <summary>
        /// method that creates new Question
        /// </summary>
        /// <param name="newQuestion"> new option object</param>
        /// <returns> questions index page</returns>
        
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

        /// <summary>
        /// method that renders Edit question page
        /// </summary>
        /// <param name="id"> question Id</param>
        /// <returns> question edit page</returns>
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

        

        /// <summary>
        /// method that saves eited Question into the database
        /// </summary>
        /// <param name="id"> question ID</param>
        /// <param name="newQuestion"> new Question values</param>
        /// <returns> questions index page</returns>
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

        /// <summary>
        /// method that renders question delete page
        /// </summary>
        /// <param name="id"> question ID</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
              return  RedirectToAction("Login", "Auth");
            }
            var question = _questionService.getSingle(controllerName, id);
            return View(question);
        }

        /// <summary>
        /// method that deletes question from database.
        /// </summary>
        /// <param name="id"> question ID</param>
        /// <param name="question"> question object to be deleted</param>
        /// <returns> question index page </returns>
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

        /// <summary>
        /// method that implement drag & drop functionality, by uploading image to the server
        /// </summary>
        /// <param name="file"> file object</param>
        /// <returns> success message </returns>
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            
            string folderRoot = Path.Combine(_hostingEnvironment.ContentRootPath, "WWWroot\\Uploads");
            string filepath = Path.Combine(folderRoot, file.FileName);


            using ( var stream = new FileStream(filepath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            return Ok(new { success = true, message = "File Uploaded" });
        }
    }
}
