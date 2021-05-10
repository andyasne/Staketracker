using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Navigation;
using Staketracker.Core.ViewModels.Login;
using Xamarin.Forms;

namespace Staketracker.Core.ViewModels.EventsList
{

    public class Customer
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public Uri ImageURL { get; set; }

    }

    public class EventsListViewModel : BaseViewModel

    {
        private Customer selectedCustomer, selectedCustomerDetail;
        private ObservableCollection<Customer> customers;
        private string listDescription;

        private readonly IMvxNavigationService _navigationService;

        public EventsListViewModel(IMvxNavigationService navigationService)
        {

            this.listDescription = "All Customers";

            //this.customers = new ObservableCollection<Customer> { new Customer("Tom"), new Customer("Anna"), new Customer("Peter"), new Customer("Teodor"), new Customer("Lorenzo"), new Customer("Andrea"), new Customer("Martin") };

            _navigationService = navigationService;



        }

        private async Task FetchData()
        {
            Customer cust = new Customer();
            cust.Name = "TEST";
            Customer cust1 = new Customer();
            cust1.Name = "TEST-1";
            Customer cust2 = new Customer();
            cust2.Name = "TEST-2";
            this.customers = new ObservableCollection<Customer>();
            customers.Add(cust);
            customers.Add(cust1);
            customers.Add(cust2);
        }
        public async override void Prepare()
        {
            base.Prepare();
            await this.FetchData();
        }
        public ObservableCollection<Customer> Customers
        {
            get => customers;
            private set => SetField(ref customers, value);
        }

        public Customer SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                if (SetProperty(ref selectedCustomer, value) && value != null)
                {
                    SetField(ref selectedCustomer, value);

                }
            }
        }
        public async Task Refresh()
        {

        }
        public string ListDescription
        {
            get => listDescription;
            private set => SetProperty(ref listDescription, value);
        }


    }
}
