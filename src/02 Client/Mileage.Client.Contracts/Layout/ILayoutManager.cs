using System.Threading.Tasks;
using System.Windows;
using Mileage.Shared.Entities.Users;

namespace Mileage.Client.Contracts.Layout
{
    public interface ILayoutManager
    {
        Task SaveLayoutForCurrentUserAsync(string layoutName, DependencyObject control);

        Task RestoreLayoutForCurrentUserAsync(string layoutName, DependencyObject control);
    }
}