using System;
using MvvmCross.Forms.Presenters.Attributes;

namespace Staketracker.Core
{
    public class MvxCustomMasterDetailPagePresentationAttribute : MvxMasterDetailPagePresentationAttribute
    {
        public MvxCustomMasterDetailPagePresentationAttribute(MasterDetailPosition position = MasterDetailPosition.Detail)
            : base(position) { }

        public Type MasterHostViewType { get; set; }
    }
}
