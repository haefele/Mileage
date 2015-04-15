using System.Windows.Controls;
using Caliburn.Micro;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using DevExpress.Xpf.Ribbon;

namespace Mileage.Client.Windows.Views.Shell.Items.Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl, IHaveRibbonToMerge
    {
        #region Properties
        /// <summary>
        /// Gets the view model.
        /// </summary>
        public DashboardViewModel ViewModel
        {
            get { return this.DataContext as DashboardViewModel; }
        }
        /// <summary>
        /// Gets the ribbon control.
        /// </summary>
        public RibbonControl RibbonControl
        {
            get { return this.ActualRibbonControl; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardView"/> class.
        /// </summary>
        public DashboardView()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Executed when the user closed an item in the <see cref="DockLayoutManager"/>.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DockItemClosedEventArgs"/> instance containing the event data.</param>
        private void DockLayoutManagerOnDockItemClosed(object sender, DockItemClosedEventArgs e)
        {
            foreach (var affectedItem in e.AffectedItems)
            {
                var itemViewModel = affectedItem.DataContext as DashboardItemViewModel;

                if (this.ViewModel.Items.Contains(itemViewModel))
                {
                    this.ViewModel.CloseItem(itemViewModel);
                }
            }
        }
        #endregion
    }
}
