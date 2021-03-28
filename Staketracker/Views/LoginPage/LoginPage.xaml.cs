using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Staketracker.Views.LoginPage.Views;

namespace Staketracker.Views.LoginPage
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage, INavigationHandler
	{
	    public LoginPage()
		{
		    InitializeComponent();

		    this.BindingContext = new LoginPageViewModel { NavigationHandler = this };

            this.Content = new LoginView();
        }

	    public void LoadView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.LoginView:
                    this.Content = new LoginView();
                    break;
                case ViewType.SignUpView:
                    this.Content = new SignUpView();
                    break;
                case ViewType.PasswordResetView:
                    this.Content = new PasswordResetView();
                    break;
            }
        }
        
        protected override bool OnBackButtonPressed()
        {
            var viewType = this.Content.GetType();

            if (viewType == typeof(SignUpView) || viewType == typeof(PasswordResetView))
            {
                this.Content = new LoginView();
                return true;
            }

            return base.OnBackButtonPressed();
        }
    }
}