using System.Windows.Controls;
using DevExpress.Xpf.Ribbon;

namespace Mileage.Client.Windows.Views.Shell.Items.Drivers
{
    /// <summary>
    /// Interaction logic for DriversRootView.xaml
    /// </summary>
    public partial class DriversRootView : UserControl, IHaveRibbonToMerge
    {
        public DriversRootView()
        {
            InitializeComponent();
        }

        public RibbonControl RibbonControl
        {
            get { return this.ActualRibbonControl; }
        }
    }
}
