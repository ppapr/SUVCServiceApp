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

namespace SUVCServiceApp.Pages.AddPagesITEmployee
{
    /// <summary>
    /// Логика взаимодействия для AddSpareITEmployee.xaml
    /// </summary>
    public partial class AddSpareITEmployee : Page
    {
        private readonly Windows.EmployeeITWindow employeeITWindow;
        public AddSpareITEmployee(Windows.EmployeeITWindow employeeITWindow)
        {
            InitializeComponent();
            this.employeeITWindow = employeeITWindow;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            employeeITWindow.FrameWorkspace.Navigate(new Pages.ITEmployeePages.SparePartsITEmployeePage(employeeITWindow));
        }
    }
}
