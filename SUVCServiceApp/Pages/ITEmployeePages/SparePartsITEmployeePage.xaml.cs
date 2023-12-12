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
    /// Логика взаимодействия для SparePartsITEmployeePage.xaml
    /// </summary>
    public partial class SparePartsITEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        public SparePartsITEmployeePage()
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseSpare>(listSpares, "SparesEquipments");
        }
    }
}
