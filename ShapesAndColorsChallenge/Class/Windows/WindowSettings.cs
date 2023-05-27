﻿/***********************************************************************
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Interfaces;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Web;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowSettings : Window, IMessage, IDisposable
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS

        Label labelTitle;
        WindowMessageBox windowMessageBox;
        Label labelPlayerName;
        Label labelID;
        Button buttonPlayerName;
        Image buttonPlayerNationality;
        CheckBox checkBoxMusic;
        CheckBox checkBoxSound;
        CheckBox checkBoxVoice;
        CheckBox checkBoxVibration;
        CheckBox checkBoxDarkMode;
        CheckBox checkBoxAlwaysDarkMode;
        Image imageLanguage;
        Button buttonResetProgress;
        Button buttonUpload;
        Button buttonDownload;

        Rectangle labelPlayerNameBounds = new(BaseBounds.Limits.X, 400, 500, 100);
        Rectangle buttonPlayerNameBounds = new(650, 390, 120, 120);
        Rectangle buttonPlayerNationalityBounds = new(800, 390, 180, 120);
        Rectangle checkBoxMusicBounds = new(BaseBounds.Limits.X, 600, BaseBounds.CheckBox.Width, BaseBounds.CheckBox.Height);
        Rectangle checkBoxSoundBounds = new(BaseBounds.Limits.X, 760, BaseBounds.CheckBox.Width, BaseBounds.CheckBox.Height);
        Rectangle checkBoxVoiceBounds = new(BaseBounds.Limits.X, 920, BaseBounds.CheckBox.Width, BaseBounds.CheckBox.Height);
        Rectangle checkBoxVibrationBounds = new(BaseBounds.Limits.X, 1080, BaseBounds.CheckBox.Width, BaseBounds.CheckBox.Height);
        Rectangle checkBoxDarkModeBounds = new(BaseBounds.Limits.X, 1240, BaseBounds.CheckBox.Width, BaseBounds.CheckBox.Height);
        Rectangle checkBoxAlwaysDarkModeBounds = new(BaseBounds.Limits.X, 1400, BaseBounds.CheckBox.Width, BaseBounds.CheckBox.Height);
        Rectangle buttonLanguageBounds = new(BaseBounds.Limits.X, 1560, 180, 120);
        Rectangle buttonResetProgressBounds = new(BaseBounds.Limits.X + 250, BaseBounds.Limits.Bottom - BaseBounds.Button.Height, BaseBounds.Limits.Width - 250, BaseBounds.Button.Height);
        Rectangle labelResetProgressBounds = new(BaseBounds.Limits.X + 260, BaseBounds.Limits.Bottom - BaseBounds.Button.Height, BaseBounds.Limits.Width - 270, BaseBounds.Button.Height - 20);
        Rectangle buttonUploadBounds = new(BaseBounds.Limits.Right - 50 - BaseBounds.Button.Width.Double(), BaseBounds.Limits.Bottom - BaseBounds.Button.Height.Double() - 50, BaseBounds.Button.Width, BaseBounds.Button.Height);
        Rectangle buttonDownloaBounds = new(BaseBounds.Limits.Right - BaseBounds.Button.Width, BaseBounds.Limits.Bottom - BaseBounds.Button.Height.Double() - 50, BaseBounds.Button.Width, BaseBounds.Button.Height);
        Rectangle labelIdBounds = new(BaseBounds.Limits.X, BaseBounds.Limits.Bottom - BaseBounds.Button.Height - 200, BaseBounds.Limits.Width - 20 - BaseBounds.Button.Width.Double(), 100);

        #endregion

        #region PROPERTIES

        public Window WindowMessage 
        {
            get { return windowMessageBox; }
            set { windowMessageBox = (WindowMessageBox)value; }
        }

        /// <summary>
        /// Indica si se ha aceptado el reset del progreso.
        /// </summary>
        bool ShowResetConfirmation { get; set; } = false;

        #endregion

        #region CONSTRUCTORS

        internal WindowSettings()
            : base(ModalLevel.Window, BaseBounds.Bounds.Redim(), WindowType.Settings)
        {

        }

        #endregion

        #region DESTRUCTOR

        /// <summary>
        /// Variable que indica si se ha destruido el objeto.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Libera todos los recursos.
        /// </summary>
        internal new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos no administrados que utiliza, y libera los recursos administrados de forma opcional.
        /// </summary>
        /// <param name="disposing">True si se quiere liberar los recursos administrados.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            /*Objetos administrados aquí*/
            if (disposing)
            {
                UnsubscribeEvents();
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~WindowSettings()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            checkBoxMusic.OnCheckedChange += CheckBoxMusic_OnCheckedChange;
            checkBoxSound.OnCheckedChange += CheckBoxSound_OnCheckedChange;
            checkBoxVoice.OnCheckedChange += CheckBoxVoice_OnCheckedChange;
            checkBoxVibration.OnCheckedChange += CheckBoxVibration_OnCheckedChange;
            checkBoxDarkMode.OnCheckedChange += CheckBoxDarkMode_OnCheckedChange;
            checkBoxAlwaysDarkMode.OnCheckedChange += CheckBoxAlwaysDarkMode_OnCheckedChange;

            buttonPlayerName.OnClick += ButtonPlayerName_OnClickAsync;
            buttonPlayerNationality.OnClick += ButtonPlayerNationality_OnClick;
            imageLanguage.OnClick += ButtonLanguage_OnClick;
            buttonResetProgress.OnClick += ButtonResetProgress_OnClick;
            buttonUpload.OnClick += ButtonUpload_OnClick;
            buttonDownload.OnClick += ButtonDownload_OnClick;
        }

        void UnsubscribeEvents()
        {
            checkBoxMusic.OnCheckedChange -= CheckBoxMusic_OnCheckedChange;
            checkBoxSound.OnCheckedChange -= CheckBoxSound_OnCheckedChange;
            checkBoxVoice.OnCheckedChange += CheckBoxVoice_OnCheckedChange;
            checkBoxVibration.OnCheckedChange -= CheckBoxVibration_OnCheckedChange;
            checkBoxDarkMode.OnCheckedChange -= CheckBoxDarkMode_OnCheckedChange;
            checkBoxAlwaysDarkMode.OnCheckedChange -= CheckBoxAlwaysDarkMode_OnCheckedChange;

            buttonPlayerName.OnClick -= ButtonPlayerName_OnClickAsync;
            buttonPlayerNationality.OnClick -= ButtonPlayerNationality_OnClick;
            imageLanguage.OnClick -= ButtonLanguage_OnClick;
            buttonResetProgress.OnClick -= ButtonResetProgress_OnClick;
            buttonUpload.OnClick -= ButtonUpload_OnClick;
            buttonDownload.OnClick -= ButtonDownload_OnClick;
        }

        void CheckBoxNotifications_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.Notifications = (sender as CheckBox).Checked;
        }

        void CheckBoxMusic_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.Music = (sender as CheckBox).Checked;

            if (UserSettingsManager.Music)/*Aquí ya se ha realizado el cambio*/
                SoundManager.PlayMusic(Statics.GetRandom(1, 10) > 5 ? SoundManager.bgm_01 : SoundManager.bgm_02);
            else
                SoundManager.StopMusic();
        }

        void CheckBoxSound_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.Sounds = (sender as CheckBox).Checked;
        }

        private void CheckBoxVoice_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.Voices = (sender as CheckBox).Checked;
        }

        void CheckBoxVibration_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.Vibration = (sender as CheckBox).Checked;
        }

        void CheckBoxDarkMode_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.DarkMode = (sender as CheckBox).Checked;
        }

        void CheckBoxAlwaysDarkMode_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.AlwaysDarkMode = (sender as CheckBox).Checked;
        }

        void ButtonPlayerNationality_OnClick(object sender, OnClickEventArgs e)
        {
            CloseMeAndOpenThis(WindowType.Nationality);
        }

        void ButtonLanguage_OnClick(object sender, OnClickEventArgs e)
        {
            CloseMeAndOpenThis(WindowType.Language);
        }

        async void ButtonPlayerName_OnClickAsync(object sender, OnClickEventArgs e)
        {
            var playerName = await KeyboardInput.Show(Resource.String.PLAYER_NAME.GetString(), Resource.String.TYPE_NAME.GetString(), UserSettingsManager.PlayerName);

            if (playerName == null)
                return;
            else if (string.IsNullOrEmpty(playerName.Trim()))
                UserSettingsManager.PlayerName = Const.PLAYER_NAME;
            else
            {
                if (playerName.Trim().Length > 20)
                    UserSettingsManager.PlayerName = playerName.Trim()[..20];
                else
                    UserSettingsManager.PlayerName = playerName.Trim();

                /*Tambien hay que actualizar el nombre en la lista de jugadores*/
                Player player = ControllerPlayer.Get().Single(t => t.IsPlayer);
                player.Name = UserSettingsManager.PlayerName;
                ControllerPlayer.Update(player);
                RestOrchestrator.TryToUpdatePlayerInfo();
            }

            InteractiveObjectManager.Remove(labelPlayerName.ID);
            SetLabelPlayerName();
        }

        void ButtonResetProgress_OnClick(object sender, OnClickEventArgs e)
        {
            OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.RESET_PROGRESS_QUESTION.GetString(), MessageBoxButton.AcceptCancel, 2));
            windowMessageBox.OnCancel += WindowConfirmReset_OnCancel;
            windowMessageBox.OnAccept += WindowConfirmReset_OnAccept;
        }

        async void ButtonDownload_OnClick(object sender, OnClickEventArgs e)
        {
            var id = await KeyboardInput.Show(Resource.String.DOWNLOAD_PROGRESS.GetString(), Resource.String.WRITE_ID.GetString(), string.Empty);

            if (id == null || string.IsNullOrEmpty(id.Trim()))
            {
                OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.INVALID_ID.GetString(), MessageBoxButton.Accept, 1));
                windowMessageBox.OnAccept += UploadingDownloadingProgressResult_OnAccept;
            }
            else
            {
                OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.GETTING_DATA.GetString(), MessageBoxButton.None, 1));
                RestOrchestrator.TryToDownloadProgress(id, this);
            }
        }

        void ButtonUpload_OnClick(object sender, OnClickEventArgs e)
        {
            OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.SAVE_PROGRESS.GetString(), MessageBoxButton.AcceptCancel, 1));
            windowMessageBox.OnCancel += WindowSaveProgress_OnCancel;
            windowMessageBox.OnAccept += WindowSaveProgress_OnAccept;
        }

        void WindowSaveProgress_OnCancel(object sender, EventArgs e)
        {
            windowMessageBox.OnCancel -= WindowSaveProgress_OnCancel;
            windowMessageBox.OnAccept -= WindowSaveProgress_OnAccept;
            WindowManager.Remove(windowMessageBox);
        }

        void WindowSaveProgress_OnAccept(object sender, EventArgs e)
        {
            windowMessageBox.OnCancel -= WindowSaveProgress_OnCancel;
            windowMessageBox.OnAccept -= WindowSaveProgress_OnAccept;
            WindowManager.Remove(windowMessageBox);
            OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.UPLOADING_PROGRESS.GetString(), MessageBoxButton.None, 1));
            RestOrchestrator.TryToUploadProgress(this);
        }

        void WindowConfirmReset_OnAccept(object sender, EventArgs e)
        {
            ShowResetConfirmation = true;
            DataBaseManager.ResetProgress();
            OrchestratorManager.CloseMessageBox(windowMessageBox, MessageBoxInvoker.IMessage);/*Cuando acabe la transición se llamará a CloseMessageBox()*/
        }

        void WindowConfirmReset_OnCancel(object sender, EventArgs e)
        {
            ShowResetConfirmation = false;
            OrchestratorManager.CloseMessageBox(windowMessageBox, MessageBoxInvoker.IMessage);/*Cuando acabe la transición se llamará a CloseMessageBox()*/
        }

        void WindowResult_OnAccept(object sender, EventArgs e)
        {
            ShowResetConfirmation = false;
            OrchestratorManager.CloseMessageBox(windowMessageBox, MessageBoxInvoker.IMessage);/*Cuando acabe la transición se llamará a CloseMessageBox()*/
        }

        void UploadingDownloadingProgressResult_OnAccept(object sender, EventArgs e)
        {
            windowMessageBox.OnAccept -= UploadingDownloadingProgressResult_OnAccept;
            WindowManager.Remove(windowMessageBox);            
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetTitle();
            SetPlayerData();
            SetCheckBoxMusic();
            SetCheckBoxSound();
            SetCheckBoxVoice();
            SetCheckBoxVibration();
            SetCheckBoxDarkMode();
            SetCheckBoxAlwaysDarkMode();
            SetButtonLanguage();
            SetButtonResetProgress();
            SetLabelID();
            SetButtonUpload();
            SetButtonDownload();
            SubscribeEvents();/*Debe estar al final para evitar que salten los eventos en las inicializaciones de los objetos*/
        }

        void SetTitle()
        {
            labelTitle = new(ModalLevel, BaseBounds.Title, Resource.String.SETTINGS.GetString(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelTitle);
        }

        void SetPlayerData()
        {
            SetLabelPlayerName();
            SetButtonPlayerName();
            SetButtonNationality();
        }

        void SetLabelPlayerName()
        {
            labelPlayerName = new(ModalLevel, labelPlayerNameBounds, UserSettingsManager.PlayerName, ColorManager.WindowBodyColorDarkMode, ColorManager.WindowBodyColorLightMode);
            InteractiveObjectManager.Add(labelPlayerName);
        }

        void SetButtonPlayerName()
        {
            buttonPlayerName = new(ModalLevel, buttonPlayerNameBounds);
            Image editPlayerName = new(ModalLevel, buttonPlayerNameBounds, TextureManager.TextureEditIcon, ColorManager.WindowBodyColorDarkMode, ColorManager.WindowBodyColorLightMode, true, 25);
            InteractiveObjectManager.Add(buttonPlayerName, editPlayerName);
        }

        void SetButtonNationality()
        {
            buttonPlayerNationality = new(ModalLevel, buttonPlayerNationalityBounds, TextureManager.Flag(UserSettingsManager.PlayerCountryCode), true, 0, false);
            int border = Const.BUTTON_BORDER.RedimX();
            Rectangle backImageBounds = new(buttonPlayerNationalityBounds.X - border.Half(), buttonPlayerNationalityBounds.Y - border.Half(), buttonPlayerNationalityBounds.Width + border, buttonPlayerNationalityBounds.Height + border);
            Image backImage = new(ModalLevel, backImageBounds, TextureManager.Get(backImageBounds.ToSize(), ColorManager.HardGray, CommonTextureType.Rectangle).Texture);
            InteractiveObjectManager.Add(backImage, buttonPlayerNationality);
        }

        void SetCheckBoxMusic()
        {
            checkBoxMusic = new(ModalLevel, checkBoxMusicBounds, UserSettingsManager.Music);
            InteractiveObjectManager.Add(checkBoxMusic, checkBoxMusic.GetLabel(Resource.String.MUSIC));
        }

        void SetCheckBoxSound()
        {
            checkBoxSound = new(ModalLevel, checkBoxSoundBounds, UserSettingsManager.Sounds);
            InteractiveObjectManager.Add(checkBoxSound, checkBoxSound.GetLabel(Resource.String.SOUND));
        }

        void SetCheckBoxVoice()
        {
            checkBoxVoice = new(ModalLevel, checkBoxVoiceBounds, UserSettingsManager.Voices);
            InteractiveObjectManager.Add(checkBoxVoice, checkBoxVoice.GetLabel(Resource.String.VOICES));
        }

        void SetCheckBoxVibration()
        {
            checkBoxVibration = new(ModalLevel, checkBoxVibrationBounds, UserSettingsManager.Vibration);
            InteractiveObjectManager.Add(checkBoxVibration, checkBoxVibration.GetLabel(Resource.String.VIBRATION));
        }

        void SetCheckBoxDarkMode()
        {
            checkBoxDarkMode = new(ModalLevel, checkBoxDarkModeBounds, UserSettingsManager.DarkMode);
            InteractiveObjectManager.Add(checkBoxDarkMode, checkBoxDarkMode.GetLabel(Resource.String.DARK_MODE));
        }

        void SetCheckBoxAlwaysDarkMode()
        {
            checkBoxAlwaysDarkMode = new(ModalLevel, checkBoxAlwaysDarkModeBounds, UserSettingsManager.AlwaysDarkMode);
            InteractiveObjectManager.Add(checkBoxAlwaysDarkMode, checkBoxAlwaysDarkMode.GetLabel(Resource.String.ALWAYS_DARK_MODE));
        }

        void SetButtonLanguage()
        {
            imageLanguage = new(ModalLevel, buttonLanguageBounds, TextureManager.Flag(UserSettingsManager.CountryCode), true, 0 , false);
            int border = Const.BUTTON_BORDER.RedimX();
            Rectangle backImageBounds = new(buttonLanguageBounds.X - border.Half(), buttonLanguageBounds.Y - border.Half(), buttonLanguageBounds.Width + border, buttonLanguageBounds.Height + border);
            Image backImage = new(ModalLevel, backImageBounds, TextureManager.Get(backImageBounds.ToSize(), ColorManager.HardGray, CommonTextureType.Rectangle).Texture, true, 0, false);
            InteractiveObjectManager.Add(backImage, imageLanguage, imageLanguage.GetLabel(Resource.String.LANGUAGE));
        }

        void SetButtonResetProgress()
        {
            buttonResetProgress = new(ModalLevel, buttonResetProgressBounds) { ColorLightMode = Color.OrangeRed };
            Label labelReset = new(ModalLevel, labelResetProgressBounds, Resource.String.RESET_PROGRESS.GetString(), Color.Red, Color.Red, AlignHorizontal.Center);
            InteractiveObjectManager.Add(buttonResetProgress, labelReset);
        }

        void SetLabelID()
        {
            labelID = new(ModalLevel, labelIdBounds, $"ID: {ControllerSettings.Get().PlayerToken}", ColorManager.WindowBodyColorDarkMode, ColorManager.WindowBodyColorLightMode);
            InteractiveObjectManager.Add(labelID);
        }

        void SetButtonUpload()
        {
            buttonUpload = new(ModalLevel, buttonUploadBounds);
            Image image = new(ModalLevel, buttonUploadBounds, TextureManager.TextureButtonUpload, Color.DarkGray, Color.DarkGray, true, 20, true);
            InteractiveObjectManager.Add(buttonUpload, image);
        }

        void SetButtonDownload()
        {
            buttonDownload = new(ModalLevel, buttonDownloaBounds);
            Image image = new(ModalLevel, buttonDownloaBounds, TextureManager.TextureButtonDownload, Color.DarkGray, Color.DarkGray, true, 20, true);
            InteractiveObjectManager.Add(buttonDownload, image);
        }

        public void CloseMessageBox()
        {
            if (WindowManager.GetTopModalLevel == ModalLevel.MessageBox)
            {
                windowMessageBox.OnCancel -= WindowConfirmReset_OnCancel;
                windowMessageBox.OnAccept -= WindowConfirmReset_OnAccept;
                WindowManager.Remove(windowMessageBox.ID);
                windowMessageBox = null;

                if (ShowResetConfirmation)
                {
                    OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.PROGRESS_RESET.GetString(), MessageBoxButton.Accept, 1));
                    windowMessageBox.OnAccept += WindowResult_OnAccept;
                }
            }
        }

        public void SetUploadResult(bool result)
        {
            WindowManager.Remove(windowMessageBox);

            if (result)
            {
                OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.UPLOADING_PROGRESS_OK.GetString(), MessageBoxButton.Accept, 1));
                windowMessageBox.OnAccept += UploadingDownloadingProgressResult_OnAccept;
            }
            else
            {
                OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.GENERIC_ERROR.GetString(), MessageBoxButton.Accept, 1));
                windowMessageBox.OnAccept += UploadingDownloadingProgressResult_OnAccept;
            }
        }

        public void SetDownloadResult(UserProgress userProgress)
        {
            WindowManager.Remove(windowMessageBox);

            if (userProgress != null)
            {
                /*TODO*/
            }
            else
            {
                OrchestratorManager.OpenMessageBox(ref windowMessageBox, new(Resource.String.GENERIC_ERROR.GetString(), MessageBoxButton.Accept, 1));
                windowMessageBox.OnAccept += UploadingDownloadingProgressResult_OnAccept;
            }
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}