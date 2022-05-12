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

namespace PerfectPoliciesFE.Controllers
{
    public class QuizController : Controller
    {
     
        private readonly IAPIRequest<Quiz> _quizService;
        private string controllerName="Quiz";
        private readonly IAPIRequest<QuestionsPerQuizViewModel> _reportService;



        public QuizController(IAPIRequest<Quiz> quizService, IAPIRequest<QuestionsPerQuizViewModel> reportService)
        {
            _quizService = quizService;
            _reportService = reportService;
        }
        // GET: QuizController
        public ActionResult Index()
        {
           
            var quizList = _quizService.getAll("Quiz");
            return View(quizList);
        }

        public ActionResult DisplayReport()
        {
            var reportData = _reportService.getAll("Report", "DataReport");


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

        public IActionResult ExportData()
        {
            // get the data for the report (find a way to improve this)
            var response = _reportService.getAll("Report", "DataReport");


            //create a memory stream
            var stream = new MemoryStream();

            // create stream writer to populate data to memory stream
            using (var writer =new StreamWriter(stream,leaveOpen:true))
            {
                // generate recirds that will be writen
                var csv = new CsvWriter(writer, CultureInfo.CurrentCulture,true);
                // write csv data by stream writer to the meory stream
                csv.WriteRecords(response);
            }

            stream.Position = 0; // point to the start of stream to read data 

            return File(stream,"application/octect-stram",$"ReportData_{DateTime.Now.ToString("ddMMM_HHmmss")}.csv");
        }



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

      
        // GET: QuizController/Create
        public ActionResult Create()
        {
            return View();
        }

       // POST: QuizController/Create
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

        // GET: QuizController/Edit/5
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

        // POST: QuizController/Edit/5
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

        // GET: QuizController/Delete/5
        public ActionResult Delete(int id)
        {
            if (AuthHelper.isNotLoogedIN(HttpContext))
            {
                return RedirectToAction("Login", "Auth");
            }
            var quiz = _quizService.getSingle(controllerName, id);

            return View(quiz);
        }

        // POST: QuizController/Delete/5
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
