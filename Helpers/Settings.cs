using Newtonsoft.Json;
using System;
using System.IO;

namespace SonarrTraktPusher.Helpers
{
    public class Settings
    {
        public static JsonPayLoads.Settings _settings = new JsonPayLoads.Settings();
        public static string SettingsBasePathAndFileName = Directory.GetCurrentDirectory() + @"/settings.json";

        public static bool WriteSettingsFile()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_settings, Formatting.Indented);
                File.WriteAllText(SettingsBasePathAndFileName, json);

                return true;
            }
            catch
            {
                Console.Write("Error -- Writing to settings file.");
                return false;
            }
        }

        public static bool ReadSettingsFile()
        {
            string jsonData = null;
            try
            {
                jsonData = File.ReadAllText(SettingsBasePathAndFileName);
            }
            catch
            {
                Console.WriteLine("Error -- Problem reading file. Check permissions or location. I'm looking for it here: " + SettingsBasePathAndFileName);
                return false;
            }

            try
            {
                _settings = JsonConvert.DeserializeObject<JsonPayLoads.Settings>(jsonData);
            }
            catch
            {
                Console.WriteLine("Error -- Check your JSON");
                return false;
            }

            try
            {
                //check required settings
                if (   string.IsNullOrEmpty(_settings.ApiUrl) 
                    || string.IsNullOrEmpty(_settings.ClientId) 
                    || string.IsNullOrEmpty(_settings.ClientSecret) 
                    || string.IsNullOrEmpty(_settings.SonarrRootFolderPath) 
                    || string.IsNullOrEmpty(_settings.SonarrApiSeries)
                    || string.IsNullOrEmpty(_settings.SonarrApiKey)
                    || (_settings.SonarrProfileId == 0)
                    )
                {
                    Console.WriteLine("Error -- Check your settings file, something is wrong or missing required settings.");
                    return false;
                }

                if (_settings.TraktUserInfo.Length == 0)
                {
                    Console.WriteLine("Error -- Check your settings file, you need at least one user and list.");
                    return false;
                }

                foreach (var userInfo in _settings.TraktUserInfo)
                {
                    if (string.IsNullOrEmpty(userInfo.user) || string.IsNullOrEmpty(userInfo.listName))
                    {
                        Console.WriteLine("Error -- Check your settings file, you need a value for both user and list name.");
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                Console.WriteLine("Error -- Check your settings file, something is wrong. Maybe JSON format.");
                return false;
            }
        }
    }
}
