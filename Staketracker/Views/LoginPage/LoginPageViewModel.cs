using Telerik.XamarinForms.Common;
using Xamarin.Forms;

namespace Staketracker.Views.LoginPage
{
    public class LoginPageViewModel : NotifyPropertyChangedBase
    {
        private string username;
        private string email;
        private string password;
        
        public LoginPageViewModel()
        {
            this.GoToViewCommand = new Command(this.LoadView);
            this.LoginCommand = new Command(this.ExecuteLogin);
            this.ResetPasswordCommand = new Command(this.SubmitPasswordReset);
        }

        public string Username
        {
            get => this.username;
            set
            {
                if (this.username == value)
                {
                    return;
                }

                this.username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => this.email;
            set
            {
                if (this.email == value)
                {
                    return;
                }

                this.email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => this.password;
            set
            {
                if (this.password == value)
                {
                    return;
                }

                this.password = value;
                OnPropertyChanged();
            }
        }
        
        public Command GoToViewCommand { get; set; }

        public Command LoginCommand { get; set; }

        public Command ResetPasswordCommand { get; set; }

        public INavigationHandler NavigationHandler { private get; set; }

        private void LoadView(object viewType)
        {
            this.NavigationHandler.LoadView((ViewType)viewType);
        }

        private void ExecuteLogin(object obj)
        {
            switch ((LoginType)obj)
            {
                case LoginType.Normal:
                    // Use Username and Password properties
                    Application.Current.MainPage.DisplayAlert("Normal Login", $"You have logged in with:\r\nUsername: {Username}\nPassword: {Password}", "ok");
                    break;
                case LoginType.SignUp:
                    // Use Username, Email and Password properties
                    Application.Current.MainPage.DisplayAlert("Signup Login", $"You have signed up with:\r\nUsername: {Username}\nEmail:{Email}\nPassword: {Password}", "ok");
                    break;
                case LoginType.PasswordReset:
                    // Use Email property for reset request
                    Application.Current.MainPage.DisplayAlert("Password Reset Requested", $"Password reset for: Email:{Email}", "ok");
                    break;
            }
        }

        private void SubmitPasswordReset()
        {
            // Use the Email property
            Application.Current.MainPage.DisplayAlert("Password Reset", $"You have requested a password reset for: {Email}", "ok");
        }
    }
}