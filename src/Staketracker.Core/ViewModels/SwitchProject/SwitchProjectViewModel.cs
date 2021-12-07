namespace Staketracker.Core.ViewModels.SwitchProject
{
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Plugin.Settings;
    using Staketracker.Core.Models;
    using Staketracker.Core.Validators;
    using Staketracker.Core.Validators.Rules;
    using Staketracker.Core.ViewModels.Login;
    using Staketracker.Core.ViewModels.Root;
    using Staketracker.Core.ViewModels.TwoStepVerification;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public class ComboListModel
    {
        public string Name { get; set; }
    }
    public class SwitchProjectViewModel : BaseViewModel
    {
        private string email;
        private AuthReply authReply;
        internal readonly IMvxNavigationService _navigationService;


        public ValidatableObject<string> Email { get; set; } = new ValidatableObject<string>();

        public ObservableCollection<ComboListModel> BusinessUnit { get; set; }
        public ObservableCollection<ComboListModel> Project { get; set; }
        public ComboListModel SelectedProject { get; set; }
        public String DomainSelected { get; set; }


        public ICommand OpenProjectCommand { get; set; }

        public SwitchProjectViewModel(IMvxNavigationService navigationService)
        {
            AddValidationRules();
            authReply = new AuthReply();
            _navigationService = navigationService;
            this.BusinessUnit = new ObservableCollection<ComboListModel>()
            {
                new ComboListModel{Name = "Sustainet Commercial"},
                new ComboListModel{Name = "Sustainet Resources"},
                new ComboListModel{Name = "Sustainet Retail"}
            };
            this.Project = new ObservableCollection<ComboListModel>()
            {
                new ComboListModel{Name = "Sanlam/Santam"},
                new ComboListModel{Name = "StakeTracker Express"}
            };

            OpenProjectCommand = new Command(OpenProject);


        }

        private async void OpenProject()
        {
            DomainSelected = SelectedProject.Name;
            CrossSettings.Current.AddOrUpdateValue("ProjectName", DomainSelected);
            SetField(ref domainSelected, DomainSelected);
            _navigationService.Navigate(new LoginViewModel(navigationService));

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
