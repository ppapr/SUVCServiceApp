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
    /// Логика взаимодействия для AdministratorWindow.xaml
    /// </summary>
    public partial class AdministratorWindow : Window
    {
        public Frame FrameWorkspace
        {
            get { return frameWorkspace; }
        }
        ResponseUsers currentUser;
        public AdministratorWindow(ResponseUsers authenticatedUser)
        {
            InitializeComponent();
            this.currentUser = authenticatedUser;
            frameWorkspace.Navigate(new Pages.RequestsPage());
        }

        private void buttonRequests_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.RequestsPage());
        }

        private void buttonEmployees_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.EmployeePage(this));
        }

        private void buttonEquipment_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.EquipmentPage(this));
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
            frameWorkspace.Navigate(new Pages.SparePartsPages(this));
        }

        private void buttonRegistryProgram_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.RegsitryProgramPage(this));
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.login = "";
            Properties.Settings.Default.password = "";
            Properties.Settings.Default.Save();
            new MainWindow().Show();
            Close();
        }

        private void buttonProfile_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.AdministratorPages.ProfileAdministrator(currentUser));
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void buttonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void buttonCloseApp_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void buttonCloseApp_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
