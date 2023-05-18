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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Interfaces;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;

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
        Button buttonPlayerName;
        Image buttonPlayerNationality;
        CheckBox checkBoxMusic;
        CheckBox checkBoxSound;
        CheckBox checkBoxVoice;
        CheckBox checkBoxVibration;
        CheckBox checkBoxDarkMode;
        CheckBox checkBoxAlwaysDarkMode;
        Image buttonLanguage;
        Button buttonResetProgress;

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

        Rectangle LabelPlayerNameBounds { get; set; } = new Rectangle(100, 400, 500, 100).Redim();
        Rectangle ButtonPlayerNameBounds { get; set; } = new Rectangle(650, 390, 120, 120).Redim();
        Rectangle ButtonPlayerNationalityBounds { get; set; } = new Rectangle(800, 390, 180, 120).Redim();
        Rectangle CheckBoxMusicBounds { get; set; } = new Rectangle(100.RedimX(), 600.RedimY(), BaseBounds.CheckBox.Width.RedimX(), BaseBounds.CheckBox.Height.RedimX());
        Rectangle CheckBoxSoundBounds { get; set; } = new Rectangle(100.RedimX(), 790.RedimY(), BaseBounds.CheckBox.Width.RedimX(), BaseBounds.CheckBox.Height.RedimX());
        Rectangle CheckBoxVoiceBounds { get; set; } = new Rectangle(100.RedimX(), 980.RedimY(), BaseBounds.CheckBox.Width.RedimX(), BaseBounds.CheckBox.Height.RedimX());
        Rectangle CheckBoxVibrationBounds { get; set; } = new Rectangle(100.RedimX(), 1170.RedimY(), BaseBounds.CheckBox.Width.RedimX(), BaseBounds.CheckBox.Height.RedimX());
        Rectangle CheckBoxDarkModeBounds { get; set; } = new Rectangle(100.RedimX(), 1360.RedimY(), BaseBounds.CheckBox.Width.RedimX(), BaseBounds.CheckBox.Height.RedimX());
        Rectangle CheckBoxAlwaysDarkModeBounds { get; set; } = new Rectangle(100.RedimX(), 1550.RedimY(), BaseBounds.CheckBox.Width.RedimX(), BaseBounds.CheckBox.Height.RedimX());
        Rectangle ButtonLanguageBounds { get; set; } = new Rectangle(100, 1740, 300, 200).Redim();
        Rectangle ButtonResetProgressBounds { get; set; } = new Rectangle(BaseBounds.Limits.X + 250, BaseBounds.Limits.Bottom - BaseBounds.Button.Height, BaseBounds.Limits.Width - 250, BaseBounds.Button.Height).Redim();
        Rectangle LabelResetProgressBounds { get; set; } = new Rectangle(BaseBounds.Limits.X + 260, BaseBounds.Limits.Bottom - BaseBounds.Button.Height, BaseBounds.Limits.Width - 270, BaseBounds.Button.Height - 20).Redim();

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
            buttonLanguage.OnClick += ButtonLanguage_OnClick;
            buttonResetProgress.OnClick += ButtonResetProgress_OnClick;
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
            buttonLanguage.OnClick -= ButtonLanguage_OnClick;
            buttonResetProgress.OnClick -= ButtonResetProgress_OnClick;
        }

        void CheckBoxNotifications_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.Notifications = (sender as CheckBox).Checked;
        }

        void CheckBoxMusic_OnCheckedChange(object sender, EventArgs e)
        {
            UserSettingsManager.Music = (sender as CheckBox).Checked;

            if (UserSettingsManager.Music)/*Aquí ya se ha realizado el cambio*/
                SoundManager.PlayMusic(SoundManager.bgm_01);
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
            labelPlayerName = new(ModalLevel, LabelPlayerNameBounds, UserSettingsManager.PlayerName, ColorManager.WindowBodyColorDarkMode, ColorManager.WindowBodyColorLightMode);
            InteractiveObjectManager.Add(labelPlayerName);
        }

        void SetButtonPlayerName()
        {
            buttonPlayerName = new(ModalLevel, ButtonPlayerNameBounds);
            Image editPlayerName = new(ModalLevel, ButtonPlayerNameBounds, TextureManager.TextureEditIcon, ColorManager.WindowBodyColorDarkMode, ColorManager.WindowBodyColorLightMode, true, 25);
            InteractiveObjectManager.Add(buttonPlayerName, editPlayerName);
        }

        void SetButtonNationality()
        {
            buttonPlayerNationality = new(ModalLevel, ButtonPlayerNationalityBounds, TextureManager.Flag(UserSettingsManager.PlayerCountryCode), true, 0, false);
            int border = Const.BUTTON_BORDER.RedimX();
            Rectangle backImageBounds = new(ButtonPlayerNationalityBounds.X - border.Half(), ButtonPlayerNationalityBounds.Y - border.Half(), ButtonPlayerNationalityBounds.Width + border, ButtonPlayerNationalityBounds.Height + border);
            Image backImage = new(ModalLevel, backImageBounds, TextureManager.Get(backImageBounds.ToSize(), ColorManager.HardGray, CommonTextureType.Rectangle).Texture);
            InteractiveObjectManager.Add(backImage, buttonPlayerNationality);
        }

        void SetCheckBoxMusic()
        {
            checkBoxMusic = new(ModalLevel, CheckBoxMusicBounds, UserSettingsManager.Music);
            InteractiveObjectManager.Add(checkBoxMusic, GetLabel(CheckBoxMusicBounds, Resource.String.MUSIC));
        }

        void SetCheckBoxSound()
        {
            checkBoxSound = new(ModalLevel, CheckBoxSoundBounds, UserSettingsManager.Sounds);
            InteractiveObjectManager.Add(checkBoxSound, GetLabel(CheckBoxSoundBounds, Resource.String.SOUND));
        }

        void SetCheckBoxVoice()
        {
            checkBoxVoice = new(ModalLevel, CheckBoxVoiceBounds, UserSettingsManager.Voices);
            InteractiveObjectManager.Add(checkBoxVoice, GetLabel(CheckBoxVoiceBounds, Resource.String.VOICES));
        }

        void SetCheckBoxVibration()
        {
            checkBoxVibration = new(ModalLevel, CheckBoxVibrationBounds, UserSettingsManager.Vibration);
            InteractiveObjectManager.Add(checkBoxVibration, GetLabel(CheckBoxVibrationBounds, Resource.String.VIBRATION));
        }

        void SetCheckBoxDarkMode()
        {
            checkBoxDarkMode = new(ModalLevel, CheckBoxDarkModeBounds, UserSettingsManager.DarkMode);
            InteractiveObjectManager.Add(checkBoxDarkMode, GetLabel(CheckBoxDarkModeBounds, Resource.String.DARK_MODE));
        }

        void SetCheckBoxAlwaysDarkMode()
        {
            checkBoxAlwaysDarkMode = new(ModalLevel, CheckBoxAlwaysDarkModeBounds, UserSettingsManager.AlwaysDarkMode);
            InteractiveObjectManager.Add(checkBoxAlwaysDarkMode, GetLabel(CheckBoxAlwaysDarkModeBounds, Resource.String.ALWAYS_DARK_MODE));
        }

        void SetButtonLanguage()
        {
            buttonLanguage = new(ModalLevel, ButtonLanguageBounds, TextureManager.Flag(UserSettingsManager.CountryCode));
            int border = Const.BUTTON_BORDER.RedimX();
            Rectangle backImageBounds = new(ButtonLanguageBounds.X - border.Half(), ButtonLanguageBounds.Y - border.Half(), ButtonLanguageBounds.Width + border, ButtonLanguageBounds.Height + border);
            Image backImage = new(ModalLevel, backImageBounds, TextureManager.Get(backImageBounds.ToSize(), ColorManager.HardGray, CommonTextureType.Rectangle).Texture);
            InteractiveObjectManager.Add(backImage, buttonLanguage, GetLabel(ButtonLanguageBounds, Resource.String.LANGUAGE));
        }

        Label GetLabel(Rectangle objectBounds, int text)
        {
            return new(
                        ModalLevel,
                        new Rectangle(
                            objectBounds.Right + 20.RedimX(),
                            objectBounds.Top + objectBounds.Height.Half() - 40.RedimY(),
                            800.RedimY(),
                            80.RedimX()),
                        text.GetString(),
                        ColorManager.HardGray,
                        ColorManager.HardGray);
        }

        void SetButtonResetProgress()
        {
            buttonResetProgress = new(ModalLevel, ButtonResetProgressBounds) { ColorLightMode = Color.OrangeRed };
            Label labelReset = new(ModalLevel, LabelResetProgressBounds, Resource.String.RESET_PROGRESS.GetString(), Color.Red, Color.Red, AlignHorizontal.Center);
            InteractiveObjectManager.Add(buttonResetProgress, labelReset);
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