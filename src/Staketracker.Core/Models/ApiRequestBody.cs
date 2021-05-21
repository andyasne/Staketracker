using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.ApiRequestBody
{
    public class JsonText
    {
        public int userId { get; set; }
        public int projectId { get; set; }
        public string LinkedTo { get; set; }
    }

    public class APIRequestBody
    {
        public APIRequestBody(AuthReply authReply)
        {
            _jsonText = new JsonText();
            _jsonText.userId = authReply.d.userId;
            _jsonText.projectId = authReply.d.projectId;
            _jsonText.LinkedTo = "{ \"EVENT\": \"\"}";

            this.jsonText = "{\"userId\":" + _jsonText.userId + ",\"projectId\":" + _jsonText.projectId + ",\"LinkedTo\":" + _jsonText.LinkedTo + "}";



        }
        private JsonText _jsonText { get; set; }
        public String jsonText { get; set; }
    }


}
