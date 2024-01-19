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
    /// Логика взаимодействия для MapITEmployeePage.xaml
    /// </summary>
    public partial class MapITEmployeePage : Page
    {
        private readonly ResponseUsers authenticatedUser;

        public ResponseUsers AuthenticatedUser
        {
            get { return authenticatedUser; }
        }
        public MapITEmployeePage(ResponseUsers user)
        {
            InitializeComponent();
            this.authenticatedUser = user;
            DataContext = this;
        }

    }
}
