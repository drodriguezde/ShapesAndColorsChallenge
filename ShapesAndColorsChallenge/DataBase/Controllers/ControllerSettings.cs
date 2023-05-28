using ShapesAndColorsChallenge.Class;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
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
                ShowHowToPlay = "0000000000000000",
                PlayerToken = string.Empty
            };

            DataBaseManager.Connection.Insert(newUserSettings);
        }

        /// <summary>
        /// Establece la tabla en base a los datos descargados desde una partida guardada en la nube.
        /// </summary>
        internal static void Deploy(Settings settings)
        {
            DataBaseManager.Connection.DropTable<Settings>();
            DataBaseManager.Connection.CreateTable<Settings>();

            Settings newUserSettings = new()
            {
                Id = 1,
                Notifications = settings.Notifications,
                Music = settings.Music,
                Sounds = settings.Sounds,
                Voices = settings.Voices,
                Vibration = settings.Vibration,
                DarkMode = settings.DarkMode,
                AlwaysDarkMode = settings.AlwaysDarkMode,
                CountryCode = settings.CountryCode,
                PlayerName = settings.PlayerName,
                PlayerCountryCode = settings.PlayerCountryCode,
                ShowHowToPlay = settings.ShowHowToPlay,
                PlayerToken = settings.PlayerToken
            };

            DataBaseManager.Connection.Insert(newUserSettings);

            Player player = ControllerPlayer.Get().Single(t => t.IsPlayer);
            player.Name = settings.PlayerName;
            player.Country = settings.PlayerCountryCode;
            ControllerPlayer.Update(player);
        }

        internal static void Reset()
        {
            Settings settings = Get();
            settings.ShowHowToPlay = "0000000000000000";
            Update(settings);
        }

        /// <summary>
        /// Obtiene la configuración del usuario.
        /// </summary>
        /// <returns></returns>
        internal static Settings Get()
        {
            return DataBaseManager.Connection.Table<Settings>().First();
        }

        internal static bool GetShowHowToPlay(GameMode gameMode)
        {
            string showHowToPlay = Get().ShowHowToPlay;

            return gameMode switch
            {
                GameMode.Classic => showHowToPlay.Substring(0, 1).ToBool(),
                GameMode.Incremental => showHowToPlay.Substring(1, 1).ToBool(),
                GameMode.Endless => showHowToPlay.Substring(2, 1).ToBool(),
                GameMode.Move => showHowToPlay.Substring(3, 1).ToBool(),
                GameMode.Memory => showHowToPlay.Substring(4, 1).ToBool(),
                GameMode.Blink => showHowToPlay.Substring(5, 1).ToBool(),
                GameMode.TimeTrial => showHowToPlay.Substring(6, 1).ToBool(),
                GameMode.Rotate => showHowToPlay.Substring(7, 1).ToBool(),
                GameMode.ClassicPlus => showHowToPlay.Substring(8, 1).ToBool(),
                GameMode.IncrementalPlus => showHowToPlay.Substring(9, 1).ToBool(),
                GameMode.EndlessPlus => showHowToPlay.Substring(10, 1).ToBool(),
                GameMode.MovePlus => showHowToPlay.Substring(11, 1).ToBool(),
                GameMode.MemoryPlus => showHowToPlay.Substring(12, 1).ToBool(),
                GameMode.BlinkPlus => showHowToPlay.Substring(13, 1).ToBool(),
                GameMode.TimeTrialPlus => showHowToPlay.Substring(14, 1).ToBool(),
                GameMode.RotatePlus => showHowToPlay.Substring(15, 1).ToBool(),
                _ => false,
            };
        }

        internal static void SetHowToPlay(GameMode gameMode, bool value)
        {
            string showHowToPlay = Get().ShowHowToPlay;

            showHowToPlay = gameMode switch
            {
                GameMode.Classic => string.Concat(value.ToInt().ToString(), showHowToPlay.Substring(1, 15)),
                GameMode.Incremental => string.Concat(showHowToPlay.Substring(0, 1), value.ToInt().ToString(), showHowToPlay.Substring(1, 14)),
                GameMode.Endless => string.Concat(showHowToPlay.Substring(0, 2), value.ToInt().ToString(), showHowToPlay.Substring(1, 13)),
                GameMode.Move => string.Concat(showHowToPlay.Substring(0, 3), value.ToInt().ToString(), showHowToPlay.Substring(1, 12)),
                GameMode.Memory => string.Concat(showHowToPlay.Substring(0, 4), value.ToInt().ToString(), showHowToPlay.Substring(1, 11)),
                GameMode.Blink => string.Concat(showHowToPlay.Substring(0, 5), value.ToInt().ToString(), showHowToPlay.Substring(1, 10)),
                GameMode.TimeTrial => string.Concat(showHowToPlay.Substring(0, 6), value.ToInt().ToString(), showHowToPlay.Substring(1, 9)),
                GameMode.Rotate => string.Concat(showHowToPlay.Substring(0, 7), value.ToInt().ToString(), showHowToPlay.Substring(1, 8)),
                GameMode.ClassicPlus => string.Concat(showHowToPlay.Substring(0, 8), value.ToInt().ToString(), showHowToPlay.Substring(1, 7)),
                GameMode.IncrementalPlus => string.Concat(showHowToPlay.Substring(0, 9), value.ToInt().ToString(), showHowToPlay.Substring(1, 6)),
                GameMode.EndlessPlus => string.Concat(showHowToPlay.Substring(0, 10), value.ToInt().ToString(), showHowToPlay.Substring(1, 5)),
                GameMode.MovePlus => string.Concat(showHowToPlay.Substring(01, 11), value.ToInt().ToString(), showHowToPlay.Substring(1, 4)),
                GameMode.MemoryPlus => string.Concat(showHowToPlay.Substring(0, 12), value.ToInt().ToString(), showHowToPlay.Substring(1, 3)),
                GameMode.BlinkPlus => string.Concat(showHowToPlay.Substring(0, 13), value.ToInt().ToString(), showHowToPlay.Substring(1, 2)),
                GameMode.TimeTrialPlus => string.Concat(showHowToPlay.Substring(0, 14), value.ToInt().ToString(), showHowToPlay.Substring(1, 1)),
                GameMode.RotatePlus => string.Concat(showHowToPlay.Substring(0, 15), value.ToInt().ToString()),
                _ => showHowToPlay,
            };

            Settings settings = Get();
            settings.ShowHowToPlay = showHowToPlay;
            Update(settings);
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
