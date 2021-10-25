using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.ChangePasswordReply
{
    public class D
    {
        public int status { get; set; }
        public string message { get; set; }
        public object PrimaryKey { get; set; }
        public bool successful { get; set; }
    }

    public class ChangePasswordReply
    {
        public D d { get; set; }
    }


}
