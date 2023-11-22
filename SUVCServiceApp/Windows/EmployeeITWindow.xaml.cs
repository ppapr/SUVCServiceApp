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
    /// Логика взаимодействия для EmployeeITWindow.xaml
    /// </summary>
    public partial class EmployeeITWindow : Window
    {
        private readonly int authenticatedUserId;
        public EmployeeITWindow(int authenticatedUserId)
        {
            InitializeComponent();
            this.authenticatedUserId = authenticatedUserId;
            frameWorkspace.Navigate(new Pages.ITEmployeePages.RequestITEmployeePage(authenticatedUserId));
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void buttonRequests_Click(object sender, RoutedEventArgs e)
        {
            frameWorkspace.Navigate(new Pages.ITEmployeePages.RequestITEmployeePage(authenticatedUserId));
        }

        private void buttonEquipment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonSpareParts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonRegistryProgram_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
