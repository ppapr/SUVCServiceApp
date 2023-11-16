using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using SUVCServiceApp.ViewModel;

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {
        public RequestsPage()
        {
            InitializeComponent();
        }

        private const string ApiBaseUrl = "http://yourapiurl/api/";

        private async Task<List<ResponseRequests>> GetRequestsFromApi()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("Requests");
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    List<ResponseRequests> requests = JsonConvert.DeserializeObject<List<ResponseRequests>>(responseData);
                    return requests;
                }
                else
                {
                    return null;
                }
            }
        }

        private async void LoadDataGrid()
        {
            List<ResponseRequests> requests = await GetRequestsFromApi();
            if (requests != null)
            {
                dataGridRequests.ItemsSource = requests;
            }
        }
    }
}
