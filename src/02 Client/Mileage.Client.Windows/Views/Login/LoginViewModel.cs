using System;
using System.Net;
using System.Net.Http;
using System.Reactive;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Castle.Windsor;
using Mileage.Shared.Entities.Authentication;
using Mileage.Shared.Models;
using ReactiveUI;

namespace Mileage.Client.Windows.Views.Login
{
    public class LoginViewModel : MileageScreen
    {
        #region Fields
        private string _emailAddress;
        private string _password;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string EmailAddress
        {
            get { return this._emailAddress; }
            set { this.RaiseAndSetIfChanged(ref this._emailAddress, value); }
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
        /// <summary>
        /// Tries to login the user with the currently entered credentials.
        /// </summary>
        public ReactiveCommand<Unit> Login { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        public LoginViewModel(IWindsorContainer container)
            : base(container)
        {
            this.CreateCommands();

            this.EmailAddress = "haefele@xemio.net";
            this.Password = "123456";
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates all commands of this ViewModel.
        /// </summary>
        private void CreateCommands()
        {
            var canLogin = this.WhenAnyValue(f => f.EmailAddress, f => f.Password,
                (emailAddress, password) => string.IsNullOrWhiteSpace(emailAddress) == false && string.IsNullOrWhiteSpace(password) == false);

            this.Login = ReactiveCommand.CreateAsyncTask(canLogin, _ => this.LoginImpl());
            this.Login.ThrownExceptions.Subscribe(this.ExceptionHandler.Handle);
        }
        private async Task LoginImpl()
        {
            var data = new LoginData
            {
                EmailAddress = this.EmailAddress,
                PasswordMD5Hash = this.GetPasswordMD5Hash()
            };

            HttpResponseMessage response = await this.WebService.Authentication.LoginAsync(data);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var token = await response.Content.ReadAsAsync<AuthenticationToken>();
                this.Session.Token = token;

                this.TryClose(true);
            }
            else
            {
                var error = await response.Content.ReadAsAsync<HttpError>();
                this.ExceptionHandler.Handle(error);
            }
        }
        private byte[] GetPasswordMD5Hash()
        {
            byte[] passwordInUTF8 = Encoding.UTF8.GetBytes(this.Password);
            return MD5.Create().ComputeHash(passwordInUTF8);
        }
        #endregion
    }
}