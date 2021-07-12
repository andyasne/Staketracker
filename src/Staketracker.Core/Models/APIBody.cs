namespace Staketracker.Core.Models
{
    public class JsonText
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginAPIBody
    {
        public LoginAPIBody()
        {
            //  this.jsonText = "{\"username\":\"alem\",\"password\":\"Biniye@99\"}";
        }

        public LoginAPIBody(string username, string password)
        {
            this.jsonText = "{\"username\":\"" + username + "\",\"password\":\"" + password + "\"}";
        }
        //
        public LoginAPIBody(JsonText jsonText)
        {
            _jsonText = jsonText;
            //    this.jsonText = "{\"username\":\"alem\",\"password\":\"Biniye@99\"}";
        }

        private JsonText _jsonText { get; set; }
        public string jsonText { get; set; }
    }
}
