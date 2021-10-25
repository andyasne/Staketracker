namespace Staketracker.Core.ViewModels.ChangePassword
{
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Plugin.Settings;
    using Staketracker.Core.Models;
    using Staketracker.Core.Models.AddEventsReply;
    using Staketracker.Core.Models.ApiRequestBody;
    using Staketracker.Core.Models.ChangePasswordReply;
    using Staketracker.Core.Models.EventsFormValue;
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
        public ChangePasswordBody ChangePasswordBodyModel { get; set; }

        public ChangePasswordViewModel(IMvxNavigationService navigationService)
        {
            authReply = new AuthReply();
            _navigationService = navigationService;
            ChangePasswordBodyModel = new ChangePasswordBody();
            SubmitForgetPasswordCommand = new MvxAsyncCommand(OnSubmitForgetPasswordCommand);

        }

        private async Task OnSubmitForgetPasswordCommand()
        {
            int userId = CrossSettings.Current.GetValueOrDefault("userId", 0);

            ChangePasswordBodyModel.UserId = userId.ToString();

            ChangePassword(ChangePasswordBodyModel);
        }

        internal async Task ChangePassword(ChangePasswordBody changePasswordBody)
        {
            ChangePasswordReply responseReply;
            jsonTextObj jto = new jsonTextObj(changePasswordBody);
            string sessionId = CrossSettings.Current.GetValueOrDefault("sessionId", "");
            HttpResponseMessage changePasswordRespMessage = await ApiManager.ChangePassword(jto, sessionId);

            if (changePasswordRespMessage.IsSuccessStatusCode)
            {
                var response = await changePasswordRespMessage.Content.ReadAsStringAsync();
                responseReply = await Task.Run(() => JsonConvert.DeserializeObject<ChangePasswordReply>(response));

                if (responseReply.d.status.ToString() == "OK")
                {
                    //TODO: navigate back
                    await PageDialog.AlertAsync("Password Changed Successfully", "Password Changed", "Ok");
                }
                else
                {
                    await PageDialog.AlertAsync(responseReply.d.message, "Error Changing Password", "Ok");
                }
            }
            else
                await PageDialog.AlertAsync("API Error While Trying to change password", "API Response Error", "Ok");

        }
    }
}
