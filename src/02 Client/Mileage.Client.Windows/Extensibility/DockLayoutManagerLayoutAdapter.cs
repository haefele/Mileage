using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Docking;

namespace Mileage.Client.Windows.Extensibility
{
    public class DockLayoutManagerLayoutAdapter : Behavior<DockLayoutManager>, ILayoutAdapter
    {
        protected override void OnAttached()
        {
            MVVMHelper.SetLayoutAdapter(this.AssociatedObject, this);
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
    }
}