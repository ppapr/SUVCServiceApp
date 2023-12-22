using SUVCServiceApp.Controller;
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

namespace SUVCServiceApp.Pages.EmployeePages
{
    /// <summary>
    /// Логика взаимодействия для ProfileEmployee.xaml
    /// </summary>

    public partial class ProfileEmployee : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        public ProfileEmployee()
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseRequests>(listRequests, $"Requests?userRequest={authenticatedUserId}");
        }
    }
}
