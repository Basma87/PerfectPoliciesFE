using Microsoft.AspNetCore.Http;
using PerfectPoliciesFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Services
{
    public class QuizServices
    {

        public static HttpClient _client;
         private readonly HttpContext _context;

   
        public QuizServices()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("https://localhost:44328/api/");
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            }
        }




            public List<Quiz> GetAllQuizes()
        {
            var response = _client.GetAsync("Quiz").Result;
            var quizList = response.Content.ReadAsAsync<List<Quiz>>().Result;

            return quizList;
        }

        public List<Quiz> GetQuizzesByOrganizationName(string orgName)
        {
            var response = _client.GetAsync($"Quiz/GetQuizzesByOrganizationName/{orgName}").Result;
            var quizList = response.Content.ReadAsAsync<List<Quiz>>().Result;

            return quizList;
        }

        public List<Question> GetAllQuestionsOfQuiz(int id)
        {

            var response = _client.GetAsync($"Quiz/getAllQuestionsOfQuiz/{id}").Result;
            var questionsList = response.Content.ReadAsAsync<List<Question>>().Result;

            return questionsList;
        }
        public Quiz GetSingleQuiz(int id)
        {
            var respose = _client.GetAsync($"Quiz/{id}").Result;
            var quiz = respose.Content.ReadAsAsync<Quiz>().Result;

            return quiz;
        }


        public void CreateQuiz(Quiz newQuiz)
        {
           
            var response = _client.PostAsJsonAsync("Quiz",newQuiz).Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
        }


        public void UpdateQuiz(int id, Quiz NewQuiz)
        {
            var response = _client.PutAsJsonAsync($"Quiz/{id}",NewQuiz).Result;
#if DEBUG
                response.EnsureSuccessStatusCode();
#endif
        }


        public void DeleteQuiz(int id)
        {
            var response = _client.DeleteAsync($"Quiz/{id}").Result;

            response.EnsureSuccessStatusCode();

            }
    }

    
}
