using MvvmCross.Commands;
using MvvmCross.Navigation;
using Staketracker.Core.Models;
using Staketracker.Core.ViewModels.Login;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.Dashboard
{
    public class DashboardViewModel : BaseViewModel

    {
        private readonly IMvxNavigationService _navigationService;

        public ICommand SignOutCommand { get; set; }

        public DashboardViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            SignOutCommand = new Command(async () => await SignOut());
        }

        private async Task SignOut()
        {
            await _navigationService.Navigate<LoginViewModel>();
        }
        private IReadOnlyCollection<NameValuePair> salesChannels, businessOverview,
            newCustomers, completedCommitment, inProgressCommitment, onHoldCommitment;
        //private Services.IErpService service;
        //private IReadOnlyCollection<Vendor> bestVendors;
        //private IReadOnlyCollection<Product> recentProducts;
        private IReadOnlyCollection<Order> latestOrders;
        private IMvxNavigationService navigationService;
        private bool busy;

        public async override void Prepare()
        {
            base.Prepare();

            IsBusy = true;
            await this.FetchData();
            //if (await this.service.SyncIfNeededAsync())
            //{
            //    await FetchData();
            //}
            IsBusy = false;

        }

        public override async void ViewAppearing()
        {
            base.ViewAppearing();

            //    MvxNotifyTask.Create(async () => await this.InitializeViewModels());

        }
        private async Task InitializeViewModels()
        {

            //    await _navigationService.Navigate<MenuViewModel>();
        }
        public IReadOnlyCollection<NameValuePair> SalesChannels { get => salesChannels; private set => SetProperty(ref salesChannels, value); }
        public IReadOnlyCollection<NameValuePair> BusinessOverview { get => businessOverview; private set => SetProperty(ref businessOverview, value); }
        public IReadOnlyCollection<NameValuePair> NewCustomers { get => newCustomers; private set => SetProperty(ref newCustomers, value); }
        public IReadOnlyCollection<NameValuePair> CompletedCommitment { get => completedCommitment; private set => SetProperty(ref completedCommitment, value); }
        public IReadOnlyCollection<NameValuePair> InProgressCommitment { get => inProgressCommitment; private set => SetProperty(ref inProgressCommitment, value); }
        public IReadOnlyCollection<NameValuePair> OnHoldCommitment { get => onHoldCommitment; private set => SetProperty(ref onHoldCommitment, value); }
        //    public IReadOnlyCollection<Vendor> BestVendors { get => bestVendors; private set => SetProperty(ref bestVendors, value); }
        //  public IReadOnlyCollection<Product> RecentProducts { get => recentProducts; private set => SetProperty(ref recentProducts, value); }
        public IReadOnlyCollection<Order> LatestOrders { get => latestOrders; private set => SetProperty(ref latestOrders, value); }
        public bool IsBusy { get => busy; private set => SetProperty(ref busy, value); }
        public MvxCommand AboutCommand { get; }

        private void ShowAboutPage()
        {
            //    this.navigationService.Navigate<AboutPageViewModel>();
        }

        private async Task FetchData()
        {
            //this.BestVendors = (await this.service.GetVendorsAsync()).Take(2).ToArray();
            //this.RecentProducts = (await this.service.GetProductsAsync()).Take(3).ToArray();
            //var ordersToTake = (await this.service.GetOrdersAsync()).Take(3).Select(c => c.Id).ToArray();
            var orders = new ObservableCollection<Order>();
            //foreach (var item in ordersToTake)
            //{
            //    var order = await this.service.GetOrderAsync(item);
            //    orders.Add(order);
            //}
            this.LatestOrders = orders;


            SalesChannels = new NameValuePair[]
            {
                    new NameValuePair("Aquifer Condition", 0.25),
                    new NameValuePair("Air Quality", 0.15),
                    new NameValuePair("Permit Application", 0.40),
                    new NameValuePair("Fish Farming", 0.20)
            };

            BusinessOverview = new NameValuePair[]
            {
                    new NameValuePair("Flagged as do-not-track", 0.15),
                    new NameValuePair("Issue In-Progress", 0.20),
                    new NameValuePair("Proposed Resolution", 0.20),
                    new NameValuePair("Re-Opened", 0.15),
                    new NameValuePair("Unresorvable", 0.25),
                    new NameValuePair("Issue Resolved", 0.05)
            };



            NewCustomers = new NameValuePair[]
            {
                    new NameValuePair("Grengrass First Nation", 1063),
                    new NameValuePair("Smilkameen Rivers", 1048),
                    new NameValuePair("Leasaki River First Nation", 1045),
                    new NameValuePair("Biodiversity Initiative", 189),
                    new NameValuePair("BC Enviromental Collective",812)
            };

            CompletedCommitment = new NameValuePair[]
            {
                    new NameValuePair(DateTime.Today.AddMonths(-2).ToString("MMMM"), 1500),
                    new NameValuePair(DateTime.Today.AddMonths(-1).ToString("MMMM"), 1400),
                    new NameValuePair(DateTime.Today.ToString("MMMM"), 1600)
            };

            InProgressCommitment = new NameValuePair[]
            {
                    new NameValuePair(DateTime.Today.AddMonths(-2).ToString("MMMM"), 1723),
                    new NameValuePair(DateTime.Today.AddMonths(-1).ToString("MMMM"), 1413),
                    new NameValuePair(DateTime.Today.ToString("MMMM"), 2313)
            };
            OnHoldCommitment = new NameValuePair[]
         {
                    new NameValuePair(DateTime.Today.AddMonths(-2).ToString("MMMM"), 700),
                    new NameValuePair(DateTime.Today.AddMonths(-1).ToString("MMMM"), 850),
                    new NameValuePair(DateTime.Today.ToString("MMMM"), 1000)
         };
        }

    }
}
