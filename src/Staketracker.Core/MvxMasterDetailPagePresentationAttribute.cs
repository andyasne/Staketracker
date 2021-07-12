using MvvmCross.Forms.Presenters.Attributes;
using System;

namespace Staketracker.Core
{
    public class MvxCustomMasterDetailPagePresentationAttribute : MvxMasterDetailPagePresentationAttribute
    {
        public MvxCustomMasterDetailPagePresentationAttribute(MasterDetailPosition position = MasterDetailPosition.Detail)
            : base(position) { }

        public Type MasterHostViewType { get; set; }
    }
}
