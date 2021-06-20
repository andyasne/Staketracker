using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Staketracker.Core.Models
{
    public class Order : INotifyPropertyChanged
    {
        private string _status, _shipMethod;
        private DateTime _duedate;

        public static string[] AvailableShipMethods = new string[] { "ZY Express", "DSL", "Postal" };

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "orderNumber")]
        public string OrderNumber { get; set; }

        [JsonProperty(PropertyName = "orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonProperty(PropertyName = "dueDate")]
        public DateTime DueDate
        {
            get => _duedate;
            set
            {
                if (_duedate == value)
                    return;

                _duedate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DueDate)));
            }
        }

        [JsonProperty(PropertyName = "isOnline")]
        public bool IsOnline { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;

                _status = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Status)));
            }
        }

        [JsonProperty(PropertyName = "shippingAddressModifiedDate")]
        public DateTime ShippingAddressModifiedDate { get; set; }

        [JsonProperty(PropertyName = "shipMethod")]
        public string ShipMethod
        {
            get => _shipMethod;
            set
            {
                if (_shipMethod == value)
                    return;

                _shipMethod = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShipMethod)));
            }
        }

        [JsonProperty(PropertyName = "customerID")]
        public string CustomerId { get; set; }

        [JsonProperty(PropertyName = "shippingAddressID")]
        public string ShippingAddressId { get; set; }


        public bool IsCompleted => String.Equals(this.Status, "Completed", StringComparison.CurrentCultureIgnoreCase);

        public string Number => OrderNumber;
        public string Description => $"Due Date: {DueDate:dd.MM.yyyy}";

        public event PropertyChangedEventHandler PropertyChanged;

        public override bool Equals(object obj)
        {
            var order = obj as Order;
            return order != null && Id == order.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }

        public Order Copy()
        {
            Order newOrder = new Order()
            {
                Id = this.Id,
                OrderNumber = this.OrderNumber,
                OrderDate = this.OrderDate,
                DueDate = this.DueDate,
                IsOnline = this.IsOnline,
                Status = this.Status,
                ShippingAddressModifiedDate = this.ShippingAddressModifiedDate,
                ShipMethod = this.ShipMethod,
                CustomerId = this.CustomerId,
                ShippingAddressId = this.ShippingAddressId,

            };
            return newOrder;
        }

        public bool Validate(out IList<string> errors)
        {
            errors = new List<string>();

            //if (this.Customer == null)
            //    errors.Add("Customer not selected");
            //if (!this.OrderDetails.Any())
            //    errors.Add("No products added");
            //if (this.ShippingAddress == null)
            //    errors.Add("Shipping address not selected");
            //if (string.IsNullOrEmpty(this.ShipMethod))
            //    errors.Add("Ship method not selected");

            return errors.Count == 0;
        }

        public static bool operator ==(Order order1, Order order2)
        {
            if (System.Object.ReferenceEquals(order1, order2))
                return true;

            if (((object)order1 == null) || ((object)order2 == null))
                return false;

            return order1.Equals(order2);
        }

        public static bool operator !=(Order order1, Order order2)
        {
            return !(order1 == order2);
        }

        public void CompleteOrder()
        {
            this.Status = "Completed";
        }

        //       public void InvalidateAmmount() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Ammount)));
        //     public void InvalidateCustomer() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Customer)));
    }
}
