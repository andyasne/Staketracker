using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.RequestSwitchProject
{
    public class RequestSwitchProject
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }

    public class ResponseSwitchProject
    {
        public D d { get; set; }

    }
    public class D
    {
        public string Status { get; set; }

    }
}
