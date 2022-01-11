using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.DelRec
{
    public class DelRecReplyModel
    {
        public string d { get; set; }

    }
    public class DelRecReqModel
    {
        public int ScreenId { get; set; }
        public int KeyId { get; set; }
    }


}
