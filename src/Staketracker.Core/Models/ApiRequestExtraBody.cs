using System;

namespace Staketracker.Core.Models.ApiRequestBody
{


    public class APIRequestExtraBody
    {
        public APIRequestExtraBody(AuthReply authReply, string typeName, string typeValue)
        {
            _jsonText = new JsonText();
            _jsonText.userId = authReply.d.userId;
            _jsonText.projectId = authReply.d.projectId;
            string type = "{ \"EVENT\": \"\"}";

            this.jsonText = "{\"userId\":" + _jsonText.userId + ",\"projectId\":" + _jsonText.projectId + ",\"" + typeName + "\":\"" + typeValue + "\"}";



        }
        private JsonText _jsonText { get; set; }
        public String jsonText { get; set; }
    }


}
