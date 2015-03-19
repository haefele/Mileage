using System.Threading.Tasks;
using System.Windows;

namespace Mileage.Client.Contracts.Layout
{
    public interface ILayoutManager
    {
        Task SaveLayoutAsync(string layoutName, DependencyObject element);

        Task RestoreLayoutAsync(string layoutName, DependencyObject element);
    }
}