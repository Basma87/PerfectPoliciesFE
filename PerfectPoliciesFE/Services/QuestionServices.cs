using PerfectPoliciesFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PerfectPoliciesFE.Services
{
    public class QuestionServices
    {
        private static HttpClient _client;


        public QuestionServices()
        {
            if (_client==null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("https://localhost:44328/api/");
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }


        public List<Question> GetAllQuestions()
        {
            var response = _client.GetAsync("Question").Result;
            var questionList = response.Content.ReadAsAsync<List<Question>>().Result;

            return questionList;
        }


        public Question GetSingleQuestion(int id)
        {
            var response = _client.GetAsync($"Question/{id}").Result;

            var question = response.Content.ReadAsAsync<Question>().Result;

            return question;
        }

        public void  createQuestion(Question newQuestion)
        {
            var response = _client.PostAsJsonAsync("Question",newQuestion).Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
        }


        public void updateQuestion(int id, Question newQuestion)
        {
            var response = _client.PutAsJsonAsync($"Question/{id}", newQuestion).Result;

#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
        }

        public void DeleteQuestion(int id)
        {
            var response = _client.DeleteAsync($"Question/{id}").Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
        }
    }


 
}
