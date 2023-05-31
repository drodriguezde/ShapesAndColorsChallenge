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
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.DataBase.Types;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowChallenges : Window, IDisposable
    {
        #region CONST

        const int TOP = 300;
        const int ITEM_HEIGHT = 240;
        const int OFFSET_Y = 8;
        const int OFFSET_X = 20;
        const int IMAGE_WH = 150;
        const int FLAG_HEIGHT = 100;
        const int BUTTON_WH = 180;
        const int TITLE_HEIGHT = 80;
        const int DESCRIPTION_HEIGHT = 100;

        #endregion

        #region VARS        

        List<Button> buttons = new();

        #endregion

        #region PROPERTIES

        NavigationPanelVertical NavigationPanelVertical { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal WindowChallenges()
            : base(ModalLevel.Window, BaseBounds.Bounds.Redim(), WindowType.Challenges)
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
        ~WindowChallenges()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        /// <summary>
        /// Engancha los eventos de los objetos interactivos.
        /// </summary>
        void SubscribeEvents()
        {
            for (int i = 0; i < buttons.Count; i++)
                buttons[i].OnClick += ButtonPlay_OnClick;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            for (int i = 0; i < buttons.Count; i++)
                buttons[i].OnClick -= ButtonPlay_OnClick;
        }

        void ButtonPlay_OnClick(object sender, OnClickEventArgs e)
        {
            Challenge challenge = (Challenge)(sender as Button).Tag[0];
            OrchestratorManager.StageNumber = challenge.StageNumber;
            OrchestratorManager.LevelNumber = challenge.LevelNumber;
            OrchestratorManager.BackWindow = WindowType;
            OrchestratorManager.GameWindowInvoker = WindowType.Challenges;
            OrchestratorManager.Challenge = challenge;

            if (ControllerSettings.GetShowHowToPlay(OrchestratorManager.GameMode))
                CloseMeAndOpenThis(WindowType.Game);
            else
                CloseMeAndOpenThis(WindowType.HowToPlay, new WindowHowToPlayParams(OrchestratorManager.GameMode, false));
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            SetPanel();
            SetChallenges();
            SubscribeEvents();
            InteractiveObjectManager.Add(NavigationPanelVertical);
            SetTitle();
            SetInfoLabel();
            base.LoadContent();/*Se pone el útimo para que el botón "Atrás" este por encima.*/
        }

        void SetPanel()
        {
            NavigationPanelVertical = new(
                ModalLevel,
                new Rectangle(
                    BaseBounds.Limits.X,
                    TOP,
                    BaseBounds.Bounds.Width,
                    ITEM_HEIGHT * AcheivementsManager.GetResume.Count),
                TOP,
                BaseBounds.Limits.X + BaseBounds.Limits.Height - BaseBounds.Button.Height - 20, this);
        }

        void SetTitle()
        {
            Label labelTitle = new(ModalLevel, BaseBounds.Title, Resource.String.CHALLENGES.GetString(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            Image titleBackground = new(
                ModalLevel,
                new(0, 0, BaseBounds.Bounds.Width, BaseBounds.Title.Bottom),
                TextureManager.Get(new(BaseBounds.Bounds.Width, BaseBounds.Title.Bottom + OFFSET_X), ColorManager.WindowBodyColor, CommonTextureType.Rectangle).Texture, false)
            { AllowFadeInFadeOut = false };
            InteractiveObjectManager.Add(titleBackground, labelTitle);
        }

        void SetChallenges()
        {
            List<Challenge> challenges = ControllerChallenge.Get().Where(t => t.IsActive && t.GameMode == OrchestratorManager.GameMode).ToList();
            List<RankingByGameMode> rankings = ControllerRanking.GetWithPlayers(OrchestratorManager.GameMode);

            for (int i = 0; i < challenges.Count; i++)
                SetChallenge(i, challenges[i], rankings);

            if (!NavigationPanelVertical.NeedMove())/*Esto es necesario para poner arriba los elementos cuando no hay suficientes para llenar el panel*/
                NavigationPanelVertical.MoveToTop();
            else
                NavigationPanelVertical.Move();
        }

        void SetChallenge(int index, Challenge challenge, List<RankingByGameMode> rankings)
        {
            Player player = ControllerPlayer.Get().Single(t => t.PlayerID == challenge.PlayerID);
            Rectangle bounds = new(BaseBounds.Limits.X, TOP + index * ITEM_HEIGHT, BaseBounds.Limits.Width, ITEM_HEIGHT);/*Bounds del item*/
            PanelItem panelItem = new(ModalLevel, bounds, GetButton(), GetFlag(player.Country), GetPlayerName(player.Name), GetDescription(challenge), GetPosition(challenge, rankings));
            SetButtonPlay(panelItem, challenge);
            NavigationPanelVertical.Add(panelItem);
        }

        Button GetButton()
        {
            Rectangle bounds = new(0, OFFSET_Y, BaseBounds.Limits.Width, ITEM_HEIGHT - OFFSET_Y.Double()); /*relativo al item*/
            Button button = new(ModalLevel, bounds) { EnableOnClick = false };
            return button;
        }

        Image GetFlag(string country)
        {
            Rectangle bounds = new(OFFSET_X, ITEM_HEIGHT.Half() - IMAGE_WH.Half() - 10, IMAGE_WH, FLAG_HEIGHT); /*relativo al item*/
            Image image = new(ModalLevel, bounds, TextureManager.Flag(country), true, 0, false);
            return image;
        }

        Label GetPosition(Challenge challenge, List<RankingByGameMode> rankings)
        {
            Rectangle bounds = new(OFFSET_X, ITEM_HEIGHT.Half() + 15, IMAGE_WH, 85); /*relativo al item*/
            Label label = new(ModalLevel, bounds, $"#{rankings.Single(t => t.PlayerID == challenge.PlayerID).Position}", Color.Red, Color.Red, AlignHorizontal.Center);
            return label;
        }

        Label GetPlayerName(string name)
        {
            Rectangle bounds = new(OFFSET_X.Double() + IMAGE_WH, OFFSET_Y.Double(), BaseBounds.Limits.Width - OFFSET_X.Multi(4) - IMAGE_WH - BUTTON_WH, TITLE_HEIGHT); /*relativo al item*/
            Label label = new(ModalLevel, bounds, name, Color.DarkGray, Color.DarkGray, AlignHorizontal.Center);
            return label;
        }

        Label GetDescription(Challenge challenge)
        {
            Rectangle bounds = new(OFFSET_X.Double() + IMAGE_WH, OFFSET_Y.Multi(3) + TITLE_HEIGHT, BaseBounds.Limits.Width - OFFSET_X.Multi(4) - IMAGE_WH - BUTTON_WH, DESCRIPTION_HEIGHT); /*relativo al item*/
            Label label = new(ModalLevel, bounds, ChallengesManager.GetDescription(challenge), Color.DarkGray, Color.DarkGray, AlignHorizontal.Left, 2);
            return label;
        }

        void SetButtonPlay(PanelItem item, Challenge challenge)
        {
            Rectangle bounds = new(item.Bounds.Width - BUTTON_WH - OFFSET_X, ITEM_HEIGHT.Half() - BUTTON_WH.Half(), BUTTON_WH, BUTTON_WH); /*relativo al item*/
            Button button = new(ModalLevel, bounds);
            bounds = new(bounds.X + BUTTON_WH.Half() - IMAGE_WH.Half(), ITEM_HEIGHT.Half() - IMAGE_WH.Half(), IMAGE_WH, IMAGE_WH);
            Image image = new(ModalLevel, bounds, TextureManager.TexturePlay, true) { ColorDarkMode = ColorManager.HardGray, ColorLightMode = ColorManager.HardGray };
            buttons.Add(button);
            item.Add(button, image);
            button.Tag.Add(challenge);
        }

        void SetInfoLabel()
        {
            Rectangle bounds = new(
                BaseBounds.Limits.X + BaseBounds.Button.Width + 50,
                BaseBounds.Limits.Bottom - BaseBounds.Button.Height,
                BaseBounds.Limits.Width - BaseBounds.Limits.X - BaseBounds.Button.Width - 50,
                BaseBounds.Button.Height);
            Label label = new(ModalLevel, bounds, Resource.String.CHALLENGE_INFO.GetString(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Left, 2);
            PostObject = label;/*Se lo enviamos al padre para poder ponerlo encimo del panel inferior que oculta el listado*/
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