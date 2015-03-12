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
using DevExpress.Xpf.Ribbon;

namespace Mileage.Client.Windows.Views.Shell.Items
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
