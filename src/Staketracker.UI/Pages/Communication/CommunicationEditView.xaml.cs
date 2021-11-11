using Xamarin.Forms;

namespace Staketracker.UI.Pages.Communication
{
    public partial class CommunicationEditView : ContentView, IPopupHost
    {
        public CommunicationEditView()
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
