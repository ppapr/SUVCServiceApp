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
using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
using SUVCServiceApp.Windows.ChangeWIndowsAdministrator;

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private readonly AdministratorWindow administratorWindow;
        public ResponseUsers selectedEmployee;
        public EmployeePage(AdministratorWindow administratorWindow)
        {
            InitializeComponent();
            this.administratorWindow = administratorWindow;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
            comboBoxCategoryUser.SelectedIndex = 0;
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseUsers>(listEmployees, "Users");
        }

        private void buttonAddUser_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new AddEmployeePage(administratorWindow));
        }

        private void buttonChangeUser_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEmployee != null)
            {
                ChangeEmployee changeWindow = new ChangeEmployee(selectedEmployee);
                changeWindow.Closed += (s, args) =>
                {
                    LoadDataGrid();
                };
                changeWindow.ShowDialog();
            }
            else
                MessageBox.Show("Выберите сотрудника!");
        }

        private void dataGridUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedEmployee = (ResponseUsers)listEmployees.SelectedItem;
        }

        private async void textBoxSearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearchUser.Text;
            Func<ResponseUsers, string> searchProperty = item => item.FullName;
            await dataGridLoader.LoadData(listEmployees, "Users", searchProperty, searchTerm);
        }

        private async void comboBoxCategoryUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxCategoryUser.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedCategory = selectedItem.Content.ToString();
                if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "Все")
                {
                    Func<ResponseUsers, string> searchProperty = item => item.Role;
                    await dataGridLoader.LoadData(listEmployees, "Users", searchProperty, selectedCategory);
                }
                else
                    LoadDataGrid();
            }
        }
    }
}
