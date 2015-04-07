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

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    /// <summary>
    /// Interaction logic for SuggestionResultsView.xaml
    /// </summary>
    public partial class SuggestionResultsView : UserControl
    {
        public SuggestionResultsViewModel ResultViewModel
        {
            get { return this.DataContext as SuggestionResultsViewModel; }
        }

        public SuggestionResultsView()
        {
            this.InitializeComponent();
        }

        private async void ContentControl_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            await this.ResultViewModel.SelectSuggestion.ExecuteAsyncTask();
        }
    }
}
