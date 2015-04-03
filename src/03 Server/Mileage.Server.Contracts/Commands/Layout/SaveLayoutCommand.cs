using System.Collections.Generic;
using JetBrains.Annotations;
using LiteGuard;

namespace Mileage.Server.Contracts.Commands.Layout
{
    public class SaveLayoutCommand : ICommand<object>
    {
        public SaveLayoutCommand([NotNull]string layoutName, [NotNull]string userId, [NotNull]Dictionary<string, byte[]> layoutData)
        {
            Guard.AgainstNullArgument("layoutName", layoutName);
            Guard.AgainstNullArgument("userId", userId);
            Guard.AgainstNullArgument("layoutData", layoutData);

            this.LayoutName = layoutName;
            this.UserId = userId;
            this.LayoutData = layoutData;
        }

        public string LayoutName { get; private set; }
        public string UserId { get; private set; }
        public Dictionary<string, byte[]> LayoutData { get; private set; }
    }
}