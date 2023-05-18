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
    internal class WindowLanguage__ : Window, IDisposable
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

        Image buttonCS;
        Image buttonDA;
        Image buttonDE;
        Image buttonEN;
        Image buttonES;
        Image buttonFI;
        Image buttonFR;
        Image buttonHU;
        Image buttonIT;
        Image buttonJA;
        Image buttonKO;
        Image buttonNL;
        Image buttonNO;
        Image buttonPL;
        Image buttonPT;
        Image buttonRU;
        Image buttonSV;
        Image buttonTR;
        Image buttonZH;

        NavigationPanelHorizontal navigationPanelHorizontal { get; set; }

        #endregion

        #region PROPERTIES

        Rectangle BoundsCS = new Rectangle(115, 150, 300, 200).Redim();
        Rectangle BoundsDA = new Rectangle(115, 500, 300, 200).Redim();
        Rectangle BoundsDE = new Rectangle(115, 850, 300, 200).Redim();
        Rectangle BoundsEN = new Rectangle(115, 1200, 300, 200).Redim();
        Rectangle BoundsES = new Rectangle(115, 1550, 300, 200).Redim();
        Rectangle BoundsFI = new Rectangle(655, 150, 300, 200).Redim();
        Rectangle BoundsFR = new Rectangle(655, 500, 300, 200).Redim();
        Rectangle BoundsHU = new Rectangle(655, 850, 300, 200).Redim();
        Rectangle BoundsIT = new Rectangle(655, 1200, 300, 200).Redim();
        Rectangle BoundsJA = new Rectangle(655, 1550, 300, 200).Redim();
        Rectangle BoundsKO = new Rectangle(115, 150, 300, 200).Redim();
        Rectangle BoundsNL = new Rectangle(115, 500, 300, 200).Redim();
        Rectangle BoundsNO = new Rectangle(115, 850, 300, 200).Redim();
        Rectangle BoundsPL = new Rectangle(115, 1200, 300, 200).Redim();
        Rectangle BoundsPT = new Rectangle(115, 1550, 300, 200).Redim();
        Rectangle BoundsRU = new Rectangle(655, 150, 300, 200).Redim();
        Rectangle BoundsSV = new Rectangle(655, 500, 300, 200).Redim();
        Rectangle BoundsTR = new Rectangle(655, 850, 300, 200).Redim();
        Rectangle BoundsZH = new Rectangle(655, 1200, 300, 200).Redim();

        #endregion

        #region CONSTRUCTORS

        internal WindowLanguage__()
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
        ~WindowLanguage__()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            buttonCS.OnClick += Button_OnClick;
            buttonDA.OnClick += Button_OnClick;
            buttonDE.OnClick += Button_OnClick;
            buttonEN.OnClick += Button_OnClick;
            buttonES.OnClick += Button_OnClick;
            buttonFI.OnClick += Button_OnClick;
            buttonFR.OnClick += Button_OnClick;
            buttonHU.OnClick += Button_OnClick;
            buttonIT.OnClick += Button_OnClick;
            buttonJA.OnClick += Button_OnClick;
            buttonKO.OnClick += Button_OnClick;
            buttonNL.OnClick += Button_OnClick;
            buttonNO.OnClick += Button_OnClick;
            buttonPL.OnClick += Button_OnClick;
            buttonPT.OnClick += Button_OnClick;
            buttonRU.OnClick += Button_OnClick;
            buttonSV.OnClick += Button_OnClick;
            buttonTR.OnClick += Button_OnClick;
            buttonZH.OnClick += Button_OnClick;
        }

        void UnsubscribeEvents()
        {
            buttonCS.OnClick -= Button_OnClick;
            buttonDA.OnClick -= Button_OnClick;
            buttonDE.OnClick -= Button_OnClick;
            buttonEN.OnClick -= Button_OnClick;
            buttonES.OnClick -= Button_OnClick;
            buttonFI.OnClick -= Button_OnClick;
            buttonFR.OnClick -= Button_OnClick;
            buttonHU.OnClick -= Button_OnClick;
            buttonIT.OnClick -= Button_OnClick;
            buttonJA.OnClick -= Button_OnClick;
            buttonKO.OnClick -= Button_OnClick;
            buttonNL.OnClick -= Button_OnClick;
            buttonNO.OnClick -= Button_OnClick;
            buttonPL.OnClick -= Button_OnClick;
            buttonPT.OnClick -= Button_OnClick;
            buttonRU.OnClick -= Button_OnClick;
            buttonSV.OnClick -= Button_OnClick;
            buttonTR.OnClick -= Button_OnClick;
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
            SetCS();/*Checo*/
            SetDA();/*Danés*/
            SetDE();/*Alemán*/
            SetEN();/*Inglés*/
            SetES();/*Español*/
            SetFI();/*Finlandés*/
            SetFR();/*Francés*/
            SetHU();/*Húngaro*/
            SetIT();/*Italiano*/
            SetJA();/*Japonés*/
            SetKO();/*Coreano*/
            SetNL();/*Holandés/Neerlandés*/
            SetNO();/*Noruego*/
            SetPL();/*Polaco*/
            SetPT();/*Portugués*/
            SetRU();/*Ruso*/
            SetSV();/*Sueco*/
            SetTR();/*Turco*/
            SetZH();/*Chino*/
        }

        Image FlagBackground(Language language, Rectangle flagBounds)
        {
            int borderSize = UserSettingsManager.CountryCode == language.ToString() ? Const.BUTTON_BORDER.Multi(8).RedimX() : Const.BUTTON_BORDER.RedimX();
            Color color = UserSettingsManager.CountryCode == language.ToString() ? Color.Cyan : ColorManager.HardGray;
            Rectangle bounds = new(flagBounds.X - borderSize.Half(), flagBounds.Y - borderSize.Half(), flagBounds.Width + borderSize, flagBounds.Height + borderSize);
            return new Image(ModalLevel, bounds, TextureManager.Get(bounds.ToSize(), color, CommonTextureType.Rectangle).Texture);
        }

        void SetCS()
        {
            buttonCS = new(ModalLevel, BoundsCS, TextureManager.Flag("cs")) { Tag = new() { Language.cs } };
            Image backImage = FlagBackground(Language.cs, BoundsCS);
            InteractiveObjectManager.Add(backImage, buttonCS);
            navigationPanelHorizontal.Add(1, buttonCS, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetDA()
        {
            buttonDA = new(ModalLevel, BoundsDA, TextureManager.Flag("da")) { Tag = new() { Language.da } };
            Image backImage = FlagBackground(Language.da, BoundsDA);
            InteractiveObjectManager.Add(backImage, buttonDA);
            navigationPanelHorizontal.Add(1, buttonDA, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetDE()
        {
            buttonDE = new(ModalLevel, BoundsDE, TextureManager.Flag("de")) { Tag = new() { Language.de } };
            Image backImage = FlagBackground(Language.de, BoundsDE);
            InteractiveObjectManager.Add(backImage, buttonDE);
            navigationPanelHorizontal.Add(1, buttonDE, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetEN()
        {
            buttonEN = new(ModalLevel, BoundsEN, TextureManager.Flag("en")) { Tag = new() { Language.en } };
            Image backImage = FlagBackground(Language.en, BoundsEN);
            InteractiveObjectManager.Add(backImage, buttonEN);
            navigationPanelHorizontal.Add(1, buttonEN, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetES()
        {
            buttonES = new(ModalLevel, BoundsES, TextureManager.Flag("es")) { Tag = new() { Language.es } };
            Image backImage = FlagBackground(Language.es, BoundsES);
            InteractiveObjectManager.Add(backImage, buttonES);
            navigationPanelHorizontal.Add(1, buttonES, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetFI()
        {
            buttonFI = new(ModalLevel, BoundsFI, TextureManager.Flag("fi")) { Tag = new() { Language.fi } };
            Image backImage = FlagBackground(Language.fi, BoundsFI);
            InteractiveObjectManager.Add(backImage, buttonFI);
            navigationPanelHorizontal.Add(1, buttonFI, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetFR()
        {
            buttonFR = new(ModalLevel, BoundsFR, TextureManager.Flag("fr")) { Tag = new() { Language.fr } };
            Image backImage = FlagBackground(Language.fr, BoundsFR);
            InteractiveObjectManager.Add(backImage, buttonFR);
            navigationPanelHorizontal.Add(1, buttonFR, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetHU()
        {
            buttonHU = new(ModalLevel, BoundsHU, TextureManager.Flag("hu")) { Tag = new() { Language.hu } };
            Image backImage = FlagBackground(Language.hu, BoundsHU);
            InteractiveObjectManager.Add(backImage, buttonHU);
            navigationPanelHorizontal.Add(1, buttonHU, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetIT()
        {
            buttonIT = new(ModalLevel, BoundsIT, TextureManager.Flag("it")) { Tag = new() { Language.it } };
            Image backImage = FlagBackground(Language.it, BoundsIT);
            InteractiveObjectManager.Add(backImage, buttonIT);
            navigationPanelHorizontal.Add(1, buttonIT, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetJA()
        {
            buttonJA = new(ModalLevel, BoundsJA, TextureManager.Flag("ja")) { Tag = new() { Language.ja } };
            Image backImage = FlagBackground(Language.ja, BoundsJA);
            InteractiveObjectManager.Add(backImage, buttonJA);
            navigationPanelHorizontal.Add(1, buttonJA, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetKO()
        {
            buttonKO = new(ModalLevel, BoundsKO, TextureManager.Flag("ko")) { Tag = new() { Language.ko } };
            Image backImage = FlagBackground(Language.ko, BoundsKO);
            InteractiveObjectManager.Add(backImage, buttonKO);
            navigationPanelHorizontal.Add(2, buttonKO, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetNL()
        {
            buttonNL = new(ModalLevel, BoundsNL, TextureManager.Flag("nl")) { Tag = new() { Language.nl } };
            Image backImage = FlagBackground(Language.nl, BoundsNL);
            InteractiveObjectManager.Add(backImage, buttonNL);
            navigationPanelHorizontal.Add(2, buttonNL, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetNO()
        {
            buttonNO = new(ModalLevel, BoundsNO, TextureManager.Flag("no")) { Tag = new() { Language.no } };
            Image backImage = FlagBackground(Language.no, BoundsNO);
            InteractiveObjectManager.Add(backImage, buttonNO);
            navigationPanelHorizontal.Add(2, buttonNO, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetPL()
        {
            buttonPL = new(ModalLevel, BoundsPL, TextureManager.Flag("pl")) { Tag = new() { Language.pl } };
            Image backImage = FlagBackground(Language.pl, BoundsPL);
            InteractiveObjectManager.Add(backImage, buttonPL);
            navigationPanelHorizontal.Add(2, buttonPL, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetPT()
        {
            buttonPT = new(ModalLevel, BoundsPT, TextureManager.Flag("pt")) { Tag = new() { Language.pt } };
            Image backImage = FlagBackground(Language.pt, BoundsPT);
            InteractiveObjectManager.Add(backImage, buttonPT);
            navigationPanelHorizontal.Add(2, buttonPT, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetRU()
        {
            buttonRU = new(ModalLevel, BoundsRU, TextureManager.Flag("ru")) { Tag = new() { Language.ru } };
            Image backImage = FlagBackground(Language.ru, BoundsRU);
            InteractiveObjectManager.Add(backImage, buttonRU);
            navigationPanelHorizontal.Add(2, buttonRU, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetSV()
        {
            buttonSV = new(ModalLevel, BoundsSV, TextureManager.Flag("sv")) { Tag = new() { Language.sv } };
            Image backImage = FlagBackground(Language.sv, BoundsSV);
            InteractiveObjectManager.Add(backImage, buttonSV);
            navigationPanelHorizontal.Add(2, buttonSV, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetTR()
        {
            buttonTR = new(ModalLevel, BoundsTR, TextureManager.Flag("tr")) { Tag = new() { Language.tr } };
            Image backImage = FlagBackground(Language.tr, BoundsTR);
            InteractiveObjectManager.Add(backImage, buttonTR);
            navigationPanelHorizontal.Add(2, buttonTR, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
        }

        void SetZH()
        {
            buttonZH = new(ModalLevel, BoundsZH, TextureManager.Flag("zh")) { Tag = new() { Language.zh } };
            Image backImage = FlagBackground(Language.zh, BoundsZH);
            InteractiveObjectManager.Add(backImage, buttonZH);
            navigationPanelHorizontal.Add(2, buttonZH, backImage);/*Esta linea debe ir después de InteractiveObjectManager.Add, para que salte LoadContent de cada objeto añadido*/
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