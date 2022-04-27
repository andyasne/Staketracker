using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.Communication
{
    public class CommunicationDetailReq
    {
        public CommunicationDetailReq()
        {

        }
        public int userId { get; set; }
        public int projectId { get; set; }
        public int ID { get; set; }
    }
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
    public class LandParcelStakeholder
    {
        public LandParcelStakeholder()
        {
            RelationshipKey = "";
            LandInterestKey = "";
            StakeHolderKey = "";
            LandParcelKey = "";




        }
        public string RelationshipKey { get; set; }
        public string LandInterestKey { get; set; }
        public string StakeHolderKey { get; set; }
        public string LandParcelKey { get; set; }
    }
    public class Stakeholders
    {
        public List<GroupedStakeholder> GroupedStakeholders { get; set; }
    }

    public class LinkTo
    {
        public List<LandParcelStakeholder> LandParcelStakeholders { get; set; }
        public List<Team> Team { get; set; }
        public List<Team> Issue { get; set; }
    }
    public class Team
    {
        public Team()
        {
            PrimaryKey = "";



            ;
        }
        public string PrimaryKey { get; set; }
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
        public LinkTo LinkTo { get; set; }

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
