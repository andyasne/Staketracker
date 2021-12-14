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
    using Staketracker.Core.Res;
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
        public ICommand ChangePasswordCommand { get; set; }
        public ChangePasswordBody ChangePasswordBodyModel { get; set; }

        public ChangePasswordViewModel(IMvxNavigationService navigationService)
        {
            authReply = new AuthReply();
            _navigationService = navigationService;
            ChangePasswordBodyModel = new ChangePasswordBody();
            ChangePasswordCommand = new MvxAsyncCommand(OnPasswordChange);

        }

        private async Task OnPasswordChange()
        {
            int userId = CrossSettings.Current.GetValueOrDefault("userId", 0);

            ChangePasswordBodyModel.UserId = userId.ToString();

            if (string.IsNullOrEmpty(ChangePasswordBodyModel.ConfirmNewPassword) || string.IsNullOrEmpty(ChangePasswordBodyModel.CurrentPassword) || string.IsNullOrEmpty(ChangePasswordBodyModel.NewPassword))
            {
                await PageDialog.AlertAsync(AppRes.msg_required_fields, AppRes.validation_error, AppRes.ok);
            }
            else if (ChangePasswordBodyModel.ConfirmNewPassword != ChangePasswordBodyModel.NewPassword)
            {
                await PageDialog.AlertAsync(AppRes.new_password_does_not_match, AppRes.validation_error, AppRes.ok);
            }
            else
            {
                ChangePassword(ChangePasswordBodyModel);
            }
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
                    await PageDialog.AlertAsync(AppRes.password_changed_successfully, AppRes.password_changed, AppRes.ok);
                }
                else
                {
                    await PageDialog.AlertAsync(responseReply.d.message, AppRes.error_changing_pw, AppRes.ok);
                }
            }
            else
                await PageDialog.AlertAsync(AppRes.api_error_trying_to_change_pw, AppRes.api_response_error, AppRes.ok);

        }
    }
}
