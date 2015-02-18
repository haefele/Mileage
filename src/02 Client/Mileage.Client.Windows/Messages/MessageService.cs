using Caliburn.Micro;
using LiteGuard;
using Mileage.Client.Contracts.Messages;
using Mileage.Client.Windows.Windows;

namespace Mileage.Client.Windows.Messages
{
    public class MessageService : IMessageService
    {
        #region Fields
        private readonly IWindowManager _windowManager;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageService"/> class.
        /// </summary>
        /// <param name="windowManager">The window manager.</param>
        public MessageService(IWindowManager windowManager)
        {
            Guard.AgainstNullArgument("windowManager", windowManager);

            this._windowManager = windowManager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates and shows a dialog with the specified <paramref name="message" />, <paramref name="caption" />, <paramref name="image" /> and <paramref name="buttons" />.
        /// Returns the index of the clicked button (0 = first button, 1 = second button, ...).
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="image">The image.</param>
        /// <param name="buttons">The buttons.</param>
        public int ShowDialog(string message, string caption, MessageImage image, params string[] buttons)
        {
            var viewModel = new MessageDialogViewModel(message, caption, image, buttons);
            this._windowManager.ShowDialog(viewModel, null, WindowSettings.With().AutoSize().NoResize().NoWindowButtons().NoIcon());

            return viewModel.Buttons.IndexOf(viewModel.SelectedButton);
        }
        #endregion
    }
}