using DevExpress.Xpf.Ribbon;

namespace Mileage.Client.Windows.Views.Shell
{
    /// <summary>
    /// A marker interface for views that have <see cref="RibbonControl"/>s to merge with the shell <see cref="RibbonControl"/>.
    /// </summary>
    internal interface IHaveRibbonToMerge
    {
        RibbonControl RibbonControl { get; }
    }
}