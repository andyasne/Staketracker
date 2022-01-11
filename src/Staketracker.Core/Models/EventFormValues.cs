using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using Newtonsoft.Json;

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
        public List<LandParcelStakeholder> LandParcelStakeholders { get; set; }
        public List<Team> Team { get; set; }
    }

    public class EventFormValue
    {
        public EventFormValue()
        {
            InputFieldValues = new List<InputFieldValue>();
            LinkTo = new LinkTo() { LandParcelStakeholders = new List<LandParcelStakeholder>(), Team = new List<Team>() };
        }

        public string PrimaryKey { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public List<InputFieldValue> InputFieldValues { get; set; }
        public LinkTo LinkTo { get; set; }


    }

    public class jsonTextObj
    {

        public jsonTextObj(Object Obj)
        {
            jsonText = GetStringFormat(Obj);
        }

        public jsonTextObj()
        {

        }

        public string jsonText { get; set; }

        private string GetStringFormat(Object obj)
        {
            String json = JsonConvert.SerializeObject(obj);
            return json;
        }

    }
}
