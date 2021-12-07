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
    using Plugin.Settings;

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

            authReply = new AuthReply();
            _navigationService = navigationService;
            Language defaultLang = new Language { Name = "English", Abr = "en" };

            this.Languages = new ObservableCollection<Language>
        {
                defaultLang,
            new Language { Name = "Spanish", Abr = "es"},
            new Language { Name = "Amharic", Abr = "am"}

            };
            string savedDefaultLang = CrossSettings.Current.GetValueOrDefault("DefaultLanguage", "");

            foreach (Language lang in Languages)
            {
                if (lang.Abr == savedDefaultLang)
                {
                    this.SelectedLanguage = lang;
                }
            }

            ChangeLanguage = new Command(() =>
            {
                if (SelectedLanguage != null)
                {
                    CultureInfo language = new CultureInfo(SelectedLanguage.Abr);
                    Thread.CurrentThread.CurrentCulture = language;
                    Thread.CurrentThread.CurrentUICulture = language;
                    System.Globalization.CultureInfo ci;
                    ci = language;
                    try
                    {
                        ci = new System.Globalization.CultureInfo(SelectedLanguage.Abr);
                    }
                    catch (CultureNotFoundException ex)
                    {
                        ci = new System.Globalization.CultureInfo("en");//default to en
                    }

                    Application.Current.Properties["Lang"] = language.TwoLetterISOLanguageName;// ci.Name.Substring(0, ci.Name.IndexOf("-"));

                    AppRes.Culture = language;
                    CrossSettings.Current.AddOrUpdateValue("DefaultLanguage", SelectedLanguage.Abr);
                    //navigationService.ChangePresentation(
                    //    new MvvmCross.Presenters.Hints.MvxPopPresentationHint(typeof(LoginViewModel)));

                }
                else
                {
                    //invalid
                }

            });

        }









    }
}
