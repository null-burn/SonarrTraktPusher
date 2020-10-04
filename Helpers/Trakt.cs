using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using SonarrTraktPusher.JsonPayLoads;
using SonarrTraktPusher.JsonPayLoads.Trakt;

namespace SonarrTraktPusher.Helpers
{
    public class Trakt
    {
        private bool Init(bool tryAgain = false)
        {
            if ((string.IsNullOrEmpty(Settings._settings.DeviceCode) || string.IsNullOrEmpty(Settings._settings.AccessToken) || string.IsNullOrEmpty(Settings._settings.RefreshToken)) || tryAgain)
            {
                if (!Authorize())
                {
                    Console.WriteLine("Error: Unable to authorize");
                    return false;
                }
                else
                {
                    return Settings.WriteSettingsFile();
                }
            }

            return true;
        }

        public void GetListItems()
        {
            if (Init())
            {
                foreach (TraktUserInfo userInfo in Settings._settings.TraktUserInfo)
                {
                    Uri apiUrl = new Uri(Settings._settings.ApiUrl + $"/users/{userInfo.user}/lists/{userInfo.listName}/items/show");
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                    request.Headers.Add("trakt-api-version", "2");
                    request.Headers.Add("trakt-api-key", Settings._settings.ClientId);
                    request.Headers.Add("authorization", $"Bearer {Settings._settings.AccessToken}");

                    HttpClient http = new HttpClient();
                    HttpResponseMessage responseMessage = http.SendAsync(request).GetAwaiter().GetResult();

                    bool access = false;
                    if (responseMessage.IsSuccessStatusCode && responseMessage.StatusCode == HttpStatusCode.OK)
                    {
                        access = true;
                    }
                    else if (responseMessage.StatusCode == HttpStatusCode.Unauthorized && Init(true))
                    {
                        access = true;
                    }

                    if (access)
                    {
                        var jsonData = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        var traktListItems = JsonConvert.DeserializeObject<List<ShowList>>(jsonData);

                        Sonarr sonarr = new Sonarr();
                        var getSeries = sonarr.GetSeries();
                        foreach (var trackListItem in traktListItems)
                        {
                            sonarr.CreateSeries(trackListItem, getSeries);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private bool Authorize()
        {
            try
            {
                HttpClient http = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Settings._settings.ApiUrl + "/oauth/device/code");

                string json = JsonConvert.SerializeObject(new DeviceCodeRequest() { 
                    client_id = Settings._settings.ClientId 
                });

                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage = http.SendAsync(request).GetAwaiter().GetResult();
                if (responseMessage.IsSuccessStatusCode && responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    var jsonData = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var deviceCodeResponse = JsonConvert.DeserializeObject<JsonPayLoads.Trakt.DeviceCodeResponse>(jsonData);
                    Settings._settings.DeviceCode = deviceCodeResponse.device_code;

                    Console.WriteLine($"Go to {deviceCodeResponse.verification_url} and enter this code {deviceCodeResponse.user_code}");

                    int wait = 0;
                    bool tokenRecieved = false;
                    while(wait < (deviceCodeResponse.expires_in * 1000))
                    {
                        Thread.Sleep(deviceCodeResponse.interval * 1000);
                        wait += (deviceCodeResponse.interval * 1000);
                        if (PollForToken())
                        {
                            tokenRecieved = true;
                            break;
                        }
                    }

                    return tokenRecieved;
                }

                Console.WriteLine("Error: Unable to get device code.");
                return false;
            }
            catch
            {
                Console.WriteLine("Error");
                return false;
            }
        }

        private bool PollForToken()
        {
            HttpClient http = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Settings._settings.ApiUrl + "/oauth/device/token");

            string json = JsonConvert.SerializeObject(new AccessTokenRequest() { code = Settings._settings.DeviceCode, client_id = Settings._settings.ClientId, client_secret = Settings._settings.ClientSecret});
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = http.SendAsync(request).GetAwaiter().GetResult();
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var jsonData = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var accessTokenResponse = JsonConvert.DeserializeObject<JsonPayLoads.Trakt.AccessTokenResponse>(jsonData);
                Settings._settings.AccessToken = accessTokenResponse.access_token;
                Settings._settings.RefreshToken = accessTokenResponse.refresh_token;

                return true;
            }

            return false;
        }
    }
}
