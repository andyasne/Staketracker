using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.ResponseProjectList
{
    public class Project
    {
        public int projectId { get; set; }
        public string name { get; set; }
        public string read { get; set; }
        public string add { get; set; }
        public string update { get; set; }
    }

    public class BusinessUnit
    {
        public string name { get; set; }
        public List<Project> projects { get; set; }
    }

    public class D
    {
        public List<BusinessUnit> businessUnits { get; set; }
    }

    public class ResponseProjectList
    {
        public D d { get; set; }
    }

    public class RequestProjectList
    {
        public int userId { get; set; }
    }

}
