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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SUVCServiceApp.Pages.ITEmployeePages
{
    /// <summary>
    /// Логика взаимодействия для RequestITEmployeePage.xaml
    /// </summary>
    public partial class RequestITEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private int authenticatedUserId;

        public RequestITEmployeePage(int authenticatedUserId)
        {
            InitializeComponent();
            this.authenticatedUserId = authenticatedUserId;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseRequests>(listRequests, $"Requests?userExecutor={authenticatedUserId}");
            await dataGridLoader.LoadData<ResponseRequests>(listRequests, $"Requests?userExecutor=10");
        }
    }
}
