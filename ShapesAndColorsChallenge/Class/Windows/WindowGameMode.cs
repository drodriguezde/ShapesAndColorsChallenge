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
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Animated;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Types;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using static ShapesAndColorsChallenge.Class.Management.DataBaseManager;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowGameMode : Window, IDisposable
    {
        #region CONST

        const int SEPARATOR_TITLE_LINE = 5;

        #endregion

        #region VARS

        readonly Rectangle[] buttonBounds = new Rectangle[]
        {
            new(BaseBounds.Limits.X, 320, 465, 340),
            new(BaseBounds.Limits.X, 700, 465, 340),
            new(BaseBounds.Limits.X, 1080, 465, 340),
            new(BaseBounds.Limits.X, 1460, 465, 340),
            new(BaseBounds.Limits.X.Double() + 465, 320, 465, 340),
            new(BaseBounds.Limits.X.Double() + 465, 700, 465, 340),
            new(BaseBounds.Limits.X.Double() + 465, 1080, 465, 340),
            new(BaseBounds.Limits.X.Double() + 465, 1460, 465, 340),
        };

        readonly Rectangle[] imageBounds = new Rectangle[] { /*No necesita RedimMatrix, las Image se redimensionan en su propio Draw*/
            new(BaseBounds.Limits.X + 465.Half() - BaseBounds.ModeImageSize.Width.Half(), 410, BaseBounds.ModeImageSize.Width, BaseBounds.ModeImageSize.Height),
            new(BaseBounds.Limits.X + 465.Half() - BaseBounds.ModeImageSize.Width.Half(), 790, BaseBounds.ModeImageSize.Width, BaseBounds.ModeImageSize.Height),
            new(BaseBounds.Limits.X + 465.Half() - BaseBounds.ModeImageSize.Width.Half(), 1170, BaseBounds.ModeImageSize.Width, BaseBounds.ModeImageSize.Height),
            new(BaseBounds.Limits.X + 465.Half() - BaseBounds.ModeImageSize.Width.Half(), 1550, BaseBounds.ModeImageSize.Width, BaseBounds.ModeImageSize.Height),
            new(BaseBounds.Limits.X.Double() + 465.Double() - 465.Half() - BaseBounds.ModeImageSize.Width.Half(), 410, BaseBounds.ModeImageSize.Width, BaseBounds.ModeImageSize.Height),
            new(BaseBounds.Limits.X.Double() + 465.Double() - 465.Half() - BaseBounds.ModeImageSize.Width.Half(), 790, BaseBounds.ModeImageSize.Width, BaseBounds.ModeImageSize.Height),
            new(BaseBounds.Limits.X.Double() + 465.Double() - 465.Half() - BaseBounds.ModeImageSize.Width.Half(), 1170, BaseBounds.ModeImageSize.Width, BaseBounds.ModeImageSize.Height),
            new(BaseBounds.Limits.X.Double() + 465.Double() - 465.Half() - BaseBounds.ModeImageSize.Width.Half(), 1550, BaseBounds.ModeImageSize.Width, BaseBounds.ModeImageSize.Height),
        };

        readonly Rectangle[] titleBounds = new Rectangle[]
        {
            new(BaseBounds.Limits.X, 310, 465, 80),
            new(BaseBounds.Limits.X, 690, 465, 80),
            new(BaseBounds.Limits.X, 1070, 465, 80),
            new(BaseBounds.Limits.X, 1450, 465, 80),
            new(BaseBounds.Limits.X.Double() + 465, 310, 465, 80),
            new(BaseBounds.Limits.X.Double() + 465, 690, 465, 80),
            new(BaseBounds.Limits.X.Double() + 465, 1070, 465, 80),
            new(BaseBounds.Limits.X.Double() + 465, 1450, 465, 80),
        };

        readonly Rectangle[] imageChallengesBounds = new Rectangle[] {/*No necesita RedimMatrix, las Image se redimensionan en su propio Draw*/
            new(BaseBounds.Limits.X + 20, 410 + SEPARATOR_TITLE_LINE, Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER),
            new(BaseBounds.Limits.X + 20, 790 + SEPARATOR_TITLE_LINE, Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER),
            new(BaseBounds.Limits.X + 20, 1170 + SEPARATOR_TITLE_LINE, Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER),
            new(BaseBounds.Limits.X + 20, 1550 + SEPARATOR_TITLE_LINE, Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER),
            new(BaseBounds.Limits.X.Double() + 20 + 460, 410 + SEPARATOR_TITLE_LINE, Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER),
            new(BaseBounds.Limits.X.Double() + 20 + 460, 790 + SEPARATOR_TITLE_LINE, Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER),
            new(BaseBounds.Limits.X.Double() + 20 + 460, 1170 + SEPARATOR_TITLE_LINE, Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER),
            new(BaseBounds.Limits.X.Double() + 20 + 460, 1550 + SEPARATOR_TITLE_LINE, Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER)
        };

        Rectangle buttonSettingsBounds = new(BaseBounds.Limits.X + BaseBounds.Limits.Width.Half() - BaseBounds.Button.Width.Half(), BaseBounds.Limits.Bottom - BaseBounds.Button.Height, BaseBounds.Button.Width, BaseBounds.Button.Height);
        Rectangle buttonAcheivementsBounds = new(BaseBounds.Limits.Right - BaseBounds.Button.Width, BaseBounds.Limits.Bottom - BaseBounds.Button.Height, BaseBounds.Button.Width, BaseBounds.Button.Height);

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        Button buttonClassic, buttonEndless, buttonMemory, buttonTimeTrial, buttonIncremental, buttonMove, buttonBlink, buttonRotate, buttonClassicPlus, buttonEndlessPlus, buttonMemoryPlus, buttonTimeTrialPlus, buttonIncrementalPlus, buttonMovePlus, buttonBlinkPlus, buttonRotatePlus;
        Image imageClassic, imageEndless, imageMemory, imageTimeTrial, imageIncremental, imageMove, imageBlink, imageRotate, imageClassicPlus, imageEndlessPlus, imageMemoryPlus, imageTimeTrialPlus, imageIncrementalPlus, imageMovePlus, imageBlinkPlus, imageRotatePlus;
        Button buttonSettings;
        Button buttonAcheivements;
        NavigationPanelHorizontal navigationPanelHorizontal;
        Texture2D textureChallenges;/*No se hace Dispose*/
        AnimationShake shakeAnimation;
        readonly List<StarsByGameMode> StarsByGameMode = GetUserStarsGroupByStage();

        #endregion

        #region CONSTRUCTORS

        internal WindowGameMode()
            : base(ModalLevel.Window, BaseBounds.Bounds, WindowType.GameMode)
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
        ~WindowGameMode()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            buttonClassic.OnClick += ButtonMode_OnClick;
            buttonEndless.OnClick += ButtonMode_OnClick;
            buttonMemory.OnClick += ButtonMode_OnClick;
            buttonTimeTrial.OnClick += ButtonMode_OnClick;
            buttonIncremental.OnClick += ButtonMode_OnClick;
            buttonMove.OnClick += ButtonMode_OnClick;
            buttonBlink.OnClick += ButtonMode_OnClick;
            buttonRotate.OnClick += ButtonMode_OnClick;
            buttonClassicPlus.OnClick += ButtonMode_OnClick;
            buttonEndlessPlus.OnClick += ButtonMode_OnClick;
            buttonMemoryPlus.OnClick += ButtonMode_OnClick;
            buttonTimeTrialPlus.OnClick += ButtonMode_OnClick;
            buttonIncrementalPlus.OnClick += ButtonMode_OnClick;
            buttonMovePlus.OnClick += ButtonMode_OnClick;
            buttonBlinkPlus.OnClick += ButtonMode_OnClick;
            buttonRotatePlus.OnClick += ButtonMode_OnClick;
            buttonSettings.OnClick += ButtonSettings_OnClick;
            buttonAcheivements.OnClick += ButtonAcheivements_OnClick;
        }

        void UnsubscribeEvents()
        {
            buttonClassic.OnClick -= ButtonMode_OnClick;
            buttonEndless.OnClick -= ButtonMode_OnClick;
            buttonMemory.OnClick -= ButtonMode_OnClick;
            buttonTimeTrial.OnClick -= ButtonMode_OnClick;
            buttonIncremental.OnClick -= ButtonMode_OnClick;
            buttonMove.OnClick -= ButtonMode_OnClick;
            buttonBlink.OnClick -= ButtonMode_OnClick;
            buttonRotate.OnClick -= ButtonMode_OnClick;
            buttonClassicPlus.OnClick -= ButtonMode_OnClick;
            buttonEndlessPlus.OnClick -= ButtonMode_OnClick;
            buttonMemoryPlus.OnClick -= ButtonMode_OnClick;
            buttonTimeTrialPlus.OnClick -= ButtonMode_OnClick;
            buttonIncrementalPlus.OnClick -= ButtonMode_OnClick;
            buttonMovePlus.OnClick -= ButtonMode_OnClick;
            buttonBlinkPlus.OnClick -= ButtonMode_OnClick;
            buttonRotatePlus.OnClick -= ButtonMode_OnClick;
            buttonSettings.OnClick -= ButtonSettings_OnClick;
            buttonAcheivements.OnClick -= ButtonAcheivements_OnClick;
        }

        void ButtonMode_OnClick(object sender, OnClickEventArgs e)
        {
            GameMode gameMode = (GameMode)(sender as Button).Tag[0];
            CheckLocked(gameMode);
        }

        void ButtonSettings_OnClick(object sender, OnClickEventArgs e)
        {
            /*Pruebas para la ventana de Resultado*/
            //WindowResult windowResult = null;
            //OrchestratorManager.OpenWindowResultMessage(ref windowResult, new(1000, 2, true));
            CloseMeAndOpenThis(WindowType.Settings);
        }

        void ButtonAcheivements_OnClick(object sender, OnClickEventArgs e)
        {
            CloseMeAndOpenThis(WindowType.Acheivements);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetTextures();
            SetPanel();
            SetTitle();
            SetModes();
            SetButtonConfiguration();
            SetButtonAcheivements();
            SubscribeEvents();
            navigationPanelHorizontal.Set();
        }

        void SetTextures()
        {
            textureChallenges = TextureManager.Get(new Size(Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER), Color.Orange, Color.Black, CommonTextureType.Circle).Texture;
        }

        void SetPanel()
        {
            navigationPanelHorizontal = new(ModalLevel, Bounds, buttonBounds[3].Bottom + 20, this);
            InteractiveObjectManager.Add(navigationPanelHorizontal);
        }

        void SetTitle()
        {
            Label labelTitle = new(ModalLevel, BaseBounds.Title, LanguageManager.Get("GAME_MODES"), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelTitle);
        }

        void SetModes()
        {
            SetMode(ref buttonClassic, buttonBounds[0], GameMode.Classic, imageBounds[0], TextureManager.ModeClassic, titleBounds[0], LanguageManager.Get("CLASSIC_MODE"), imageChallengesBounds[0], 1, ref imageClassic);
            SetMode(ref buttonEndless, buttonBounds[1], GameMode.Endless, imageBounds[1], TextureManager.ModeEndless, titleBounds[1], LanguageManager.Get("ENDLESS_MODE"), imageChallengesBounds[1], 1, ref imageEndless);
            SetMode(ref buttonMemory, buttonBounds[2], GameMode.Memory, imageBounds[2], TextureManager.ModeMemory, titleBounds[2], LanguageManager.Get("MEMORY_MODE"), imageChallengesBounds[2], 1, ref imageMemory);
            SetMode(ref buttonTimeTrial, buttonBounds[3], GameMode.TimeTrial, imageBounds[3], TextureManager.ModeTimeTrial, titleBounds[3], LanguageManager.Get("TIMETRIAL_MODE"), imageChallengesBounds[3], 1, ref imageTimeTrial);
            SetMode(ref buttonIncremental, buttonBounds[4], GameMode.Incremental, imageBounds[4], TextureManager.ModeIncremental, titleBounds[4], LanguageManager.Get("INCREMENTAL_MODE"), imageChallengesBounds[4], 1, ref imageIncremental);
            SetMode(ref buttonMove, buttonBounds[5], GameMode.Move, imageBounds[5], TextureManager.ModeMove, titleBounds[5], LanguageManager.Get("MOVE_MODE"), imageChallengesBounds[5], 1, ref imageMove);
            SetMode(ref buttonBlink, buttonBounds[6], GameMode.Blink, imageBounds[6], TextureManager.ModeBlink, titleBounds[6], LanguageManager.Get("BLINK_MODE"), imageChallengesBounds[6], 1, ref imageBlink);
            SetMode(ref buttonRotate, buttonBounds[7], GameMode.Rotate, imageBounds[7], TextureManager.ModeRotate, titleBounds[7], LanguageManager.Get("ROTATE_MODE"), imageChallengesBounds[7], 1, ref imageRotate);

            SetMode(ref buttonClassicPlus, buttonBounds[0], GameMode.ClassicPlus, imageBounds[0], TextureManager.ModeClassic, titleBounds[0], LanguageManager.Get("CLASSIC_MODE_PLUS"), imageChallengesBounds[0], 2, ref imageClassicPlus);
            SetMode(ref buttonEndlessPlus, buttonBounds[1], GameMode.EndlessPlus, imageBounds[1], TextureManager.ModeEndless, titleBounds[1], LanguageManager.Get("ENDLESS_MODE_PLUS"), imageChallengesBounds[1], 2, ref imageEndlessPlus);
            SetMode(ref buttonMemoryPlus, buttonBounds[2], GameMode.MemoryPlus, imageBounds[2], TextureManager.ModeMemory, titleBounds[2], LanguageManager.Get("MEMORY_MODE_PLUS"), imageChallengesBounds[2], 2, ref imageMemoryPlus);
            SetMode(ref buttonTimeTrialPlus, buttonBounds[3], GameMode.TimeTrialPlus, imageBounds[3], TextureManager.ModeTimeTrial, titleBounds[3], LanguageManager.Get("TIMETRIAL_MODE_PLUS"), imageChallengesBounds[3], 2, ref imageTimeTrialPlus);
            SetMode(ref buttonIncrementalPlus, buttonBounds[4], GameMode.IncrementalPlus, imageBounds[4], TextureManager.ModeIncremental, titleBounds[4], LanguageManager.Get("INCREMENTAL_MODE_PLUS"), imageChallengesBounds[4], 2, ref imageIncrementalPlus);
            SetMode(ref buttonMovePlus, buttonBounds[5], GameMode.MovePlus, imageBounds[5], TextureManager.ModeMove, titleBounds[5], LanguageManager.Get("MOVE_MODE_PLUS"), imageChallengesBounds[5], 2, ref imageMovePlus);
            SetMode(ref buttonBlinkPlus, buttonBounds[6], GameMode.BlinkPlus, imageBounds[6], TextureManager.ModeBlink, titleBounds[6], LanguageManager.Get("BLINK_MODE_PLUS"), imageChallengesBounds[6], 2, ref imageBlinkPlus);
            SetMode(ref buttonRotatePlus, buttonBounds[7], GameMode.RotatePlus, imageBounds[7], TextureManager.ModeRotate, titleBounds[7], LanguageManager.Get("ROTATE_MODE_PLUS"), imageChallengesBounds[7], 2, ref imageRotatePlus);
        }

        void SetButtonConfiguration()
        {
            buttonSettings = new(ModalLevel, buttonSettingsBounds);
            Image imageSettings = new(ModalLevel, buttonSettingsBounds, TextureManager.TextureSettingsButton, Color.DarkGray, Color.DarkGray, true, 25);
            InteractiveObjectManager.Add(buttonSettings, imageSettings);
        }

        void SetButtonAcheivements()
        {
            buttonAcheivements = new(ModalLevel, buttonAcheivementsBounds);
            Image imageAcheivements = new(ModalLevel, buttonAcheivementsBounds, TextureManager.TextureAcheivementsButton, Color.DarkGray, Color.DarkGray, true);
            InteractiveObjectManager.Add(buttonAcheivements, imageAcheivements);
            SetAcheivementsBubble();
        }

        void SetAcheivementsBubble()
        {
            int acheivements = AcheivementsManager.GetTotalPendingToClaim();

            if (acheivements == 0)
                return;

            Rectangle infoBubbleBounds = new(buttonAcheivementsBounds.X - Const.BUBBLE_INFO_DIAMETER.Half(), buttonAcheivementsBounds.Top - Const.BUBBLE_INFO_DIAMETER.Half(), Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER);
            Image image = new(ModalLevel, infoBubbleBounds, textureChallenges);
            Label label = new(ModalLevel, infoBubbleBounds, acheivements.ToString(), Color.White, Color.Black, AlignHorizontal.Center);
            InteractiveObjectManager.Add(image, label);
        }

        void SetMode(ref Button button, Rectangle buttonBounds, GameMode gameMode, Rectangle imageBounds, Texture2D textureMode, Rectangle labelModeBounds, string title, Rectangle challengesBounds, int panel, ref Image imageMode)
        {
            bool locked = IsLocked(gameMode);
            button = new(ModalLevel, buttonBounds) { Tag = new() { gameMode }, IsTransparent = true };
            imageMode = new(ModalLevel, imageBounds, locked ? TextureManager.TexturePadLock : textureMode, Color.DarkGray, Color.DarkGray, true, 30);
            Label labelMode = new(ModalLevel, labelModeBounds, title, ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            Line lineMode = new(ModalLevel, new Point(labelModeBounds.Left, labelModeBounds.Bottom + SEPARATOR_TITLE_LINE), new Point(labelModeBounds.Right, labelModeBounds.Bottom + SEPARATOR_TITLE_LINE), ColorManager.HardGray, ColorManager.HardGray, 1);
            (Image imageChallenges, Label labelChallenges) = SetChallengesBubble(locked, gameMode, challengesBounds);
            InteractiveObjectManager.Add(button, imageMode, labelMode, lineMode, imageChallenges, imageChallenges, labelChallenges);
            navigationPanelHorizontal.Add(panel, button, imageMode, labelMode, lineMode, imageChallenges, imageChallenges, labelChallenges);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/

            if (!locked)
                SetStarsByGameMode(gameMode, panel, imageBounds);
        }

        (Image, Label) SetChallengesBubble(bool locked, GameMode gameMode, Rectangle challengesBounds)
        {
            int challenges = ControllerChallenge.Get().Count(t => t.IsActive && t.GameMode == gameMode);
            Image image = new(ModalLevel, challengesBounds, textureChallenges) { VisibleForNavigationPanel = !locked && challenges.NotIsZero(), Visible = !locked && challenges.NotIsZero() };
            Label label = new(ModalLevel, challengesBounds, challenges.ToString(), Color.White, Color.Black, AlignHorizontal.Center) { VisibleForNavigationPanel = !locked && challenges.NotIsZero(), Visible = !locked && challenges.NotIsZero() };
            return (image, label);
        }

        bool IsLocked(GameMode gameMode)
        {
            if (gameMode.IsIn(GameMode.Classic, GameMode.Rotate, GameMode.TimeTrial, GameMode.Blink, GameMode.Endless, GameMode.Incremental, GameMode.Memory, GameMode.Move))
                return false;

            int currentStars = StarsByGameMode.Single(t => t.GameMode == gameMode).Stars;
            return currentStars < GameData.StarsToUnlockMode;
        }

        void SetStarsByGameMode(GameMode gameMode, int panelIndex, Rectangle imageBounds)
        {
            int currentStars = StarsByGameMode.Single(t => t.GameMode == gameMode).Stars;
            Rectangle bounds = new(imageBounds.Right - 40, imageBounds.Bottom - 85, 70, 70);
            Image imageStar = new(ModalLevel, new Rectangle(bounds.X - 15 * currentStars.ToString().Length, bounds.Y, bounds.Width, bounds.Height), TextureManager.TextureStar, true);
            Label labelStars = new(ModalLevel, new Rectangle(bounds.Right - 40, bounds.Bottom - 75, 90, 90), currentStars.ToString(), Color.GreenYellow, Color.GreenYellow, AlignHorizontal.Right);
            InteractiveObjectManager.Add(imageStar, labelStars);
            navigationPanelHorizontal.Add(panelIndex, imageStar, labelStars);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        /// <summary>
        /// Comprueba si un modo de juego podría estar bloqueado.
        /// </summary>
        void CheckLocked(GameMode gameMode)
        {
            if (IsLocked(gameMode))/*Está bloqueado*/
            {
                shakeAnimation?.Stop();
                shakeAnimation = new AnimationShake(GetImagePadlockByGameMode(gameMode), 700);
                shakeAnimation.Start();
                Toast.Start(LanguageManager.Get(GetLockedDescription(gameMode), GameData.StarsToUnlockMode));
                SoundManager.Padlock.PlaySound();/*En estos botones hay que hacer sonar el sonido dependiendo de si se puede abrir el modo de juego o no*/
                Statics.Vibrate(VibrationDuration.Long);
            }
            else/*No está bloqueado*/
            {
                OrchestratorManager.GameMode = gameMode;
                CloseMeAndOpenThis(WindowType.Stage);
            }
        }

        static string GetLockedDescription(GameMode gameMode)
        {
            return gameMode switch
            {
                GameMode.ClassicPlus => "CLASSIC_MODE_PLUS_LOCKED",
                GameMode.EndlessPlus => "ENDLESS_MODE_PLUS_LOCKED",
                GameMode.MemoryPlus => "MEMORY_MODE_PLUS_LOCKED",
                GameMode.TimeTrialPlus => "TIMETRIAL_MODE_PLUS_LOCKED",
                GameMode.IncrementalPlus => "INCREMENTAL_MODE_PLUS_LOCKED",
                GameMode.MovePlus => "MOVE_MODE_PLUS_LOCKED",
                GameMode.BlinkPlus => "BLINK_MODE_PLUS_LOCKED",
                GameMode.RotatePlus => "ROTATE_MODE_PLUS_LOCKED",
                _ => string.Empty,
            };
        }

        Image GetImagePadlockByGameMode(GameMode gameMode)
        {
            return gameMode switch
            {
                GameMode.ClassicPlus => imageClassicPlus,
                GameMode.EndlessPlus => imageEndlessPlus,
                GameMode.MemoryPlus => imageMemoryPlus,
                GameMode.TimeTrialPlus => imageTimeTrialPlus,
                GameMode.IncrementalPlus => imageIncrementalPlus,
                GameMode.MovePlus => imageMovePlus,
                GameMode.BlinkPlus => imageBlinkPlus,
                GameMode.RotatePlus => imageRotatePlus,
                _ => null,
            };
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            shakeAnimation?.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}