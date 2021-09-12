namespace Staketracker.Core.ViewModels.ChangePassword
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using Staketracker.Core.ViewModels.Root;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ChangePasswordViewModel : BaseViewModel
    {
        private string email;
        private AuthReply authReply;
        internal readonly IMvxNavigationService _navigationService;


        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();


        public ICommand SubmitForgetPasswordCommand { get; set; }

        public ChangePasswordViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();
            authReply = new AuthReply();
            _navigationService = navigationService;


        }

        private async void SubmitForgetUserId()
        {


        }
        public void AddValidationRules()
        {
            Email.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Enter Valid Email Address" });
        }

        private bool AreFieldsValid()
        {
            return Email.Validate();
        }

        internal async Task AuthenticateUser(LoginAPIBody loginApiBody)
        {
            if (AreFieldsValid())
            {


            }
        }



    }
}
