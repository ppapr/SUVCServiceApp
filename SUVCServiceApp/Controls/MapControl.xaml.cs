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

namespace SUVCServiceApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для MapControl.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        public static readonly DependencyProperty UserProperty =
     DependencyProperty.Register("AuthenticatedUser", typeof(ResponseUsers), typeof(MapControl));

        public ResponseUsers AuthenticatedUser
        {
            get { return (ResponseUsers)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public MapControl()
        {
            InitializeComponent();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Rectangle rect)
            {
                new Windows.MapRequestsWindow(AuthenticatedUser).ShowDialog();
            }
        }
    }
}
