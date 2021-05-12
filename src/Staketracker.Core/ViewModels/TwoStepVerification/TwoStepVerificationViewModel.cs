
namespace Staketracker.Core.ViewModels.TwoStepVerification
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using Newtonsoft.Json;
    using Staketracker.Core.Models;
    using Staketracker.Core.ViewModels.Dashboard;
    using Staketracker.Core.ViewModels.Login;
    using Xamarin.Forms;

    public class TwoStepVerificationViewModel : BaseViewModel
    {

        public ICommand OpenDashboardPageCommand { get; set; }
        public ICommand ResendCodeCommand { get; set; }
        private readonly IMvxLog _logger;
        internal readonly IMvxNavigationService _navigationService;
        private int generatedCode;
        private string _email;
        private int _VerificationCode;
        private readonly Random _random = new Random();

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
            Email = GetUserEmail();
            SendTwoStepVerificationEmail(Email, generatedCode);
            String msg = "Email Sent to " + Email;
            PageDialog.Toast(msg, TimeSpan.FromSeconds(3));
        }
        string GetUserEmail()
        {
            return "andyasne@gmail.com";
        }
        void SendTwoStepVerificationEmail(string email, int generatedCode)
        {
            string emailTemplate = "Hi!, This is your 2FA login verification code: {0}. \n" + " Thank you";


            string emailBody = String.Format(emailTemplate, generatedCode.ToString());
            //define mail
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("noreply@sustainet.com");
            mail.To.Add(email);
            mail.Subject = "Please verify your device";
            mail.Body = emailBody;

            //end email attachment part

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("datastorea4@gmail.com", "storepassword");
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

                await _navigationService.Navigate<DashboardViewModel>();
            }
            else
            {
                await PageDialog.AlertAsync("Incorrect Verification Code Entered", "Verification Error", "Ok");


                //  await _navigationService.Navigate<LoginViewModel>();

            }

        }
        Task ResendCode()
        {
            return InitTwoStepVerification();

        }
    }
}
