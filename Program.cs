using SonarrTraktPusher.Helpers;

namespace SonarrTraktPusher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Settings.ReadSettingsFile())
            {
                Trakt trakt = new Trakt();
                trakt.GetListItems();
            }
        }
    }
}
