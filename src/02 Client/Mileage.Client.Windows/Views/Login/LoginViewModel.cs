using System;
using System.Reactive;
using System.Threading.Tasks;
using Caliburn.Micro.ReactiveUI;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Login
{
    public class LoginViewModel : ReactiveScreen
    {
        #region Fields
        private string _username;
        private string _password;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get { return this._username; }
            set { this.RaiseAndSetIfChanged(ref this._username, value); }
        }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get { return this._password; }
            set { this.RaiseAndSetIfChanged(ref this._password, value); }
        }
        #endregion

        #region Commands
        public ReactiveCommand<object> Login { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        public LoginViewModel()
        {
            this.DisplayName = "Mileage";

            this.CreateCommands();
        }

        private void CreateCommands()
        {
            var canLogin = this.WhenAnyValue(f => f.Username, f => f.Password,
                (username, password) => string.IsNullOrWhiteSpace(username) == false && string.IsNullOrWhiteSpace(password) == false);

            this.Login = ReactiveCommand.CreateAsyncTask(canLogin, _ => this.LoginImpl());
        }


        private async Task<object> LoginImpl()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));

            return Unit.Default;
        }

        #endregion
    }
}