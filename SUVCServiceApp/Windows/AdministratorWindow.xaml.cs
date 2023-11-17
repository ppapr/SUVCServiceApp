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
    /// Логика взаимодействия для AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public AdministratorWindow()
        {
            InitializeComponent();
            frameWorkspace.Navigate(new Pages.RequestsPage());
        }

        private void buttonRequests_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.RequestsPage());
        }

        private void buttonEmployees_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.EmployeePage());
        }

        private void buttonEquipment_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.EquipmentPage());
        }

        private void buttonInventory_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.InventoryPage());
        }

        private void buttonReports_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.ReportsPage());
        }

        private void buttonSpareParts_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.SparePartsPages());
        }

        private void buttonRegistryProgram_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.RegsitryProgramPage());
        }
    }
}
