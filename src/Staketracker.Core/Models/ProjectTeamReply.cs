using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Staketracker.Core.Models.ProjectTeam
{
    public class Detail
    {
        public string Department { get; set; }
        public string Organization { get; set; }
    }

    public class D
    {
        public IList<Detail> Details { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PrimaryKey { get; set; }

        public string FullName => $"{LastName}, {LastName} ({Details[0].Department})";
    }

    public class ProjectTeamReply
    {
        public List<D> d { get; set; }

    }


}
