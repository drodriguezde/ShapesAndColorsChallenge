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
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowLanguage : Window, IDisposable
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

        Image buttonDA;
        Image buttonDE;
        Image buttonEN;
        Image buttonES;
        Image buttonFR;
        Image buttonIT;
        Image buttonJA;
        Image buttonKO;
        Image buttonZH;

        #endregion

        #region PROPERTIES

        Rectangle BoundsES = new Rectangle(115, 150, 300, 200).Redim();
        Rectangle BoundsDA = new Rectangle(115, 500, 300, 200).Redim();
        Rectangle BoundsDE = new Rectangle(115, 850, 300, 200).Redim();
        Rectangle BoundsEN = new Rectangle(115, 1200, 300, 200).Redim();
        Rectangle BoundsKO = new Rectangle(115, 1550, 300, 200).Redim();
        Rectangle BoundsJA = new Rectangle(655, 150, 300, 200).Redim();
        Rectangle BoundsFR = new Rectangle(655, 500, 300, 200).Redim();
        Rectangle BoundsIT = new Rectangle(655, 850, 300, 200).Redim();
        Rectangle BoundsZH = new Rectangle(655, 1200, 300, 200).Redim();

        #endregion

        #region CONSTRUCTORS

        internal WindowLanguage()
            : base(ModalLevel.Window, BaseBounds.Bounds.Redim(), WindowType.Language)
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
        ~WindowLanguage()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            buttonDA.OnClick += Button_OnClick;
            buttonDE.OnClick += Button_OnClick;
            buttonEN.OnClick += Button_OnClick;
            buttonES.OnClick += Button_OnClick;
            buttonFR.OnClick += Button_OnClick;
            buttonIT.OnClick += Button_OnClick;
            buttonJA.OnClick += Button_OnClick;
            buttonKO.OnClick += Button_OnClick;
            buttonZH.OnClick += Button_OnClick;
        }

        void UnsubscribeEvents()
        {
            buttonDA.OnClick -= Button_OnClick;
            buttonDE.OnClick -= Button_OnClick;
            buttonEN.OnClick -= Button_OnClick;
            buttonES.OnClick -= Button_OnClick;
            buttonFR.OnClick -= Button_OnClick;
            buttonIT.OnClick -= Button_OnClick;
            buttonJA.OnClick -= Button_OnClick;
            buttonKO.OnClick -= Button_OnClick;
            buttonZH.OnClick -= Button_OnClick;
        }

        void Button_OnClick(object sender, OnClickEventArgs e)
        {
            Language language = (Language)(sender as Image).Tag[0];

            if (UserSettingsManager.CountryCode != language.ToString())
                UserSettingsManager.CountryCode = language.ToString();

#if DEBUG
            DebugManager.RunLanguageTest();
#endif

            CloseMeAndOpenThis(WindowType.Settings);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetButtons();
            SubscribeEvents();
        }

        void SetButtons()
        {
            SetDA();/*Danés*/
            SetDE();/*Alemán*/
            SetEN();/*Inglés*/
            SetES();/*Español*/
            SetFR();/*Francés*/
            SetIT();/*Italiano*/
            SetJA();/*Japonés*/
            SetKO();/*Coreano*/
            SetZH();/*Chino*/
        }

        Image FlagBackground(Language language, Rectangle flagBounds)
        {
            int borderSize = UserSettingsManager.CountryCode == language.ToString() ? Const.BUTTON_BORDER.Multi(8).RedimX() : Const.BUTTON_BORDER.RedimX();
            Color color = UserSettingsManager.CountryCode == language.ToString() ? Color.Cyan : ColorManager.HardGray;
            Rectangle bounds = new(flagBounds.X - borderSize.Half(), flagBounds.Y - borderSize.Half(), flagBounds.Width + borderSize, flagBounds.Height + borderSize);
            return new Image(ModalLevel, bounds, TextureManager.Get(bounds.ToSize(), color, CommonTextureType.Rectangle).Texture);
        }

        void SetDA()
        {
            buttonDA = new(ModalLevel, BoundsDA, TextureManager.Flag("da")) { Tag = new() { Language.da } };
            Image backImage = FlagBackground(Language.da, BoundsDA);
            InteractiveObjectManager.Add(backImage, buttonDA);
            //navigationPanelHorizontal.Add(1, buttonDA, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetDE()
        {
            buttonDE = new(ModalLevel, BoundsDE, TextureManager.Flag("de")) { Tag = new() { Language.de } };
            Image backImage = FlagBackground(Language.de, BoundsDE);
            InteractiveObjectManager.Add(backImage, buttonDE);
            //navigationPanelHorizontal.Add(1, buttonDE, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetEN()
        {
            buttonEN = new(ModalLevel, BoundsEN, TextureManager.Flag("en")) { Tag = new() { Language.en } };
            Image backImage = FlagBackground(Language.en, BoundsEN);
            InteractiveObjectManager.Add(backImage, buttonEN);
            //navigationPanelHorizontal.Add(1, buttonEN, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetES()
        {
            buttonES = new(ModalLevel, BoundsES, TextureManager.Flag("es")) { Tag = new() { Language.es } };
            Image backImage = FlagBackground(Language.es, BoundsES);
            InteractiveObjectManager.Add(backImage, buttonES);
            //navigationPanelHorizontal.Add(1, buttonES, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetFR()
        {
            buttonFR = new(ModalLevel, BoundsFR, TextureManager.Flag("fr")) { Tag = new() { Language.fr } };
            Image backImage = FlagBackground(Language.fr, BoundsFR);
            InteractiveObjectManager.Add(backImage, buttonFR);
            //navigationPanelHorizontal.Add(1, buttonFR, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetIT()
        {
            buttonIT = new(ModalLevel, BoundsIT, TextureManager.Flag("it")) { Tag = new() { Language.it } };
            Image backImage = FlagBackground(Language.it, BoundsIT);
            InteractiveObjectManager.Add(backImage, buttonIT);
            //navigationPanelHorizontal.Add(1, buttonIT, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetJA()
        {
            buttonJA = new(ModalLevel, BoundsJA, TextureManager.Flag("ja")) { Tag = new() { Language.ja } };
            Image backImage = FlagBackground(Language.ja, BoundsJA);
            InteractiveObjectManager.Add(backImage, buttonJA);
            //navigationPanelHorizontal.Add(1, buttonJA, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetKO()
        {
            buttonKO = new(ModalLevel, BoundsKO, TextureManager.Flag("ko")) { Tag = new() { Language.ko } };
            Image backImage = FlagBackground(Language.ko, BoundsKO);
            InteractiveObjectManager.Add(backImage, buttonKO);
            //navigationPanelHorizontal.Add(2, buttonKO, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetZH()
        {
            buttonZH = new(ModalLevel, BoundsZH, TextureManager.Flag("zh")) { Tag = new() { Language.zh } };
            Image backImage = FlagBackground(Language.zh, BoundsZH);
            InteractiveObjectManager.Add(backImage, buttonZH);
            //navigationPanelHorizontal.Add(2, buttonZH, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
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