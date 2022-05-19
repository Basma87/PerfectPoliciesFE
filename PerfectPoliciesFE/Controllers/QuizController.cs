using ChartJSCore.Models;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfectPoliciesFE.Helpers;
using PerfectPoliciesFE.Models;
using PerfectPoliciesFE.Models.ViewModels;
using PerfectPoliciesFE.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

/// <summary>
/// controller that handles quiz moudule methods.
/// </summary>
namespace PerfectPoliciesFE.Controllers
{
    public class QuizController : Controller
    {
     
        private readonly IAPIRequest<Quiz> _quizService;
        private string controllerName="Quiz";
        private readonly IAPIRequest<QuestionsPerQuizViewModel> _reportService;
        private static List<QuestionsPerQuizViewModel> reportData;


        /// <summary>
        /// inject Quiz & report service.
        /// call method that rerieves report data as alist
        /// </summary>
        /// <param name="quizService"></param>
        /// <param name="reportService"></param>
        public QuizController(IAPIRequest<Quiz> quizService, IAPIRequest<QuestionsPerQuizViewModel> reportService)
        {
            _quizService = quizService;
            _reportService = reportService;
            reportData = _reportService.getAll("Report", "DataReport");
        }

        /// <summary>
        /// method that displays all quizzes.
        /// </summary>
        /// <returns> page displays quizzes list</returns>
        public ActionResult Index()
        {
           
            var quizList = _quizService.getAll("Quiz");
            return View(quizList);
        }

        /// <summary>
        /// method that represent report data as a 2D chart
        /// </summary>
        /// <returns> view with 2D chart</returns>
        public ActionResult DisplayReport()
        {
        
            Chart chart = new Chart();

            chart.Type = Enums.ChartType.Bar;

            ChartJSCore.Models.Data chartData = new ChartJSCore.Models.Data();

            chartData.Labels = reportData.Select(c => c.quizName).ToList();


            BarDataset dataset = new BarDataset()
            {
                Label = "Questions Count",
                Data = reportData.Select(c => (double?)c.questionsCount).ToList()
            };

            chartData.Datasets = new List<Dataset>();
            chartData.Datasets.Add(dataset);

            chart.Data = chartData;

            ViewData["chart"] = chart;

            return View("DisplayChart");

        }

        /// <summary>
        /// method that export report data represented on chart to a CSV file.
        /// </summary>
        /// <returns></returns>
        public IActionResult ExportData()
        {
            {

                //create a memory stream to store data in it.
                var stream = new MemoryStream();

                // create stream writer to wite data to memory stream
                using (var writer = new StreamWriter(stream, leaveOpen: true))
                {
                    // generate records that will be writen to the file.
                    var csv = new CsvWriter(writer, CultureInfo.CurrentCulture, true);
                    // write csv data by stream writer to the meory stream
                    csv.WriteRecords(reportData);
                }

                stream.Position = 0; // point to the start of stream to read data after finishing writing it.

                return File(stream, "application/octect-stram", $"ReportData_{DateTime.Now.ToString("ddMMM_HHmm")}.csv"); // CSV file is generated 
            }
        }

        /// <summary>
        /// method that renders edit Quiz page
        /// </summary>
        /// <param name="id"> quiz ID </param>
        /// <returns> edit Quiz page </returns>
        public ActionResult Details(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))   
            {
               return RedirectToAction("Login", "Auth");
            }
            else
            {
                var quiz = _quizService.getSingle(controllerName, id);
                return View(quiz);
            }
        }


        /// <summary>
        /// method that renders create quiz page.
        /// </summary>
        /// <returns> create quiz page. </returns>
        public ActionResult Create()
        {
            return View();
        }

       /// <summary>
       /// method hat create quiz and save it to the database.
       /// </summary>
       /// <param name="newQuiz"> new Quiz object</param>
       /// <returns> quiz index page </returns>
       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(Quiz newQuiz)
        {
            try
            {

             
                _quizService.Create(controllerName,newQuiz);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// method that renders Edit quiz page.
        /// </summary>
        /// <param name="id"> quiz ID</param>
        /// <returns> Edit quiz page. </returns>
        public ActionResult Edit(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
               
                var quiz = _quizService.getSingle(controllerName, id);
                return View(quiz);

            }
        }

        /// <summary>
        /// method that saves edited quiz into the database
        /// </summary>
        /// <param name="id"> quiz ID </param>
        /// <param name="NewQuiz"> new quiz values</param>
        /// <returns> quizzes index page with updated list of quizzes</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Quiz NewQuiz)
        {
            try
            {
                if (AuthHelper.isNotLoogedIN(HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                   
                    _quizService.Edit(controllerName, id, NewQuiz);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception)
            {

                return View();
            }

        }

        /// <summary>
        /// method that renders quiz deletion page
        /// </summary>
        /// <param name="id"> quiz ID</param>
        /// <returns>  quiz deletion page </returns>
        public ActionResult Delete(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            var quiz = _quizService.getSingle(controllerName, id);

            return View(quiz);
        }

        /// <summary>
        /// method that deletes quiz from the databse
        /// </summary>
        /// <param name="id"> quiz ID </param>
        /// <param name="quiz"> quiz object</param>
        /// <returns> quizzes index page with quizzes updated list</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Quiz quiz)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            { 
                try
                {
                    _quizService.Delete(controllerName, id);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
        }
    }
}
