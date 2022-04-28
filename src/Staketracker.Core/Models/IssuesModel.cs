using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.Issues
{
    public class IssuesDetailReq
    {
        public IssuesDetailReq()
        {

        }
        public int userId { get; set; }
        public int projectId { get; set; }
        public string PrimaryKey { get; set; }
    }
    public class Detail
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
    }

    public class D
    {
        public IList<Detail> Details { get; set; }
        public string Name { get; set; }
        public string PrimaryKey { get; set; }
        public bool IsChecked { get; internal set; }
    }

    public class IssuesModel
    {
        public IList<D> d { get; set; }
    }


}
