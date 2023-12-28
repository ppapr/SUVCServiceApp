using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows;

namespace SUVCServiceApp.Controller
{
    public class ApiDataProvider
    {
        private const string ApiBaseUrl = "http://localhost:61895/api/";

        public async Task<List<T>> GetDataFromApi<T>(string apiEndpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(apiEndpoint);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    List<T> data = JsonConvert.DeserializeObject<List<T>>(responseData);
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> AddDataToApi<T>(string apiEndpoint, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                                
                HttpResponseMessage response = await client.PostAsync(apiEndpoint, content);
                //string responseContent = await response.Content.ReadAsStringAsync();
                //MessageBox.Show(responseContent);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> UpdateDataToApi<T>(string apiEndpoint, int id, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"{apiEndpoint}/{id}", content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteDataFromApi<T>(string apiEndpoint, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"{apiEndpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<List<T>> SearchData<T>(string apiEndpoint, Func<T, string> searchProperty, string searchTerm)
        {
            List<T> data = await GetDataFromApi<T>(apiEndpoint);
            if (data != null)
            {
                var filteredData = data.Where(item =>
                {
                    string value = searchProperty(item);
                    return value != null && value.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                }).ToList();

                return filteredData;
            }
            return null;
        }
    }
}
