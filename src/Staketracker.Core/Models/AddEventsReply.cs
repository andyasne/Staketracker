namespace Staketracker.Core.Models.AddEventsReply
{
    public class D
    {
        public int status { get; set; }
        public string message { get; set; }
        public string PrimaryKey { get; set; }
        public bool successful { get; set; }
    }

    public class AddEventsReply
    {
        public D d { get; set; }
    }


}
