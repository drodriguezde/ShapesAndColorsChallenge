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
* DISPOSE CONTROL : STATIC
* 
*
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using ShapesAndColorsChallenge.Class.Params;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class SoundManager
    {
        #region CONST

        const float VOLUME_MASTER = 1f;
        const float VOLUME_MUSIC = 1f;
        const float VOLUME_FX = 1f;

        #endregion

        #region VARS

        internal static readonly string bgm_01 = "BGM_01";
        internal static readonly string bgm_02 = "BGM_02";
        internal static readonly string bgm_03 = "BGM_03";
        internal static readonly string bgm_04 = "BGM_04";
        internal static readonly string bgm_05 = "BGM_05";
        internal static readonly string bgm_06 = "BGM_06";
        internal static readonly string bgm_07 = "BGM_07";
        internal static readonly string bgm_08 = "BGM_08";
        internal static readonly string bgm_09 = "BGM_09";
        internal static readonly string bgm_10 = "BGM_10";
        internal static readonly string bgm_11 = "BGM_11";
        internal static readonly string bgm_12 = "BGM_12";
        internal static readonly string bgm_13 = "BGM_13";
        internal static readonly string bgm_14 = "BGM_14";
        internal static readonly string bgm_15 = "BGM_15";
        internal static readonly string bgm_16 = "BGM_16";
        internal static readonly string bgm_17 = "BGM_17";
        internal static readonly string bgm_18 = "BGM_18";
        internal static readonly string bgm_19 = "BGM_19";
        internal static readonly string bgm_20 = "BGM_20";

        /// <summary>
        /// Indica la última canción que ha sonado.
        /// </summary>
        static string lastSong = string.Empty;

        static BackgroundWorker backgroundWorkerFadeOutMusic;

        #endregion

        #region PROPERTIES

        internal static SoundEffect ButtonClickCloseWindow { get; private set; }
        internal static SoundEffect ButtonClickOpenWindow { get; private set; }
        internal static SoundEffect TabClick { get; private set; }
        internal static SoundEffect CheckBoxClick { get; private set; }
        internal static SoundEffect PanelSlide { get; private set; }
        internal static SoundEffect Padlock { get; private set; }
        internal static SoundEffect Counter { get; private set; }
        internal static SoundEffect CorrectTile1 { get; private set; }
        internal static SoundEffect CorrectTile2 { get; private set; }
        internal static SoundEffect PerkChange { get; private set; }
        internal static SoundEffect PerkReveal { get; private set; }
        internal static SoundEffect PerkTimeStop { get; private set; }
        internal static SoundEffect Switch { get; private set; }
        internal static SoundEffect WrongTile { get; private set; }
        internal static SoundEffect VoiceNewHighScore { get; private set; }
        internal static SoundEffect VoiceYouLose { get; private set; }
        internal static SoundEffect VoiceYouWin { get; private set; }

        static List<string> Album { get; set; }

        static Song CurrentSong { get; set; }

        internal static bool IsPlaying { get { return MediaPlayer.State == MediaState.Playing; } }

        /// <summary>
        /// Indica en que momento finalizó o finalizará la última pista de voz.
        /// </summary>
        static long LastVoiceEnded { get; set; } = 0;

        /// <summary>
        /// Indica el índice de la última voz positiva que ha sonado.
        /// </summary>
        static int LastVoiceSounded { get; set; } = -1;

        /// <summary>
        /// Colección de voces de feedback positivo.
        /// </summary>
        static SoundEffect[] PositiveFeedbackVoices = new SoundEffect[13];

        /// <summary>
        /// Colección de explosión de globo.
        /// </summary>
        static SoundEffect[] BallonPop = new SoundEffect[3];

        #endregion

        #region EVENTS

        /// <summary>
        /// La duración del desvanecimiento dependerá del volumen actual, si está muy alto será más largo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void BackgroundWorkerFadeOutMusic_DoWork(object sender, DoWorkEventArgs e)
        {
            int volume = (MediaPlayer.Volume * 1000).ToInt();

            for (int i = volume; i >= 0; i--)
            {
                volume--;
                MediaPlayer.Volume = volume.ToSingle() / 1000;
                Thread.Sleep(1);
            }

            SetVolume();/*Se reestablece el volumen*/
            PlayMusicAsync(e.Argument as MusicFadeOutParams);
        }

        #endregion

        #region METHODS

        internal static void LoadContent()
        {
            LoadSoundEffect();
            LoadVoice();
            SetAlbum();
            SetVolume();
        }

        static void SetAlbum()
        {
            Album = new List<string>()
            {
                bgm_01,
                bgm_02,
                bgm_03,
                bgm_04,
                bgm_05,
                bgm_06,
                bgm_07,
                bgm_08,
                bgm_09,
                bgm_10,
                bgm_11,
                bgm_12,
                bgm_13,
                bgm_14,
                bgm_15,
                bgm_16,
                bgm_17,
                bgm_18,
                bgm_19,
                bgm_20
            };
        }

        static void SetVolume()
        {
            SoundEffect.MasterVolume = VOLUME_MASTER;
            MediaPlayer.Volume = VOLUME_MASTER * VOLUME_MUSIC;
        }

        static void LoadSong(string song)
        {
            CurrentSong = GameContent.ContentMusic.Load<Song>($"Sound/Music/{song}");
        }

        static void LoadSoundEffect()
        {
            ButtonClickCloseWindow = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/ButtonClickCloseWindow");
            ButtonClickOpenWindow = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/ButtonClickOpenWindow");
            TabClick = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/TabClick");
            CheckBoxClick = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/BoxClick");
            PanelSlide = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/PanelSlide");
            Padlock = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/Padlock");
            Counter = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/Counter");
            CorrectTile1 = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/CorrectTile1");
            CorrectTile2 = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/CorrectTile2");
            PerkChange = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/PerkChange");
            PerkReveal = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/PerkReveal");
            PerkTimeStop = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/PerkTimeStop");
            Switch = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/Switch");
            WrongTile = GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/WrongTile");
        }

        static void LoadVoice()
        {
            VoiceNewHighScore = GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/NewHighScore");
            VoiceYouLose = GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/YouLose");
            VoiceYouWin = GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/YouWin");
        }

        /// <summary>
        /// Se usa para reproducir efectos de sonido.
        /// </summary>
        /// <param name="soundEffect"></param>
        internal static void PlaySound(this SoundEffect soundEffect)
        {
            if (UserSettingsManager.Sounds)
                soundEffect.Play(VOLUME_FX * VOLUME_MASTER, 0, 0);
        }

        /// <summary>
        /// Establece como la canción que debe sonar a continuación una aleatoria.
        /// Debe ser diferente a la última que ha sonado.
        /// Como las cancinoes son por parejas, es decir, la 0 es igual a 1 con alguna modificación,
        /// se entiende que la anterior puede ser cualquier de las dos, hay que evitar que la siguiente en sonar sea una de esas dos.
        /// </summary>
        static void SetRandomSong()
        {
            while (true)
            {
                int i = Statics.GetRandom(2/*Las dos primeras son las principales y no suenan en la partida*/, Album.Count - 1);

                if (Album[i] == lastSong)/*Si la seleccionada es la misma que la última que ha sonado*/
                    continue;

                if (i.IsOdd() && Album[i - 1] == lastSong)/*Comprobamos si es impar y si la anterior fue la última*/
                    continue;

                if (!i.IsOdd() && Album[i + 1] == lastSong)/*Comprobamos si es par y si la siguiente fue la última*/
                    continue;

                LoadSong(Album[i]);
                return;
            }
        }

        /// <summary>
        /// DEtiene la música haciendo un fade out.
        /// </summary>
        static void StopMusic(MusicFadeOutParams musicFadeOutParams)
        {
            backgroundWorkerFadeOutMusic = new() { WorkerReportsProgress = false, WorkerSupportsCancellation = false };
            backgroundWorkerFadeOutMusic.DoWork += BackgroundWorkerFadeOutMusic_DoWork;
            backgroundWorkerFadeOutMusic.RunWorkerAsync(musicFadeOutParams);
        }

        /// <summary>
        /// Hace sonar una canción aleatoria, y opcionalmente en modo loop.
        /// </summary>
        /// <param name="repeat"></param>
        internal static void PlayMusic(int delay = 0, bool repeat = true)
        {
            if (!UserSettingsManager.Music)
                return;

            MusicFadeOutParams musicFadeOutParams = new(repeat, "", true, false, delay);
            StopMusic(musicFadeOutParams);
        }

        /// <summary>
        /// Hace sonar la canción elegida, y opcionalmente en modo loop.
        /// </summary>
        /// <param name="song"></param>
        /// <param name="delay">tiempo sin sonido entre una canción y otra, en milisegundo</param>
        /// <param name="repeat"></param>
        internal static void PlayMusic(string song, int delay = 0, bool repeat = true)
        {
            if (!UserSettingsManager.Music)
                return;

            MusicFadeOutParams musicFadeOutParams = new(repeat, song, false, false, delay);
            StopMusic(musicFadeOutParams);
        }

        /// <summary>
        /// Detiene la música sin hacer sonar otra después.
        /// </summary>
        internal static void StopMusic()
        {
            MediaPlayer.Stop();
            MusicFadeOutParams musicFadeOutParams = new(false, "", false, true, 0);
            StopMusic(musicFadeOutParams);
        }

        /// <summary>
        /// Reproduce la música depués de haber desvanecido la anterior.
        /// </summary>
        /// <param name="musicFadeOutParams"></param>
        static void PlayMusicAsync(MusicFadeOutParams musicFadeOutParams)
        {
            if (musicFadeOutParams.OnlyStop)
                return;

            MediaPlayer.Stop();
            GameContent.ResetContentMusic();/*Descargamos el content de música*/

            if (musicFadeOutParams.Delay > 0)
                Statics.TimeStop(musicFadeOutParams.Delay);

            if (musicFadeOutParams.Random)
                SetRandomSong();
            else
                LoadSong(musicFadeOutParams.Song);

            MediaPlayer.IsRepeating = musicFadeOutParams.Repeat;
            MediaPlayer.Play(CurrentSong);
        }

        /// <summary>
        /// Reproduce una voz inmediatamente.
        /// </summary>
        /// <param name="voice"></param>
        internal static void PlayVoice(this SoundEffect voice)
        {
            if (UserSettingsManager.Voices)
                voice.Play(VOLUME_FX * VOLUME_MASTER, 0, 0);
        }

        internal static void PlayBallonPop()
        {
            int i = Statics.GetRandom(0, 2);

            if (BallonPop[i] == null)
            {
                SoundEffect sound = i switch
                {
                    0 => GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/BalloonPop1"),
                    1 => GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/BalloonPop2"),
                    2 => GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/BalloonPop3"),
                    _ => GameContent.ContentSound.Load<SoundEffect>("Sound/Effect/UI/BalloonPop1")
                };

                BallonPop[i] = sound;
            }

            BallonPop[i].PlaySound();
        }

        /// <summary>
        /// Reproduce una voz con un mensaje positivo aleatorio.
        /// </summary>
        internal static void PlayRandomVoicePositiveFeedback()
        {
            int i = Statics.GetRandom(0, 12);

            while (LastVoiceSounded == i)/*Para que reproduzca otra diferente al anterior*/
                i = Statics.GetRandom(0, 12);

            if (PositiveFeedbackVoices[i] == null)
            {
                SoundEffect sound = i switch
                {
                    0 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Amazing"),
                    1 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Atomic"),
                    2 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Awesome"),
                    3 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Epic"),
                    4 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Excellent"),
                    5 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Fabulous"),
                    6 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Fantastic"),
                    7 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Impressive"),
                    8 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Incredible"),
                    9 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Marvelous"),
                    10 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Unbelievable"),
                    11 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Unstoppable"),
                    12 => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Wonderful"),
                    _ => GameContent.ContentSound.Load<SoundEffect>("Sound/Voice/Amazing")
                };

                PositiveFeedbackVoices[i] = sound;
            }

            if (DateTime.Now.Ticks > LastVoiceEnded + PositiveFeedbackVoices[i].Duration.Ticks)/*Para que no se solapen los mensajes*/
            {
                LastVoiceEnded = DateTime.Now.Ticks + PositiveFeedbackVoices[i].Duration.Ticks;
                PositiveFeedbackVoices[i].PlayVoice();
                LastVoiceSounded = i;
            }
        }

        #endregion
    }
}