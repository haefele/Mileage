using System;
using System.Windows;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;

namespace Mileage.Client.Windows.Extensibility
{
    public class DockLayoutManagerLayoutAdapter : Behavior<DockLayoutManager>, ILayoutAdapter
    {
        protected override void OnAttached()
        {
            MVVMHelper.SetLayoutAdapter(this.AssociatedObject, this);

            this.AssociatedObject.Loaded += this.AssociatedObjectOnLoaded;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= this.AssociatedObjectOnLoaded;
        }

        public string Resolve(DockLayoutManager owner, object item)
        {
            var layoutItem = owner.GetItem("PanelHost") as LayoutGroup;
            
            if (layoutItem == null)
            {
                layoutItem = new LayoutGroup {Name = "PanelHost", DestroyOnClosingChildren = false};
                owner.LayoutRoot = layoutItem;
            }

            return "PanelHost";
        }


        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            //Name the children based on their index

        }
    }
}