using System;
using System.Collections.Generic;
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
            Linked_SelectedCommunications = new List<Team>();
            Linked_SelectedEvents = new List<Team>();
            Linked_SelectedStakeholder = new List<LandParcelStakeholder>();
            Linked_SelectedTopics = new List<Team>();
            Linked_SelectedTeam = new List<Team>();
        }

        public D d { get; set; }
        public LoginAPIBody loginAPIBody { get; set; }
        public Object attachment { get; set; }
        public Type Sender { get; set; }
        public string SelectedCommunications { get; set; }

        public EventFormValue EventFormValue { get; set; }
        public CommunicationReply CommunicationReply { get; set; }
        public string fromPage { get; set; }
        public List<Communication.Team> Linked_SelectedCommunications { get; set; }
        public List<Communication.Team> Linked_SelectedEvents { get; set; }
        public List<Communication.LandParcelStakeholder> Linked_SelectedStakeholder { get; set; }
        public List<Communication.Team> Linked_SelectedTopics { get; set; }
        public List<Team> Linked_SelectedTeam { get; set; }
    }
}
