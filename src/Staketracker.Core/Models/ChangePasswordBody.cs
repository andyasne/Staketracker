using System;

namespace Staketracker.Core.Models.ApiRequestBody
{

    public class ChangePasswordBody
    {

        public ChangePasswordBody()
        {

        }


        public String UserId { get; set; }
        public String CurrentPassword { get; set; }
        public String NewPassword { get; set; }
        public String ConfirmNewPassword { get; set; }
    }


}
