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
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowAcheivements : Window, IDisposable
    {
        #region CONST

        const int TOP = 300;
        const int ITEM_HEIGHT = 240;
        const int OFFSET_Y = 8;
        const int OFFSET_X = 20;
        const int IMAGE_WH = 150;
        const int BUTTON_WH = 180;
        const int TITLE_HEIGHT = 80;
        const int DESCRIPTION_HEIGHT = 100;

        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS

        List<Button> buttons = new();
        WindowReward windowReward;

        #endregion

        #region PROPERTIES

        NavigationPanelVertical NavigationPanelVertical { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal WindowAcheivements()
            : base(ModalLevel.Window, BaseBounds.Bounds.Redim(), WindowType.Acheivements)
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
        ~WindowAcheivements()
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
                buttons[i].OnClick += ButtonClaimReward_OnClick;

            OnBack += WindowAcheivements_OnBack;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            for (int i = 0; i < buttons.Count; i++)
                buttons[i].OnClick -= ButtonClaimReward_OnClick;

            OnBack -= WindowAcheivements_OnBack;
        }

        void WindowAcheivements_OnBack(object sender, EventArgs e)
        {
            AcheivementsManager.Refresh();
        }

        void ButtonClaimReward_OnClick(object sender, OnClickEventArgs e)
        {
            AcheivementResume acheivementResume = (AcheivementResume)(sender as Button).Tag[0];
            WindowRewardParams windowRewardParams;

            if (acheivementResume.RewardType == RewardType.AllPerks)
                windowRewardParams = AcheivementsManager.RewardAllPerks(acheivementResume);
            else
                windowRewardParams = AcheivementsManager.RewardRandomPerks(acheivementResume);

            acheivementResume = AcheivementsManager.GetAcheivementResume(acheivementResume.AcheivementType);

            (sender as Button).Visible = false;
            (sender as Button).VisibleForNavigationPanel = false;
            (sender as Button).OnClick -= ButtonClaimReward_OnClick;
            ((Image)(sender as Button).Tag[1]).Visible = false;
            ((Image)(sender as Button).Tag[1]).VisibleForNavigationPanel = false;

            if (acheivementResume.Completed)
            {
                ((Image)(sender as Button).Tag[2]).Visible = true;
                ((Image)(sender as Button).Tag[2]).VisibleForNavigationPanel = true;
            }
            else
            {
                ((Image)(sender as Button).Tag[3]).Visible = true;
                ((Image)(sender as Button).Tag[3]).VisibleForNavigationPanel = true;
            }

            NavigationPanelVertical.Move();
            OrchestratorManager.OpenWindowRewardMessage(ref windowReward, windowRewardParams);
            windowReward.OnAccept += WindowReward_OnAccept;
        }

        private void WindowReward_OnAccept(object sender, EventArgs e)
        {
            WindowManager.Remove(windowReward.ID);
            windowReward = null;
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            SetPanel();
            SetAcheivements();
            SubscribeEvents();
            InteractiveObjectManager.Add(NavigationPanelVertical);
            SetTitle();
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
            Label labelTitle = new(ModalLevel, BaseBounds.Title, Resource.String.ACHEIVEMENTS.GetString(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            Image titleBackground = new(
                ModalLevel,
                new(0, 0, BaseBounds.Bounds.Width, BaseBounds.Title.Bottom),
                TextureManager.Get(new(BaseBounds.Bounds.Width, BaseBounds.Title.Bottom + OFFSET_X), ColorManager.WindowBodyColor, CommonTextureType.Rectangle).Texture, false)
            { AllowFadeInFadeOut = false };
            InteractiveObjectManager.Add(titleBackground, labelTitle);
        }

        void SetAcheivements()
        {
            for (int i = 0; i < AcheivementsManager.GetResume.Count; i++)
                SetAcheiment(i, AcheivementsManager.GetResume[i]);

            NavigationPanelVertical.Move();
        }

        void SetAcheiment(int index, AcheivementResume acheivementResume)
        {
            Rectangle bounds = new(BaseBounds.Limits.X, TOP + index * ITEM_HEIGHT, BaseBounds.Limits.Width, ITEM_HEIGHT);/*Bounds del item*/
            PanelItem panelItem = new(ModalLevel, bounds, GetButton(), GetImage(), GetTitle(acheivementResume), GetDescription(acheivementResume));

            if (acheivementResume.Completed)/*Muestra una imagen de completado*/
                SetCompleted(panelItem);
            else
                SetPendingOrIncompleted(panelItem, acheivementResume);

            NavigationPanelVertical.Add(panelItem);
        }

        Button GetButton()
        {
            Rectangle bounds = new(0, OFFSET_Y, BaseBounds.Limits.Width, ITEM_HEIGHT - OFFSET_Y.Double()); /*relativo al item*/
            Button button = new(ModalLevel, bounds) { EnableOnClick = false };
            return button;
        }

        Image GetImage()
        {
            Rectangle bounds = new(OFFSET_X, ITEM_HEIGHT.Half() - IMAGE_WH.Half(), IMAGE_WH, IMAGE_WH); /*relativo al item*/
            Image image = new(ModalLevel, bounds, TextureManager.TextureStar, true);
            return image;
        }

        Label GetTitle(AcheivementResume acheivementResume)
        {
            Rectangle bounds = new(OFFSET_X.Double() + IMAGE_WH, OFFSET_Y.Double(), BaseBounds.Limits.Width - OFFSET_X.Multi(4) - IMAGE_WH - BUTTON_WH, TITLE_HEIGHT); /*relativo al item*/
            Label label = new(ModalLevel, bounds, AcheivementsManager.GetAcheivementName(acheivementResume), Color.DarkGray, Color.DarkGray, AlignHorizontal.Center);
            return label;
        }

        Label GetDescription(AcheivementResume acheivementResume)
        {
            Rectangle bounds = new(OFFSET_X.Double() + IMAGE_WH, OFFSET_Y.Multi(3) + TITLE_HEIGHT, BaseBounds.Limits.Width - OFFSET_X.Multi(4) - IMAGE_WH - BUTTON_WH, DESCRIPTION_HEIGHT); /*relativo al item*/
            Label label = new(ModalLevel, bounds, AcheivementsManager.GetAcheivementDescription(acheivementResume), Color.DarkGray, Color.DarkGray, AlignHorizontal.Left, 2);
            return label;
        }

        Image SetCompleted(PanelItem item)
        {
            Rectangle bounds = new(item.Bounds.Width - OFFSET_X - IMAGE_WH, ITEM_HEIGHT.Half() - IMAGE_WH.Half(), IMAGE_WH, IMAGE_WH); /*relativo al item*/
            Image image = new(ModalLevel, bounds, TextureManager.TextureCompleted, true) { ColorDarkMode = ColorManager.HardGray, ColorLightMode = ColorManager.HardGray };
            item.Add(image);
            return image;
        }

        Image SetIncompleted(PanelItem item)
        {
            Rectangle bounds = new(item.Bounds.Width - OFFSET_X - IMAGE_WH, ITEM_HEIGHT.Half() - IMAGE_WH.Half(), IMAGE_WH, IMAGE_WH); /*relativo al item*/
            Image image = new(ModalLevel, bounds, TextureManager.TextureForbidden, true) { ColorDarkMode = ColorManager.HardGray, ColorLightMode = ColorManager.HardGray };
            item.Add(image);
            return image;
        }

        void SetPendingOrIncompleted(PanelItem item, AcheivementResume acheivementResume)
        {
            if (acheivementResume.Pending > 0)/*Muestra el botón de reclamar una recompensa*/
            {
                /*Botón de obtener recompensa y su imagen*/
                Rectangle bounds = new(item.Bounds.Width - BUTTON_WH - OFFSET_X, ITEM_HEIGHT.Half() - BUTTON_WH.Half(), BUTTON_WH, BUTTON_WH); /*relativo al item*/
                Button button = new(ModalLevel, bounds);
                bounds = new(bounds.X + BUTTON_WH.Half() - IMAGE_WH.Half(), ITEM_HEIGHT.Half() - IMAGE_WH.Half(), IMAGE_WH, IMAGE_WH);
                Image image = new(ModalLevel, bounds, TextureManager.TextureGift, true) { ColorDarkMode = ColorManager.HardGray, ColorLightMode = ColorManager.HardGray };
                buttons.Add(button);
                Image completedImage = SetCompleted(item);/*Imagen de completo, se mostrará al obtener la recompensa*/
                Image incompletedImage = SetIncompleted(item);/*Imagen de incompleto, se mostrará al obtener la recompensa*/
                completedImage.Visible = incompletedImage.Visible = completedImage.VisibleForNavigationPanel = incompletedImage.VisibleForNavigationPanel = false;
                item.Add(button, image, completedImage, incompletedImage);
                button.Tag.AddRange(new List<object>() { acheivementResume, image, completedImage, incompletedImage });
            }
            else/*Muestra imagen o texto de pendiente*/
                SetIncompleted(item);
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