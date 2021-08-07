using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.Stakeholders
{
    public class Contact
    {
        //public GroupedStakeholders GroupedStakeholders { get; set; }
        //public LandParcelStakeholders LandParcelStakeholders { get; set; }

    }

    public class IndividualStakeholder
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object NickName { get; set; }
        public string WorkPhone { get; set; }
        public string Email { get; set; }
        public Contact Contact { get; set; }
        public string PrimaryKey { get; set; }
    }

    public class GroupedStakeholder
    {
        public string GroupName { get; set; }
        public string MainPhone { get; set; }
        public string Email { get; set; }

        //   public Contact { get; set; }
        public string PrimaryKey { get; set; }
    }

    public class LandParcelStakeholder
    {
        public string LegalDescription { get; set; }
        public string LandCategory { get; set; }
        public string AddressLine1 { get; set; }
        public string CityorTown { get; set; }

        // public Contact { get; set; }
        public string PrimaryKey { get; set; }
    }

    public class D
    {
        public List<IndividualStakeholder> IndividualStakeholders { get; set; }
        public List<GroupedStakeholder> GroupedStakeholders { get; set; }
        public List<LandParcelStakeholder> LandParcelStakeholders { get; set; }
    }

    public class Stakeholders
    {
        public Staketracker.Core.Models.Stakeholders.D d { get; set; }
    }

    public class LinkedTo
    {
        public LinkedTo()
        {
            GroupedStakeholder = "";
        }
        public string GroupedStakeholder { get; set; }
    }

    public class StakeholderBody
    {
        public StakeholderBody()
        {
            LinkedTo = new LinkedTo();
        }
        public int userId { get; set; }
        public int projectId { get; set; }
        public LinkedTo LinkedTo { get; set; }
    }



}
