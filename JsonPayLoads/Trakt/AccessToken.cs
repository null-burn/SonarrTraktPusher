namespace SonarrTraktPusher.JsonPayLoads.Trakt
{
    class AccessTokenRequest
    {
        public string code { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }

    class AccessTokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public int created_at { get; set; }
    }

    class DeviceCodeRequest
    {
        public string client_id { get; set; }
    }

    class DeviceCodeResponse
    {
        public string device_code { get; set; }
        public string user_code { get; set; }
        public string verification_url { get; set; }
        public int expires_in { get; set; }
        public int interval { get; set; }
    }
}
