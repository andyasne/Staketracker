using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models
{
    public class D
    {
        public int userId { get; set; }
        public string loginName { get; set; }
        public string userName { get; set; }
        public int companyId { get; set; }
        public string businessUnitName { get; set; }
        public int projectId { get; set; }
        public string projectName { get; set; }
        public int cultureId { get; set; }
        public string sessionId { get; set; }
    }

    public class AuthReply
    {
        public D d { get; set; }
    }

}
