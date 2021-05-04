using System;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace Staketracker.Core.Helpers.Behaviours
{
    public class EntryLineValidationBehaviour : BehaviorBase<RadEntry>
    {
        #region StaticFields
        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(EntryLineValidationBehaviour), true, BindingMode.Default, null, (bindable, oldValue, newValue) => OnIsValidChanged(bindable, newValue));
        #endregion
        #region Properties
        public bool IsValid
        {
            get
            {
                return (bool)GetValue(IsValidProperty);
            }
            set
            {
                SetValue(IsValidProperty, value);
            }
        }
        #endregion
        #region StaticMethods
        private static void OnIsValidChanged(BindableObject bindable, object newValue)
        {
            if (bindable is EntryLineValidationBehaviour IsValidBehavior &&
                 newValue is bool IsValid)
            {
                IsValidBehavior.AssociatedObject.WatermarkTextColor = IsValid ? Color.Default : Color.Red;
            }
        }

        #endregion
    }
}
