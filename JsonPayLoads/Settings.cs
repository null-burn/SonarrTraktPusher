using System.Collections.Generic;

namespace SonarrTraktPusher.JsonPayLoads
{
    public class Settings
    {
        public string ApiUrl = "https://api.trakt.tv";
        public string DeviceCode { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string SonarrRootFolderPath { get; set; }
        public int SonarrProfileId { get; set; }
        public string SonarrApiSeries { get; set; }
        public string SonarrApiKey { get; set; }
        public TraktUserInfo[] TraktUserInfo { get; set; }
    }
    public class TraktUserInfo
    {
        public string user { get; set; }
        public string listName { get; set; }
    }
}
