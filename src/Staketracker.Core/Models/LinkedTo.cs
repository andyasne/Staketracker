using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models
{
    public class LandInterestSelectionVariables
    {
        public string myKey { get; set; }
        public string parentKey { get; set; }
    }

    public class LandParcel
    {
        public int counters { get; set; }
        public bool mandatory { get; set; }
        public string buttonLabel { get; set; }
        public object arraysNameObject { get; set; }
        public List<string> sortVariable { get; set; }
        public string sortBy { get; set; }
        public string linkedTo { get; set; }
        public string contactAndInterestDetailArrayName { get; set; }
        public bool enableEditing { get; set; }
        public bool enableLandInterestSelection { get; set; }
        public bool landParcelEnableContactSelection { get; set; }
        public bool groupEnableContactSelection { get; set; }
        public LandInterestSelectionVariables landInterestSelectionVariables { get; set; }
    }

    public class LinkedTo
    {
        public LandParcel LandParcel { get; set; }
    }


    public class LinkedToConfig
    {
        public static List<KeyValuePair<String, LinkedTo>> CommunicationsPage = new List<KeyValuePair<String, LinkedTo>>();
        public static List<KeyValuePair<String, LinkedTo>> projectTeamPage = new List<KeyValuePair<String, LinkedTo>>();
        public static List<KeyValuePair<String, LinkedTo>> eventsPage = new List<KeyValuePair<String, LinkedTo>>();
        public LinkedToConfig()
        {
            CommunicationsPage.Add(new KeyValuePair<string, LinkedTo>("Stakeholder",
                new LinkedTo()
                {
                    LandParcel = new LandParcel()
                    {
                        counters = 3,
                        mandatory = true,
                        buttonLabel = "Stakeholders",
                        arraysNameObject = new List<string>() { "IndividualStakeholders", "GroupedStakeholders", "LandParcelStakeholders" },
                        sortVariable = new List<string>() { "LastName", "GroupName", "LegalDescription" },
                        sortBy = "alpha",
                        linkedTo = "Communications",
                        enableEditing = true,
                        landParcelEnableContactSelection = true,
                        groupEnableContactSelection = true
                    }
                }));

            CommunicationsPage.Add(new KeyValuePair<string, LinkedTo>("Issue",
                new LinkedTo()
                {
                    LandParcel = new LandParcel()
                    {
                        counters = 1,
                        mandatory = false,
                        buttonLabel = "Topics",
                        arraysNameObject = "Issues",
                        sortVariable = new List<string>() { "Name" },
                        sortBy = "alpha",
                        linkedTo = "Communications",
                        enableEditing = true

                    }
                }));

            projectTeamPage.Add(new KeyValuePair<string, LinkedTo>("Event",
               new LinkedTo()
               {
                   LandParcel = new LandParcel()
                   {
                       counters = 1,
                       mandatory = false,
                       buttonLabel = "Event",
                       arraysNameObject = "Event",
                       sortVariable = new List<string>() { "EventDate" },
                       sortBy = "Chrono",
                       linkedTo = "Team",
                       enableEditing = true

                   }
               }));
            eventsPage.Add(new KeyValuePair<string, LinkedTo>("Communication",
              new LinkedTo()
              {
                  LandParcel = new LandParcel()
                  {
                      counters = 1,
                      mandatory = false,
                      buttonLabel = "Communication",
                      arraysNameObject = "Communication",
                      sortVariable = new List<string>() { "Date" },
                      sortBy = "Chrono",
                      linkedTo = "Event",
                      enableEditing = true

                  }
              }));

            eventsPage.Add(new KeyValuePair<string, LinkedTo>("Stakeholder",
            new LinkedTo()
            {
                LandParcel = new LandParcel()
                {
                    counters = 3,
                    mandatory = false,
                    buttonLabel = "Stakeholders",
                    arraysNameObject = new List<string>() { "IndividualStakeholders", "GroupedStakeholders", "LandParcelStakeholders" },
                    sortVariable = new List<string>() { "LastName", "GroupName", "LegalDescription" },
                    sortBy = "alpha",
                    linkedTo = "Event",
                    enableEditing = true,
                    groupEnableContactSelection = true,
                    landParcelEnableContactSelection = true

                }
            }));


            eventsPage.Add(new KeyValuePair<string, LinkedTo>("Project Team",
             new LinkedTo()
             {
                 LandParcel = new LandParcel()
                 {
                     counters = 1,
                     mandatory = false,
                     buttonLabel = "Project Team",
                     arraysNameObject = "Team",
                     sortVariable = new List<string>() { "LastName" },
                     sortBy = "alpha",
                     linkedTo = "Event",
                     enableEditing = false

                 }
             }));

            eventsPage.Add(new KeyValuePair<string, LinkedTo>("Issue",
           new LinkedTo()
           {
               LandParcel = new LandParcel()
               {
                   counters = 1,
                   mandatory = false,
                   buttonLabel = "Topics",
                   arraysNameObject = "Issues",
                   sortVariable = new List<string>() { "Name" },
                   sortBy = "alpha",
                   linkedTo = "Event",
                   enableEditing = false

               }
           }));



        }
    }
}
