using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
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
using System.Windows.Shapes;

namespace SUVCServiceApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для MapRequestsWindow.xaml
    /// </summary>
    public partial class MapRequestsWindow : Window
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private ResponseUsers authenticatedUser;
        public MapRequestsWindow(ResponseUsers authenticatedUser)
        {
            InitializeComponent();
            this.authenticatedUser = authenticatedUser;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadData();
        }

        async Task LoadData()
        {
            List<ResponseRequests> requests = await apiDataProvider.GetDataFromApi<ResponseRequests>($"Requests?userExecutor={authenticatedUser.ID}");
            requests = requests.Where(r => r.IDStatus == 1 || r.IDStatus == 2).OrderByDescending(r => r.DateCreateRequest).ToList();
            dataGridLoader.LoadData(listRequests, requests);
        }
    }
}
