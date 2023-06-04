/***********************************************************************
* DESCRIPTION :
*
*
* NOTES :
* 
* 
* WARNINGS :
* 
* 
* OPTIMIZE IMPORTS : NO
* EXCEPTION CONTROL : NO
* DISPOSE CONTROL : YES
* 
*
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class UserSettingsManager
    {
        #region PROPERTIES

        static Settings UserSettings { get; set; }

        internal static bool Notifications
        {
            get
            {
                return UserSettings.Notifications;
            }

            set
            {
                UserSettings.Notifications = value;
                UpdateData();
            }
        }

        internal static bool Music
        {
            get
            {
                return UserSettings.Music;
            }

            set
            {
                UserSettings.Music = value;
                UpdateData();
            }
        }

        internal static bool Sounds
        {
            get
            {
                return UserSettings.Sounds;
            }

            set
            {
                UserSettings.Sounds = value;
                UpdateData();
            }
        }

        internal static bool Voices
        {
            get
            {
                return UserSettings.Voices;
            }

            set
            {
                UserSettings.Voices = value;
                UpdateData();
            }
        }

        internal static bool Vibration
        {
            get
            {
                return UserSettings.Vibration;
            }

            set
            {
                UserSettings.Vibration = value;
                UpdateData();
            }
        }

        internal static bool DarkMode
        {
            get
            {
                return UserSettings.DarkMode;
            }

            set
            {
                UserSettings.DarkMode = value;
                UpdateData();
            }
        }

        internal static bool AlwaysDarkMode
        {
            get
            {
                return UserSettings.AlwaysDarkMode;
            }

            set
            {
                UserSettings.AlwaysDarkMode = value;
                UpdateData();
            }
        }

        internal static string CountryCode
        {
            get
            {
                return UserSettings.CountryCode;
            }

            set
            {
                UserSettings.CountryCode = value;
                UpdateData();
                LanguageManager.SetLanguage(CountryCode);
            }
        }

        internal static string PlayerName
        {
            get
            {
                return UserSettings.PlayerName;
            }

            set
            {
                UserSettings.PlayerName = value;
                UpdateData();
            }
        }

        internal static string PlayerCountryCode
        {
            get
            {
                return UserSettings.PlayerCountryCode;
            }

            set
            {
                UserSettings.PlayerCountryCode = value;
                UpdateData();
            }
        }

        #endregion

        #region METHODS

        internal static void Initialize()
        {
            UserSettings = ControllerSettings.Get();
            LanguageManager.SetLanguage(CountryCode);
        }

        static void UpdateData()
        {
            ControllerSettings.Update(UserSettings);
        }

        #endregion
    }
}