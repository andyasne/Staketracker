using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models
{

    public class JsonText
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginAPIBody
    {
        public JsonText jsonText { get; set; }
    }


}
