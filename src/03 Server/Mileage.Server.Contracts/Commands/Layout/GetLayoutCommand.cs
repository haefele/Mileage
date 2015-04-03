using System.Collections.Generic;
using JetBrains.Annotations;
using LiteGuard;

namespace Mileage.Server.Contracts.Commands.Layout
{
    public class GetLayoutCommand : ICommand<Dictionary<string, byte[]>>
    {
        public GetLayoutCommand([NotNull]string userId, [NotNull]string layoutName)
        {
            Guard.AgainstNullArgument("userId", userId);
            Guard.AgainstNullArgument("layoutName", layoutName);

            this.UserId = userId;
            this.LayoutName = layoutName;
        }

        public string UserId { get; private set; }
        public string LayoutName { get; private set; }
    }
}