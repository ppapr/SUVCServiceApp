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

namespace SUVCServiceApp.Pages.AddPagesEmployee
{
    /// <summary>
    /// Логика взаимодействия для CreateNewRequestEmployeePage.xaml
    /// </summary>
    public partial class CreateNewRequestEmployeePage : Page
    {
        private readonly EmployeeWindow employeeWindow;
        private int authenticatedUserId;
        public CreateNewRequestEmployeePage(int authenticatedUserId, EmployeeWindow employeeWindow)
        {
            InitializeComponent();
            this.authenticatedUserId = authenticatedUserId;
            this.employeeWindow = employeeWindow;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            employeeWindow.FrameWorkspace.Navigate(new Pages.EmployeePages.RequestsEmployeePage(authenticatedUserId, employeeWindow));
        }
    }
}
