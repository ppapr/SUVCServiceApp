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
        private readonly int authenticatedUserId;
        public EmployeeWindow(int authenticatedUserId)
        {
            InitializeComponent();
            this.authenticatedUserId = authenticatedUserId;
            frameWorkspace.Navigate(new Pages.EmployeePages.RequestsEmployeePage(authenticatedUserId));
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void buttonEquipment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonRequests_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.EmployeePages.RequestsEmployeePage(authenticatedUserId));
        }
    }
}
