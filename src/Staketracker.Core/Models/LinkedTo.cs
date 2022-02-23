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
        public List<KeyValuePair<String, LinkedTo>> CommunicationsPage = new List<KeyValuePair<String, LinkedTo>>();
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

            CommunicationsPage.Add(new KeyValuePair<string, LinkedTo>("Team Member",
                new LinkedTo()
                {
                    LandParcel = new LandParcel()
                    {
                        counters = 1,
                        mandatory = true,
                        buttonLabel = "Team members",
                        arraysNameObject = "Team",
                        sortVariable = new List<string>() { "LastName" },
                        sortBy = "alpha",
                        linkedTo = "Communications",
                        enableEditing = true

                    }
                }));

        }
    }
}
