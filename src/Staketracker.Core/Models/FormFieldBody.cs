using System;
using System.Collections.Generic;
using System.Text;

namespace Staketracker.Core.Models.ApiRequestBody
{

    public class FormFieldBody
    {
        public FormFieldBody(AuthReply authReply, string type)
        {

            userId = authReply.d.userId;
            projectId = authReply.d.projectId;
            this.jsonText = "{\"userId\":" + userId + ",\"projectId\":" + projectId;

            if (!String.IsNullOrEmpty(type))
            {

                this.jsonText = this.jsonText + ",\"Type\":\"" + type + "\"}";
                ;
            }
            else
            {
                this.jsonText = this.jsonText + "}";
            }
        }
        public String jsonText { get; set; }

        public int userId { get; set; }
        public int projectId { get; set; }
        private string type { get; set; }
    }


}
