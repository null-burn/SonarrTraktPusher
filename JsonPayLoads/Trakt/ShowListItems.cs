using System;

namespace SonarrTraktPusher.JsonPayLoads.Trakt
{
    public class ShowList
    {
        public int rank { get; set; }
        public int id { get; set; }
        public DateTime listed_at { get; set; }
        public object notes { get; set; }
        public string type { get; set; }
        public Show show { get; set; }
    }

    public class Show
    {
        public string title { get; set; }
        public int year { get; set; }
        public Ids ids { get; set; }
    }

    public class Ids
    {
        public int trakt { get; set; }
        public string slug { get; set; }
        public int tvdb { get; set; }
        public string imdb { get; set; }
        public int tmdb { get; set; }
        public int tvrage { get; set; }
    }
}
