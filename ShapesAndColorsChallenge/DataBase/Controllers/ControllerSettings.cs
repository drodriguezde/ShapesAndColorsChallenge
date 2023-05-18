using ShapesAndColorsChallenge.Class;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Tables;
using System.Linq;

namespace ShapesAndColorsChallenge.DataBase.Controllers
{
    internal static class ControllerSettings
    {
        #region METHODS

        /// <summary>
        /// Despliega la tabla de configuración de usuario.
        /// </summary>
        internal static void Deploy()
        {
            //DataBaseManager.Connection.DropTable<Settings>();
            DataBaseManager.Connection.CreateTable<Settings>();

            if (Any())
                return;

            Settings newUserSettings = new()
            {
                Id = 1,
                Notifications = true,
                Music = true,
                Sounds = true,
                Voices = true,
                Vibration = true,
                DarkMode = true,
                AlwaysDarkMode = false,
                CountryCode = Statics.GetSystemLanguage(),
                PlayerName = "PLAYER",
                PlayerCountryCode = Statics.GetSystemLanguage(),
            };

            DataBaseManager.Connection.Insert(newUserSettings);
        }

        /// <summary>
        /// Obtiene la configuración del usuario.
        /// </summary>
        /// <returns></returns>
        internal static Settings Get()
        {
            return DataBaseManager.Connection.Table<Settings>().First();
        }

        internal static bool Any()
        {
            return DataBaseManager.Connection.Table<Settings>().Any();
        }

        /// <summary>
        /// Actualiza la configuración de usuario.
        /// </summary>
        /// <param name="userSettings"></param>
        internal static void Update(Settings userSettings)
        {
            DataBaseManager.Connection.Update(userSettings);
        }

        #endregion
    }
}
