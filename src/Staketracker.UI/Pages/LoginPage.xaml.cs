using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views; 
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Staketracker.Core.ViewModels.Login;

namespace Staketracker.UI.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxMasterDetailPagePresentation(Position = MasterDetailPosition.Detail, NoHistory = true, Title = "Login Page")]
    public partial class LoginPage : MvxContentPage<LoginViewModel>
    {
		public LoginPage()
		{
			InitializeComponent ();
		}
	}
}
