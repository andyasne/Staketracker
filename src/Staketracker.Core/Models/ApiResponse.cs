using System;
using Staketracker.Core.Models.Communication;
using Staketracker.Core.Models.EventsFormValue;

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
        public AuthReply()
        {
            this.d = new D();
        }

        public D d { get; set; }
        public LoginAPIBody loginAPIBody { get; set; }
        public Object attachment { get; set; }
        public Type Sender { get; set; }
        public string SelectedCommunications { get; set; }

        public EventFormValue EventFormValue { get; set; }
        public CommunicationReply CommunicationReply { get; set; }
        public string fromPage { get; set; }
    }
}
