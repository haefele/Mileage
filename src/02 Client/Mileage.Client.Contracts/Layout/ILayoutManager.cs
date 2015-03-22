using System.Threading.Tasks;
using System.Windows;
using Mileage.Shared.Entities.Users;

namespace Mileage.Client.Contracts.Layout
{
    public interface ILayoutManager
    {
        Task SaveLayoutAsync(User user, string layoutName, DependencyObject control);

        Task RestoreLayoutAsync(User user, string layoutName, DependencyObject control);
    }
}