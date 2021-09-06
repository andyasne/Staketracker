using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Staketracker.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Staketracker.UI.Pages
{

    [MvxMasterDetailPagePresentation(MasterDetailPosition.Root, WrapInNavigationPage = false)]
    public partial class RV : MvxMasterDetailPage<Rvm>
    {
        public RV()
        {
            InitializeComponent();
        }


    }
}
