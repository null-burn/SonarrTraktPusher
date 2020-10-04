using System;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;
using SonarrTraktPusher.JsonPayLoads.Sonarr;
using SonarrTraktPusher.JsonPayLoads.Trakt;
using System.Collections.Generic;

namespace SonarrTraktPusher.Helpers
{
    public class Sonarr
    {
        public class Status
        {
            public string SeriesName { get; set; }
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }

        public bool CreateSeries(ShowList showList, List<GetSeries> getSeries)
        {
            Status status = new Status();
            try
            {
                if (!getSeries.Where(x => x.tvdbId == showList.show.ids.tvdb).Any())
                {
                    AddSeries series = new AddSeries
                    {
                        monitored = true,
                        tvdbId = showList.show.ids.tvdb,
                        title = showList.show.title,
                        titleSlug = showList.show.ids.slug,
                        seasonFolder = true,
                        profileId = Settings._settings.SonarrProfileId,
                        rootFolderPath = $"{Settings._settings.SonarrRootFolderPath}/{showList.show.title}"
                    };

                    AddOptions addOptions = new AddOptions { ignoreEpisodesWithFiles = false, searchForMissingEpisodes = true };
                    series.addOptions = addOptions;

                    status = PushSeries(series);

                    if (status.Success)
                    {
                        Console.WriteLine($"Added {series.title}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to add {series.title}, {status.ErrorMessage}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add {showList.show.title}, {ex.Message}");
                return false;
            }
        }

        public Status PushSeries(AddSeries series)
        {
            HttpClient http = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Settings._settings.SonarrApiSeries);
            request.Headers.Add("X-Api-Key", Settings._settings.SonarrApiKey);

            string json = JsonConvert.SerializeObject(series);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            Status status = new Status();
            HttpResponseMessage responseMessage = http.SendAsync(request).GetAwaiter().GetResult();
            status.SeriesName = series.title;
            status.Success = responseMessage.StatusCode == HttpStatusCode.Created;

            if (!status.Success)
            {
                status.ErrorMessage = responseMessage.Content.ReadAsStringAsync().Result;
            }

            return status;
        }

        public List<GetSeries> GetSeries()
        {
            List<GetSeries> getSeries = new List<GetSeries>();
            try
            {
                HttpClient http = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Settings._settings.SonarrApiSeries);
                request.Headers.Add("X-Api-Key", Settings._settings.SonarrApiKey);

                HttpResponseMessage responseMessage = http.SendAsync(request).GetAwaiter().GetResult();

                var jsonData = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                getSeries = JsonConvert.DeserializeObject<List<GetSeries>>(jsonData);

                return getSeries;
            }
            catch
            {
                return getSeries;
            }
        }
    }
}
