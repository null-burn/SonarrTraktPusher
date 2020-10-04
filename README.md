# SonarrTraktPusher - Alpha Version

My attempt to push Trakt lists to Sonarr using their APIs using .net core. This app should be able to compile/run on windows, linux, mac.

Trakt - https://trakt.tv

Sonarr - https://sonarr.tv/

Things you'll need. These items will need to be placed in the settings.json provided.

* Sonarr
    * API key
    * API URL (i.e. http://192.168.1.220:8989/api/series)
    * Root Folder Path (i.e. /tv)
    * Profile Id (integer value for the Profiles under /settings/profiles)

* Trakt App (https://trakt.tv/oauth/applications)
    * Client Id
    * Client Secret
    * User & List 

```json
{
  "ApiUrl": "https://api.trakt.tv",
  "DeviceCode": "",
  "ClientId": "Trakt Client Id",
  "ClientSecret": "Trakt Client Secret",
  "AccessToken": "",
  "RefreshToken": "",
  "SonarrRootFolderPath": "Sonarr Root Folder Path",
  "SonarrProfileId": Sonarr Profile Id,
  "SonarrApiSeries": "Sonarr API URL",
  "SonarrApiKey": "Sonarr API key",
  "TraktUserInfo": [
    {
      "user": "Required",
      "listName": "Required"
    }
  ]
}
```
Compile and run the console app. It will ask you to go to the Trakt website to authorize the app. A code will be provided you'll need to enter on the website. Once the code is entered and you allow the app access. The app will download the list you supplied and push it to Sonarr. The series created in Sonarr is automatically monitored.