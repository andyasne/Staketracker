using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.EventsFormValue
{
    public class InputFieldValue
    {
        public string PrimaryKey { get; set; }
        public object Value { get; set; }
    }

    public class LandParcelStakeholder
    {
        public string RelationshipKey { get; set; }
        public string LandInterestKey { get; set; }
        public string StakeHolderKey { get; set; }
        public string LandParcelKey { get; set; }
    }

    public class Team
    {
        public string PrimaryKey { get; set; }
    }

    public class LinkTo
    {
        public IList<LandParcelStakeholder> LandParcelStakeholders { get; set; }
        public IList<Team> Team { get; set; }
    }

    public class EventFormValue
    {
        public string PrimaryKey { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public IList<InputFieldValue> InputFieldValues { get; set; }
        public LinkTo LinkTo { get; set; }
    }


}
