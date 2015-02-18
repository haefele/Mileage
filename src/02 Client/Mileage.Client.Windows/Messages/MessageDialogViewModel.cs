using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.ReactiveUI;
using LiteGuard;
using Mileage.Client.Contracts.Messages;
using ReactiveUI;

namespace Mileage.Client.Windows.Messages
{
    public class MessageDialogViewModel : ReactiveScreen
    {
        #region Fields
        private string _message;
        private MessageImage _image;
        private IObservableCollection<string> _buttons;
        private string _selectedButton;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get { return this._message; }
            set { this.RaiseAndSetIfChanged(ref this._message, value); }
        }
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public MessageImage Image
        {
            get { return this._image; }
            set { this.RaiseAndSetIfChanged(ref this._image, value); }
        }
        /// <summary>
        /// Gets or sets the buttons.
        /// </summary>
        public IObservableCollection<string> Buttons
        {
            get { return this._buttons; }
            set { this.RaiseAndSetIfChanged(ref this._buttons, value); }
        }
        /// <summary>
        /// Gets or sets the selected button.
        /// </summary>
        public string SelectedButton
        {
            get { return this._selectedButton; }
            set { this.RaiseAndSetIfChanged(ref this._selectedButton, value); }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDialogViewModel"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <param name="image">The image.</param>
        /// <param name="buttons">The buttons.</param>
        public MessageDialogViewModel(string message, string title, MessageImage image, params string[] buttons)
        {
            Guard.AgainstNullArgument("message", message);
            Guard.AgainstNullArgument("title", title);
            Guard.AgainstNullArgument("buttons", buttons);

            this.Message = message;
            this.DisplayName = title;
            this.Image = image;
            this.Buttons = new BindableCollection<string>(buttons);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Selects the specified <paramref name="button"/>.
        /// </summary>
        /// <param name="button">The button.</param>
        public void SelectButton(string button)
        {
            this.SelectedButton = button;
            this.TryClose(true);
        }
        #endregion
    }
}
