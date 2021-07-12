using System;
using System.Collections.Generic;

namespace Staketracker.Core.Models.Events
{
    public class Detail
    {
        public object Description { get; set; }
    }

    public class D
    {
        public IList<Detail> Details { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime EventDate { get; set; }
        public string PrimaryKey { get; set; }

        public string GetDateString
        {
            get
            {
                if (this.EventDate != null)
                {
                    return this.EventDate.ToShortDateString();
                }
                else
                    return null;

            }
        }

    }

    public class EventsReply
    {
        public IList<D> d { get; set; }
    }


}
