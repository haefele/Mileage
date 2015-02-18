using Mileage.Client.Contracts.Messages;
using Mileage.Localization.Common;

namespace Mileage.Client.Windows.Messages
{
    public static class MessageServiceExtensions
    {
        public static void ShowConfirmationDialog(this IMessageService messageService, string message, string caption, MessageImage image)
        {
            messageService.ShowDialog(message, caption, image, CommonMessages.OK);
        }
    }
}