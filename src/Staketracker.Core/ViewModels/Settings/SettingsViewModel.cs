namespace Staketracker.Core.ViewModels.Settings
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using Staketracker.Core.ViewModels.ChangePassword;
    using Staketracker.Core.ViewModels.Language;
    using Staketracker.Core.ViewModels.Root;
    using Staketracker.Core.ViewModels.SwitchProject;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class SettingsViewModel : BaseViewModel<AuthReply>
    {
        private string email;
        private AuthReply authReply;
        internal readonly IMvxNavigationService _navigationService;

        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();


        public ICommand SubmitForgetPasswordCommand { get; set; }
        public ICommand OnDevelopmentCommand { get; set; }
        public ICommand ChangeLanguageCommand { get; set; }
        public ICommand SwitchProjectCommand { get; set; }
        public ICommand ChangePasswordCommand { get; set; }

        public SettingsViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();
            authReply = new AuthReply();
            _navigationService = navigationService;
            SubmitForgetPasswordCommand = new Command(SubmitForgetUserId);
            ChangePasswordCommand = new Command(() =>
            {
                _navigationService.Navigate<ChangePasswordViewModel>();

            });
            ChangeLanguageCommand = new Command(() =>
            {
                _navigationService.Navigate<LanguageViewModel>();

            });

            SwitchProjectCommand = new Command(() =>
            {
                _navigationService.Navigate<SwitchProjectViewModel, AuthReply>(authReply);


            });

            OnDevelopmentCommand = new Command(() =>
            {
                OnDevelopment().Start();
            });


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
        public override void Prepare(AuthReply parameter)
        {
            authReply = parameter;

        }


    }
}
