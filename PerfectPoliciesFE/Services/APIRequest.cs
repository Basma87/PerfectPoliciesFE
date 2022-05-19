using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using PerfectPoliciesFE.Models;

/// <summary>
/// class that handles calls to backend API.
/// </summary>
namespace PerfectPoliciesFE.Services
{
    public class APIRequest<T> : IAPIRequest<T>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _context;

        public APIRequest(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            // save the injected HTTPClient to access API.
            _client = httpClientFactory.CreateClient("APIClient");
            //inject HTTPContext to manage session values.
            _context = httpContextAccessor.HttpContext;

            if (_context.Session.GetString("Token") != null)
            {
                // write token to authorization header
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _context.Session.GetString("Token"));
            }
        }

        /// <summary>
        /// Generc method that get list of values depend on called controller.
        /// </summary>
        /// <param name="controllerName"> controller name</param>
        /// <returns> list of objects </returns>
        public List<T> getAll(string controllerName)
        {
            var response = _client.GetAsync(controllerName).Result;

            var list = response.Content.ReadAsAsync<List<T>>().Result;

            return list;
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

            var list = response.Content.ReadAsAsync<List<T>>().Result;

            return list;
        }
        /// <summary>
        /// method to get data of certain object
        /// </summary>
        /// <param name="controllerName"> controller name</param>
        /// <param name="id"> object ID</param>
        /// <returns> object </returns>
        public T getSingle(string controllerName, int id)
        {
            var response = _client.GetAsync($"{controllerName}/{id}").Result;

            var item = response.Content.ReadAsAsync<T>().Result;

            return item;
        }

        /// <summary>
        /// method to get sub items of an object, for example options of a question
        /// </summary>
        /// <param name="controllerName"> controller name</param>
        /// <param name="Endpoint"> endPoint name</param>
        /// <param name="id"> ID of parent object</param>
        /// <returns></returns>
        public List<T> GetChildrenforParentID(string controllerName, string Endpoint, int id)
        {
            var response = _client.GetAsync($"{controllerName}/{Endpoint}/{id}").Result;

            return response.Content.ReadAsAsync<List<T>>().Result;

        }
        /// <summary>
        /// method to get sub items of an object, for example options of a question
        /// </summary>
        /// <param name="controllerName"> controller name </param>
        /// <param name="endPoint"> endPoint name</param>
        /// <param name="name"> name of parent object</param>
        /// <returns>list of values</returns>
        public List<T> GetChilrenforParentName(string controllerName, string endPoint, string name)
        {
            var response = _client.GetAsync($"{controllerName}/{endPoint}/{name}").Result;
            var list = response.Content.ReadAsAsync<List<T>>().Result;

            return list;
        }

        /// <summary>
        /// method to edit object values
        /// </summary>
        /// <param name="controllerName"> controller name</param>
        /// <param name="id"> object ID</param>
        /// <param name="entity">new object values</param>
        /// <returns> response status</returns>
        public T Edit(string controllerName, int id, T entity)
        {
            var response = _client.PutAsJsonAsync($"{controllerName}/{id}", entity).Result;

            return response.Content.ReadAsAsync<T>().Result;
        }


        /// <summary>
        ///  method to create new object
        /// </summary>
        /// <param name="controllerName"> controller name</param>
        /// <param name="entity"> object</param>
        /// <returns> response status</returns>
        public T Create(string controllerName, T entity)
        {
            var response = _client.PostAsJsonAsync(controllerName, entity).Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
            return response.Content.ReadAsAsync<T>().Result;
        }

        /// <summary>
        /// method to delete object 
        /// </summary>
        /// <param name="controllerName"> controller name</param>
        /// <param name="id"></param>
        public T Delete(string controllerName, int id)
        {
            var response = _client.DeleteAsync($"{controllerName}/{id}").Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
            return response.Content.ReadAsAsync<T>().Result;
        }


    }
}
