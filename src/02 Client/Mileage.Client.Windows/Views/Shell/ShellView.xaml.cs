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
using Caliburn.Micro;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Grid.TreeList;
using DevExpress.Xpf.Ribbon;
using Mileage.Client.Windows.Windows;

namespace Mileage.Client.Windows.Views.Shell
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : UserControl, IHaveRibbonToMerge
    {
        #region Fields
        private IHaveRibbonToMerge _latestMergedRibbonView;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the view model.
        /// </summary>
        public ShellViewModel ViewModel
        {
            get { return this.DataContext as ShellViewModel; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellView"/> class.
        /// </summary>
        public ShellView()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Occures when the <see cref="ShellView.DataContext"/> changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ShellViewOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.ViewModel.ActivationProcessed += ViewModelOnActivationProcessed;
            this.HandleRibbonMergeAndUnMerge();
        }
        /// <summary>
        /// Occures when the <see cref="ViewModel"/> activates a content view.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ActivationProcessedEventArgs"/> instance containing the event data.</param>
        private void ViewModelOnActivationProcessed(object sender, ActivationProcessedEventArgs e)
        {
            this.HandleRibbonMergeAndUnMerge();
        }
        /// <summary>
        /// Handles the ribbon merge and un merge.
        /// </summary>
        private void HandleRibbonMergeAndUnMerge()
        {
            //UnMerge the RibbonControls
            if (this._latestMergedRibbonView != null)
            {
                this.RibbonControl.UnMerge(this._latestMergedRibbonView.RibbonControl);
            }

            var ribbon = this.ActiveItem.Content as IHaveRibbonToMerge;
            this._latestMergedRibbonView = ribbon;

            if (ribbon != null)
            {
                //Merge the RibbonControls
                this.RibbonControl.Merge(ribbon.RibbonControl);
            }
        }
        #endregion

        #region Implementation of IHaveRibbonToMerge
        /// <summary>
        /// Gets the ribbon control.
        /// </summary>
        public RibbonControl RibbonControl
        {
            get { return this.ActualRibbonControl; }
        }
        #endregion
    }
}
