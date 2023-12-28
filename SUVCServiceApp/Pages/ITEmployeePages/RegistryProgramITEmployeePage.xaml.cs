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
    /// Логика взаимодействия для RegistryProgramITEmployeePage.xaml
    /// </summary>
    public partial class RegistryProgramITEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        public RegistryProgramITEmployeePage()
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
            comboBoxSpecialization.SelectedIndex = 0;
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseRegistry>(listPrograms, "RegistryPrograms");
        }

        private async void textBoxSearchProgram_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearchProgram.Text;
            Func<ResponseRegistry, string> searchProperty = item => item.NameProgram;
            await dataGridLoader.LoadData(listPrograms, "RegistryPrograms", searchProperty, searchTerm);
        }

        private async void comboBoxSpecialization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxSpecialization.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedSpecialization = selectedItem.Content.ToString();
                if (!string.IsNullOrEmpty(selectedSpecialization) && selectedSpecialization != "Все")
                {
                    Func<ResponseRegistry, string> searchProperty = item => item.Specialization;
                    await dataGridLoader.LoadData(listPrograms, "RegistryPrograms", searchProperty, selectedSpecialization);
                }
                else
                    LoadDataGrid();
            }
        }
    }
}
