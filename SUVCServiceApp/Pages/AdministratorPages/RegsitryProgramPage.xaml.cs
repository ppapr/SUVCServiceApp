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
        int currentPage = 1;
        int sizePage = 20;
        int maxPages = 0;
        public RegsitryProgramPage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            dataGridLoader = new DataGridLoader(apiDataProvider);
            this.administratorWindow = administratorWindow;
            LoadData(currentPage,sizePage);
            comboBoxSpecialization.SelectedIndex = 0;
        }

        private async Task LoadData(int currentPage, int sizePage)
        {
            labelPage.Content = currentPage.ToString();
            await dataGridLoader.LoadData<ResponseRegistry>(listPrograms, "RegistryPrograms", currentPage, sizePage);
            var countRegistry = await apiDataProvider.GetDataFromApi<ResponseRegistry>("RegistryPrograms");
            MessageBox.Show(countRegistry.Count.ToString());
            maxPages = (int)Math.Ceiling(countRegistry.Count * 1.0 / sizePage);
            if (currentPage == maxPages)
                buttonNextPage.IsEnabled = false;
            if (currentPage == 1)
                buttonPreviousPage.IsEnabled = false;
            else { buttonNextPage.IsEnabled = true; buttonPreviousPage.IsEnabled = true; }
        }

        private void buttonAddProgram_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new AddProgramPage(administratorWindow));
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
                    await LoadData(currentPage,sizePage);
            }
        }
        ResponseRegistry currentProgram;
        private async void buttonDeleteProgram_Click(object sender, RoutedEventArgs e)
        {
            if (currentProgram != null)
            {
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить программу {currentProgram.NameProgram}?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await apiDataProvider.DeleteDataFromApi<ResponseSpare>("RegistryPrograms", currentProgram.ID);
                    MessageBox.Show("Удаление завершено!");
                    await LoadData(currentPage,sizePage);
                }
            }
            else MessageBox.Show("Выберите программу!");
        }

        private void listPrograms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentProgram = (ResponseRegistry)listPrograms.SelectedItem;
        }
        private async void buttonPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                await LoadData(currentPage, sizePage);
            }
        }

        private async void buttonNextPage_Click(object sender, RoutedEventArgs e)
        {
            currentPage++;
            await LoadData(currentPage, sizePage);
        }
    }
}
