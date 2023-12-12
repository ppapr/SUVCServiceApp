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

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;

        public RequestsPage()
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadDataGrid<ResponseRequests>(listRequests, "Requests");
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
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
            }
        }
    }
}
