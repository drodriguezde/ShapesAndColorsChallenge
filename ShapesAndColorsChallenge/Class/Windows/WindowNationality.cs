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
using ShapesAndColorsChallenge.Class.Web;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowNationality : Window, IDisposable
    {
        #region CONST

        const int NAVIGATION_PANEL_BOTTOM_BUTTONS = 1800;

        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        List<Image> imagesNation = new List<Image>();

        NavigationPanelHorizontal navigationPanelHorizontal { get; set; }

        #endregion

        #region PROPERTIES

        Rectangle BoundsColumn1Row1 = new Rectangle(115, 150, 300, 200).Redim();
        Rectangle BoundsColumn1Row2 = new Rectangle(115, 500, 300, 200).Redim();
        Rectangle BoundsColumn1Row3 = new Rectangle(115, 850, 300, 200).Redim();
        Rectangle BoundsColumn1Row4 = new Rectangle(115, 1200, 300, 200).Redim();
        Rectangle BoundsColumn1Row5 = new Rectangle(115, 1550, 300, 200).Redim();
        Rectangle BoundsColumn2Row1 = new Rectangle(655, 150, 300, 200).Redim();
        Rectangle BoundsColumn2Row2 = new Rectangle(655, 500, 300, 200).Redim();
        Rectangle BoundsColumn2Row3 = new Rectangle(655, 850, 300, 200).Redim();
        Rectangle BoundsColumn2Row4 = new Rectangle(655, 1200, 300, 200).Redim();
        Rectangle BoundsColumn2Row5 = new Rectangle(655, 1550, 300, 200).Redim();

        #endregion

        #region CONSTRUCTORS

        internal WindowNationality()
            : base(ModalLevel.Window, BaseBounds.Bounds.Redim(), WindowType.Nationality)
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
        ~WindowNationality()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            for (int i = 0; i < imagesNation.Count; i++)
                imagesNation[i].OnClick += Button_OnClick;
        }

        void UnsubscribeEvents()
        {
            for (int i = 0; i < imagesNation.Count; i++)
                imagesNation[i].OnClick -= Button_OnClick;
        }

        void Button_OnClick(object sender, OnClickEventArgs e)
        {
            if (UserSettingsManager.PlayerCountryCode != (sender as Image).Tag[0].ToString())
            {
                UserSettingsManager.PlayerCountryCode = (sender as Image).Tag[0].ToString();

                /*Tambien hay que actualizar el pais en la lista de jugadores*/
                Player player = ControllerPlayer.Get().Single(t => t.IsPlayer);
                player.Country = UserSettingsManager.PlayerCountryCode;
                ControllerPlayer.Update(player);
                RestOrchestrator.TryToUpdatePlayerInfo();
            }

            CloseMeAndOpenThis(WindowType.Settings);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetPanel();
            SetButtons();
            SubscribeEvents();
            navigationPanelHorizontal.Set();
        }

        void SetPanel()
        {
            navigationPanelHorizontal = new NavigationPanelHorizontal(ModalLevel, Bounds, NAVIGATION_PANEL_BOTTOM_BUTTONS.RedimY(), this);
            InteractiveObjectManager.Add(navigationPanelHorizontal);
        }

        void SetButtons()
        {
            int index = 0;

            for (int i = 0; i < Const.ALL_FLAGS.Count; i++, index++)
            {
                Image buttonFlag = new(ModalLevel, GetBoundsByIndex(index), TextureManager.Flag(Const.ALL_FLAGS[i])) { Tag = new() { Const.ALL_FLAGS[i] } };
                Image backImage = FlagBackground(Const.ALL_FLAGS[i], GetBoundsByIndex(index));
                imagesNation.Add(buttonFlag);
                InteractiveObjectManager.Add(backImage, buttonFlag);
                navigationPanelHorizontal.Add(i / 10 + 1, backImage, buttonFlag);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/

                if (index == 9)
                    index = -1;
            }
        }

        Rectangle GetBoundsByIndex(int index)
        {
            return index switch
            {
                0 => BoundsColumn1Row1,
                1 => BoundsColumn1Row2,
                2 => BoundsColumn1Row3,
                3 => BoundsColumn1Row4,
                4 => BoundsColumn1Row5,
                5 => BoundsColumn2Row1,
                6 => BoundsColumn2Row2,
                7 => BoundsColumn2Row3,
                8 => BoundsColumn2Row4,
                9 => BoundsColumn2Row5,
                _ => BoundsColumn1Row1,
            };
        }

        Image FlagBackground(string language, Rectangle flagBounds)
        {
            int borderSize = UserSettingsManager.PlayerCountryCode == language ? Const.BUTTON_BORDER.Multi(8).RedimX() : Const.BUTTON_BORDER.RedimX();
            Color color = UserSettingsManager.PlayerCountryCode == language ? Color.Cyan : ColorManager.HardGray;
            Rectangle bounds = new(flagBounds.X - borderSize.Half(), flagBounds.Y - borderSize.Half(), flagBounds.Width + borderSize, flagBounds.Height + borderSize);
            return new(ModalLevel, bounds, TextureManager.Get(bounds.ToSize(), color, CommonTextureType.Rectangle).Texture);
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