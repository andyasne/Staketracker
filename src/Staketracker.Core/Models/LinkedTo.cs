using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.LinkedTo
{
    public class LandInterestSelectionVariables
    {
        public string myKey { get; set; }
        public string parentKey { get; set; }
    }

    public class LinkedTo
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
        public bool enableContactTypeSelection { get; set; }
        public bool enableLandInterestSelection { get; set; }
        public bool landParcelEnableContactSelection { get; set; }
        public bool groupEnableContactSelection { get; set; }
        public LandInterestSelectionVariables landInterestSelectionVariables { get; set; }
        public LandInterestSelectionVariables contactTypeSelectionVariables { get; set; }
    }


    public class LinkedToConfig
    {
        public List<KeyValuePair<String, LinkedTo>> CommunicationsPage = new List<KeyValuePair<String, LinkedTo>>();
        public List<KeyValuePair<String, LinkedTo>> StakeHolders_IndividualPage = new List<KeyValuePair<String, LinkedTo>>();
        public List<KeyValuePair<String, LinkedTo>> StakeHolders_Group = new List<KeyValuePair<String, LinkedTo>>();
        public List<KeyValuePair<String, LinkedTo>> StakeHolders_LandParcel = new List<KeyValuePair<String, LinkedTo>>();
        public List<KeyValuePair<String, LinkedTo>> ProjectTeamPage = new List<KeyValuePair<String, LinkedTo>>();
        public List<KeyValuePair<String, LinkedTo>> EventsPage = new List<KeyValuePair<String, LinkedTo>>();
        public List<KeyValuePair<String, LinkedTo>> IssuesPage = new List<KeyValuePair<String, LinkedTo>>();


        public LinkedToConfig()
        {

            StakeHolders_LandParcel.Add(new KeyValuePair<string, LinkedTo>("Individual",
          new LinkedTo()
          {

              counters = 1,
              mandatory = false,
              buttonLabel = "Individual contacts",
              arraysNameObject = new List<string>() { "IndividualStakeholders" },
              sortVariable = new List<string>() { "LastName" },
              sortBy = "alpha",
              linkedTo = "LandParcelStakeholder",
              enableEditing = true,
              contactAndInterestDetailArrayName = "LandParcelStakeholder",
              enableContactTypeSelection = true,
              contactTypeSelectionVariables = new LandInterestSelectionVariables()
              {
                  myKey = "StakeHolderKey",
                  parentKey = "LandParcelKey"
              }

          }));

            StakeHolders_LandParcel.Add(new KeyValuePair<string, LinkedTo>("Group",
       new LinkedTo()
       {

           counters = 1,
           mandatory = false,
           buttonLabel = "Group contacts",
           arraysNameObject = new List<string>() { "GroupedStakeholders" },
           sortVariable = new List<string>() { "GroupName" },
           sortBy = "alpha",
           linkedTo = "LandParcelStakeholder",
           enableEditing = true,
           contactAndInterestDetailArrayName = "LandParcelStakeholder",
           enableContactTypeSelection = true,
           contactTypeSelectionVariables = new LandInterestSelectionVariables()
           {
               myKey = "GroupedStakeHolderKey",
               parentKey = "LandParcelKey"
           }

       }));
            StakeHolders_LandParcel.Add(new KeyValuePair<string, LinkedTo>("Issue",
       new LinkedTo()
       {

           counters = 1,
           mandatory = false,
           buttonLabel = "Topics",
           arraysNameObject = new List<string>() { "Issues" },
           sortVariable = new List<string>() { "Name" },
           sortBy = "alpha",
           linkedTo = "LandParcelStakeholder",
           enableEditing = false

       }));



            StakeHolders_Group.Add(new KeyValuePair<string, LinkedTo>("Contact",
            new LinkedTo()
            {

                counters = 1,
                mandatory = false,
                buttonLabel = "Contacts",
                arraysNameObject = new List<string>() { "IndividualStakeholders" },
                sortVariable = new List<string>() { "LastName" },
                sortBy = "alpha",
                linkedTo = "GroupedStakeholder",
                enableEditing = true,
                contactAndInterestDetailArrayName = "GroupedStakeholder",
                enableContactTypeSelection = true,
                contactTypeSelectionVariables = new LandInterestSelectionVariables()
                {
                    myKey = "StakeHolderKey",
                    parentKey = "GroupedStakeHolderKey"
                }

            }));


            StakeHolders_Group.Add(new KeyValuePair<string, LinkedTo>("LandParcel",
           new LinkedTo()
           {

               counters = 1,
               mandatory = false,
               buttonLabel = "Land Parcels",
               arraysNameObject = new List<string>() { "LandParcelStakeholders" },
               sortVariable = new List<string>() { "LegalDescription" },
               sortBy = "alpha",
               linkedTo = "GroupedStakeholder",
               enableEditing = true,
               contactAndInterestDetailArrayName = "GroupedStakeholder",
               enableContactTypeSelection = true,
               contactTypeSelectionVariables = new LandInterestSelectionVariables()
               {
                   myKey = "LandParcelKey",
                   parentKey = "GroupedStakeHolderKey"
               }

           }));


            StakeHolders_Group.Add(new KeyValuePair<string, LinkedTo>("Issue",
        new LinkedTo()
        {

            counters = 1,
            mandatory = false,
            buttonLabel = "Topics",
            arraysNameObject = new List<string>() { "Issues" },
            sortVariable = new List<string>() { "Name" },
            sortBy = "alpha",
            linkedTo = "GroupedStakeholder",
            enableEditing = false,
            contactAndInterestDetailArrayName = "GroupedStakeholder",


        }));



            StakeHolders_IndividualPage.Add(new KeyValuePair<string, LinkedTo>("Group",
              new LinkedTo()
              {

                  counters = 1,
                  mandatory = false,
                  buttonLabel = "Groups",
                  arraysNameObject = new List<string>() { "GroupedStakeholders" },
                  sortVariable = new List<string>() { "GroupName" },
                  sortBy = "alpha",
                  linkedTo = "IndividualStakeholder",
                  enableEditing = true,
                  contactAndInterestDetailArrayName = "IndividualStakeholders",
                  enableContactTypeSelection = true,
                  contactTypeSelectionVariables = new LandInterestSelectionVariables()
                  {
                      myKey = "GroupedStakeHolderKey",
                      parentKey = "StakeHolderKey"
                  }

              }));


            StakeHolders_IndividualPage.Add(new KeyValuePair<string, LinkedTo>("Land Parcel",
            new LinkedTo()
            {

                counters = 1,
                mandatory = false,
                buttonLabel = "Land parcels",
                arraysNameObject = new List<string>() { "LandParcelStakeholders" },
                sortVariable = new List<string>() { "LegalDescription" },
                sortBy = "alpha",
                linkedTo = "IndividualStakeholder",
                enableEditing = true,
                contactAndInterestDetailArrayName = "IndividualStakeholders",
                enableContactTypeSelection = true,
                contactTypeSelectionVariables = new LandInterestSelectionVariables()
                {
                    myKey = "LandParcelKey",
                    parentKey = "StakeHolderKey"
                }

            }));



            StakeHolders_IndividualPage.Add(new KeyValuePair<string, LinkedTo>("Issue",
            new LinkedTo()
            {

                counters = 1,
                mandatory = false,
                buttonLabel = "Topics",
                arraysNameObject = new List<string>() { "Issues" },
                sortVariable = new List<string>() { "Name" },
                sortBy = "alpha",
                linkedTo = "IndividualStakeholder",
                enableEditing = false
            }));


            CommunicationsPage.Add(new KeyValuePair<string, LinkedTo>("Stakeholder",
                new LinkedTo()
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

                }));

            CommunicationsPage.Add(new KeyValuePair<string, LinkedTo>("Issue",
                new LinkedTo()
                {

                    counters = 1,
                    mandatory = false,
                    buttonLabel = "Topics",
                    arraysNameObject = "Issues",
                    sortVariable = new List<string>() { "Name" },
                    sortBy = "alpha",
                    linkedTo = "Communications",
                    enableEditing = true


                }));

            CommunicationsPage.Add(new KeyValuePair<string, LinkedTo>("Team Member",
              new LinkedTo()
              {

                  counters = 1,
                  mandatory = false,
                  buttonLabel = "Team members",
                  arraysNameObject = "Team",
                  sortVariable = new List<string>() { "LastName" },
                  sortBy = "alpha",
                  linkedTo = "Communications",
                  enableEditing = true


              }));

            ProjectTeamPage.Add(new KeyValuePair<string, LinkedTo>("Event",
               new LinkedTo()
               {

                   counters = 1,
                   mandatory = false,
                   buttonLabel = "Event",
                   arraysNameObject = "Event",
                   sortVariable = new List<string>() { "EventDate" },
                   sortBy = "Chrono",
                   linkedTo = "Team",
                   enableEditing = true


               }));
            EventsPage.Add(new KeyValuePair<string, LinkedTo>("Communication",
              new LinkedTo()
              {

                  counters = 1,
                  mandatory = false,
                  buttonLabel = "Communication",
                  arraysNameObject = "Communication",
                  sortVariable = new List<string>() { "Date" },
                  sortBy = "Chrono",
                  linkedTo = "Event",
                  enableEditing = false


              }));

            EventsPage.Add(new KeyValuePair<string, LinkedTo>("Stakeholder",
            new LinkedTo()
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


            }));


            EventsPage.Add(new KeyValuePair<string, LinkedTo>("Project Team",
             new LinkedTo()
             {
                 counters = 1,
                 mandatory = false,
                 buttonLabel = "Project Team",
                 arraysNameObject = "Team",
                 sortVariable = new List<string>() { "LastName" },
                 sortBy = "alpha",
                 linkedTo = "Event",
                 enableEditing = false


             }));

            EventsPage.Add(new KeyValuePair<string, LinkedTo>("Issue",
           new LinkedTo()
           {

               counters = 1,
               mandatory = false,
               buttonLabel = "Topics",
               arraysNameObject = "Issues",
               sortVariable = new List<string>() { "Name" },
               sortBy = "alpha",
               linkedTo = "Event",
               enableEditing = false


           }));

            IssuesPage.Add(new KeyValuePair<string, LinkedTo>("Individual",
           new LinkedTo()
           {

               counters = 1,
               mandatory = false,
               buttonLabel = "Individuals",
               arraysNameObject = "IndividualStakeholders",
               sortVariable = new List<string>() { "LastName" },
               sortBy = "alpha",
               linkedTo = "Issue",
               enableEditing = false


           }));


            IssuesPage.Add(new KeyValuePair<string, LinkedTo>("Group",
           new LinkedTo()
           {

               counters = 1,
               mandatory = false,
               buttonLabel = "Groups",
               arraysNameObject = "GroupedStakeholders",
               sortVariable = new List<string>() { "GroupName" },
               sortBy = "alpha",
               linkedTo = "Issue",
               enableEditing = false


           }));


            IssuesPage.Add(new KeyValuePair<string, LinkedTo>("LandParcel",
         new LinkedTo()
         {

             counters = 1,
             mandatory = false,
             buttonLabel = "Land Parcels",
             arraysNameObject = "LandParcelStakeholders",
             sortVariable = new List<string>() { "LegalDescription" },
             sortBy = "alpha",
             linkedTo = "Issue",
             enableEditing = false


         }));



        }
    }
}
