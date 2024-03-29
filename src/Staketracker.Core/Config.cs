using System.Text.RegularExpressions;

namespace Staketracker.Core
{
    public static class Config
    {
        public static string ApiUrl = "https://www.sustainet.com/";

        public static string StaketrackerApiUrl = "https://www.staketracker.com";

        public static string ApiHostName
        {
            get
            {
                var apiHostName = Regex.Replace(ApiUrl, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase)
                                   .Replace("/", string.Empty);
                return apiHostName;
            }
        }
    }
}
