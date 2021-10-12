using Xamarin.Forms;

namespace Staketracker.UI.Pages.Events
{
    public partial class EventEditView : ContentView, IPopupHost
    {
        public EventEditView()
        {
            InitializeComponent();
        }

        public void ClosePopup()
        {
            this.popup.IsOpen = false;
        }

        public void OpenPopup()
        {
            this.popup.IsOpen = true;
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            this.ClosePopup();
        }
    }
}
