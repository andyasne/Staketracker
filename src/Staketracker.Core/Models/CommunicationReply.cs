using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.Communication
{
    public class GroupedStakeholder
    {
        public object PrimaryKey { get; set; }
        public string GroupName { get; set; }
        public object LandInterestKey { get; set; }
        public object LandInterest { get; set; }
        public object RelationshipKey { get; set; }
        public object ContactTypeKey { get; set; }
        public object ContactTypeName { get; set; }
    }

    public class Stakeholders
    {
        public List<GroupedStakeholder> GroupedStakeholders { get; set; }
    }

    public class D
    {
        public string ID { get; set; }
        public object Title { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public string CommunicationSubject { get; set; }
        public Stakeholders Stakeholders { get; set; }
        public string IssueSubject { get; set; }
        public string PrimaryKey { get; set; }

        public string GetDateString
        {
            get
            {
                if (this.Date != null)
                {
                    return this.Date.ToShortDateString();
                }
                else
                    return null;

            }
        }
    }

    public class CommunicationReply
    {
        public List<D> d { get; set; }
    }


}
