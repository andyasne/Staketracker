
namespace Staketracker.Core.ViewModels.TwoStepVerification
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.ViewModels.Dashboard;
    using Staketracker.Core.ViewModels.Login;
    using Staketracker.Core.ViewModels.Root;
    using Xamarin.Forms;

    public class TwoStepVerificationViewModel : BaseViewModel<AuthReply>
    {

        public ICommand OpenDashboardPageCommand { get; set; }
        public ICommand ResendCodeCommand { get; set; }
        private readonly IMvxLog _logger;
        internal readonly IMvxNavigationService _navigationService;
        private int generatedCode;
        private string _email;
        private int _VerificationCode;
        private readonly Random _random = new Random();
        //      private LoginAPIBody loginAPIBody;

        public int VerificationCode
        {
            get => _VerificationCode;
            set => SetField(ref _VerificationCode, value);
        }

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }
        public TwoStepVerificationViewModel(IMvxNavigationService navigationService, IMvxLog logger)
        {
            _navigationService = navigationService;
            OpenDashboardPageCommand = new Command(async () => await RunSafe(OpenDashboardPage()));
            ResendCodeCommand = new Command(async () => await RunSafe(ResendCode()));
            _logger = logger;
        }


        public override async Task Initialize()
        {
            await base.Initialize();

            try
            {
                await InitTwoStepVerification();

            }
            catch (Exception e)
            {
                _logger.Debug(e.Message);
            }
        }

        private async Task InitTwoStepVerification()
        {
            generatedCode = RandomNumber(1000, 9999);
            Email = await GetUserEmail();
            SendTwoStepVerificationEmail(Email, generatedCode);
            String msg = "Email Sent to " + Email;
            PageDialog.Toast(msg, TimeSpan.FromSeconds(3));
        }

        internal async Task<String> GetUserEmail()
        {

            UsrEmailResponse usrEmailResponse;
            HttpResponseMessage userEmail = await ApiManager.GetUsrEmail(authReply.loginAPIBody);

            if (userEmail.IsSuccessStatusCode)
            {
                var response = await userEmail.Content.ReadAsStringAsync();
                usrEmailResponse = await Task.Run(() => JsonConvert.DeserializeObject<UsrEmailResponse>(response));
                return usrEmailResponse.d;

            }
            else
            {
                await PageDialog.AlertAsync("API Error While retrieving Email address for the user", "API Response Error", "Ok");
                return null;
            }

        }
        void SendTwoStepVerificationEmail(string email, int generatedCode)
        {
            string emailTemplate = "Here's your Authentication code: {0} \n Please note that, for security purposes, this temporary confirmation code will expire in 10 minutes.\nIf you did not try to login, please ignore this email, or reply to let us know.\n Best Regards,\n StakeTracker Customer Support \n Email:support @staketracker.com \n Phone: 604 - 670 - 0240";

            string emailBody = String.Format(emailTemplate, generatedCode.ToString());
            //define mail
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("noreply@sustainet.com");
            mail.To.Add(email);
            mail.Subject = "StakeTracker two Factor Auth Access Code";
            mail.Body = emailBody;

            //end email attachment part

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("staketracker2021@gmail.com", "Stake@123!");
            SmtpServer.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception)
            {

            }
        }

        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        internal async Task OpenDashboardPage()
        {
            if (VerificationCode == generatedCode)
            {
                await _navigationService.Navigate<RootViewModel, AuthReply>(authReply);

                // await _navigationService.Navigate<DashboardViewModel>();
            }
            else
            {
                await PageDialog.AlertAsync("Incorrect Verification Code Entered", "Verification Error", "Ok");


                //  await _navigationService.Navigate<LoginViewModel>();

            }

        }
        Task ResendCode()
        {
            VerificationCode = 0;
            return InitTwoStepVerification();

        }
        AuthReply authReply;


        public override void Prepare(AuthReply authReply)
        {
            this.authReply = authReply;
        }
    }
}
