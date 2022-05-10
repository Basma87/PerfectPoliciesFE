using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using PerfectPoliciesFE.Models;

namespace PerfectPoliciesFE.Services
{
    public class APIRequest<T> : IAPIRequest<T>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _context;

        public APIRequest(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClientFactory.CreateClient("APIClient");
            _context = httpContextAccessor.HttpContext;

            if (_context.Session.GetString("Token")!=null)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",_context.Session.GetString("Token"));
            }
        }

        public List<T> getAll(string controllerName)
        {
            var response = _client.GetAsync(controllerName).Result;

            var quizList = response.Content.ReadAsAsync<List<T>>().Result;

            return quizList;
        }


        /// <summary>
        /// Method to retrieve data to be displayed by chart
        /// </summary>
        /// <param name="controllerName">controllerName</param>
        /// <param name="endPoint">endpoint</param>
        /// <returns> quizzes list</returns>
        public List<T> getAll(string controllerName, string endPoint = "")

        {
            string requestURl;

            if (String.IsNullOrEmpty(endPoint))
            {
                requestURl = controllerName;
            }
            else
            {
                requestURl = $"{controllerName}/{endPoint}";
            }

            var response = _client.GetAsync(requestURl).Result;

            var quizzes = response.Content.ReadAsAsync<List<T>>().Result;

            return quizzes;
        }
        public T getSingle(string controllerName, int id)
        {
            var response = _client.GetAsync($"{controllerName}/{id}").Result;

            var quiz = response.Content.ReadAsAsync<T>().Result;

            return quiz;
        }

        public List<T> GetChildrenforParentID(string controllerName, string Endpoint, int id)
        {
            var response = _client.GetAsync($"{controllerName}/{Endpoint}/{id}").Result;

            return response.Content.ReadAsAsync<List<T>>().Result;

        }

        public List<T> GetChilrenforParentName(string controllerName, string endPoint, string name)
        {
            var response = _client.GetAsync($"{controllerName}/{endPoint}/{name}").Result;
            var quizList = response.Content.ReadAsAsync<List<T>>().Result;

            return quizList;
        }

        public T Edit(string controllerName, int id, T entity)
        {
            var response = _client.PutAsJsonAsync($"{controllerName}/{id}",entity).Result;

            return response.Content.ReadAsAsync<T>().Result;
        }

        public T Create(string controllerName, T entity)
        {
            var response = _client.PostAsJsonAsync(controllerName, entity).Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
            return response.Content.ReadAsAsync<T>().Result;
        }


       


        public void Delete(string controllerName, int id)
        {
            var response = _client.DeleteAsync($"{controllerName}/{id}").Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif

        }

       
    }
}
