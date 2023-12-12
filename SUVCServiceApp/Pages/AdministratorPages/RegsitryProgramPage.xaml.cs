using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
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

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegsitryProgramPage.xaml
    /// </summary>
    public partial class RegsitryProgramPage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private readonly AdministratorWindow administratorWindow;
        public RegsitryProgramPage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            this.administratorWindow = administratorWindow;
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadDataGrid<ResponseRegistry>(listPrograms, "RegistryPrograms");
            await dataGridLoader.LoadDataGrid<ResponseSpecialization>(comboBoxSpecialization, "Specializations");
        }

        private void buttonAddProgram_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new AddProgramPage(administratorWindow));
        }
    }
}
