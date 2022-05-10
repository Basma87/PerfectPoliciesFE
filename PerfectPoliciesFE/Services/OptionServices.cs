using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PerfectPoliciesFE.Models;

namespace PerfectPoliciesFE.Services
{
    public class OptionServices
    {
        private static HttpClient _client;


        public OptionServices()
        {
            if (_client==null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("https://localhost:44328/api/");
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        /// get all Options

        public List<Option> GetAllOptions()
        {
            var response = _client.GetAsync("Options").Result;
            var OptionsList = response.Content.ReadAsAsync<List<Option>>().Result;

            return OptionsList;
        }
        /// Get sigle option
        public Option GetSingleOption(int id)
        {
            var response = _client.GetAsync($"Options/{id}").Result;
            var Option = response.Content.ReadAsAsync<Option>().Result;
            return Option;

        }

        /// create new Option
        public void CreateNewOption(Option newOption)
        {
            var response = _client.PostAsJsonAsync("Options",newOption).Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
        }
        /// update option 
        public void UpdateOption(int id, Option newOption)
        {
            var response = _client.PutAsJsonAsync($"Options/{id}",newOption).Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
        }
        /// delete Option
        public void deleteOption(int id)
        {
            var response = _client.DeleteAsync($"Options/{id}").Result;
#if DEBUG
            response.EnsureSuccessStatusCode();
#endif
        }


    }
}
