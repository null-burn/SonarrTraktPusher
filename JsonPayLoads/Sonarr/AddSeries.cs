namespace SonarrTraktPusher.JsonPayLoads.Sonarr
{
    public class AddSeries
    {
        public bool monitored { get; set; }
        public int tvdbId { get; set; }
        public string title { get; set; }
        public string titleSlug { get; set; }
        public bool seasonFolder { get; set; }
        public int profileId { get; set; }
        public string rootFolderPath { get; set; }
        public AddOptions addOptions { get; set; }
    }

    public class AddOptions
    {
        public bool ignoreEpisodesWithFiles { get; set; }
        public bool searchForMissingEpisodes { get; set; }
    }


}
