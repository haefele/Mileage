using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using Mileage.Client.Contracts.Layout;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var layoutManager = IoC.Get<ILayoutManager>();
            layoutManager.SaveLayout(this);
        }
    }
}
