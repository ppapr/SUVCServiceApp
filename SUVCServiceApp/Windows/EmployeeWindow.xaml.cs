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
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        private ResponseUsers authenticatedUser;
        public Frame FrameWorkspace
        {
            get { return frameWorkspace; }
        }
        public EmployeeWindow(ResponseUsers user)
        {
            InitializeComponent();
            this.authenticatedUser = user;
            frameWorkspace.Navigate(new Pages.EmployeePages.RequestsEmployeePage(authenticatedUser, this));
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void buttonEquipment_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.EmployeePages.EquipmentEmployeePage(authenticatedUser, this));
        }

        private void buttonRequests_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.EmployeePages.RequestsEmployeePage(authenticatedUser, this));
        }

        private void buttonProfile_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.EmployeePages.ProfileEmployee(authenticatedUser));
        }
    }
}
