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
using Newtonsoft.Json;
using System.Net.Http.Headers;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Controller;
using SUVCServiceApp.Windows.ChangeWindowsAdministrator;

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        public ResponseRequests selectedRequest;
        public RequestsPage()
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadData();
        }

        private async void LoadData()
        {
            await dataGridLoader.LoadData<ResponseRequests>(listRequests, "Requests");
        }

        private async void buttonAddTask_Click(object sender, RoutedEventArgs e)
        {
            ResponseRequests request = new ResponseRequests
            {
                Description = textBoxTask.Text,
                DateCreateRequest = DateTime.Now,
                DateExecuteRequest = DateTime.Parse("01.01.0001"),
                IDUserRequest = 1,
                IDExecutorRequest = 10,
                IDStatus = 1,
                IDPriority = 2,
                IDEquipment = 42,
            };
            bool isSuccess = await apiDataProvider.AddDataToApi("Requests", request);
            if (isSuccess)
            {
                MessageBox.Show($"Задача успешно добавлена!");
                LoadData();
                textBoxTask.Clear();
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
            }
        }

        private async void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearch.Text;
            Func<ResponseRequests, string> searchProperty = item => item.Description;
            await dataGridLoader.LoadData(listRequests, "Requests", searchProperty, searchTerm);
        }

        private void buttonSetExecutor_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRequest != null)
            {
                ChangeExecutor changeWindow = new ChangeExecutor(selectedRequest);
                changeWindow.Closed += (s, args) =>
                {
                    LoadData();
                };
                changeWindow.ShowDialog();
            }
            else
                MessageBox.Show("Выберите заявку!");
        }

        private void listRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedRequest = (ResponseRequests)listRequests.SelectedItem;
        }

        private async void buttonCloseRequest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                int currentRequestID = selectedRequest.ID;
                ResponseRequests request = new ResponseRequests
                {
                    ID = selectedRequest.ID,
                    Description = selectedRequest.Description,
                    DateCreateRequest = selectedRequest.DateCreateRequest,
                    DateExecuteRequest = selectedRequest.DateExecuteRequest,
                    IDStatus = 4,
                    IDPriority = selectedRequest.IDPriority,
                    IDEquipment = selectedRequest.IDEquipment,
                    IDUserRequest = selectedRequest.IDUserRequest,
                    IDExecutorRequest = selectedRequest.IDExecutorRequest
                };

                bool isSuccess = await apiDataProvider.UpdateDataToApi("Requests", currentRequestID, request);
                if (isSuccess)
                {
                    MessageBox.Show($"Заявка отклонена!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
                }
            }
            catch
            {
                MessageBox.Show("Проверьте заполненность данных и соеднинение с интернетом!");
            }
        }
    }
}
