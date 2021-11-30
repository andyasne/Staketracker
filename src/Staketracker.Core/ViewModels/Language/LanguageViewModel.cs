namespace Staketracker.Core.ViewModels.Language
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using Staketracker.Core.ViewModels.Root;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using Staketracker.Core.Res;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Staketracker.Core.ViewModels.Login;

    public class Language
    {
        public string Name { get; set; }
        public string Abr { get; set; }
    }
    public class LanguageViewModel : BaseViewModel
    {
        private string email;
        private AuthReply authReply;
        internal readonly IMvxNavigationService _navigationService;


        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();

        public ICommand ChangeLanguage { get; set; }

        public ICommand SubmitForgetPasswordCommand { get; set; }
        public ObservableCollection<Language> Languages { get; set; }
        public Language SelectedLanguage { get; set; }


        public LanguageViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();
            authReply = new AuthReply();
            _navigationService = navigationService;
            Language defaultLang = new Language { Name = "English", Abr = "en" };

            this.Languages = new ObservableCollection<Language>
        {
                defaultLang,
            new Language { Name = "Spanish", Abr = "es"},
            new Language { Name = "Amharic", Abr = "am"}

            };

            //  SelectedLanguage = defaultLang;

            ChangeLanguage = new Command(() =>
            {
                if (SelectedLanguage != null)
                {
                    CultureInfo language = new CultureInfo(SelectedLanguage.Abr);
                    Thread.CurrentThread.CurrentUICulture = language;
                    AppRes.Culture = language;

                    navigationService.ChangePresentation(
                        new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(LoginViewModel)));

                }
                else
                {
                    //invalid
                }

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



    }
}
