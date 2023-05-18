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
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Types;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowRankings : Window, IDisposable
    {
        #region CONST

        const int TOP = 300;
        const int POSITION_WIDTH = 50;
        const int NAME_WIDTH = 705;
        const int FLAG_WIDTH = 150;
        const int OFFSET_X = 30;
        const int OFFSET_Y = 6;
        const int INNER_OBJECT_HEIGHT = 100;
        const int ITEM_HEIGHT = 112;

        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS        

        readonly List<RankingByGameMode> rankingByGameMode = ControllerRanking.GetWithPlayers(OrchestratorManager.GameMode);
        BackgroundWorker workerLoadRanking;
        ProgressBar progressBar;

        #endregion

        #region PROPERTIES

        NavigationPanelVertical NavigationPanelVertical { get; set; }

        /// <summary>
        /// Indica la ubicación del jugador en el ranking en medida de distancia para poder centrarlo al cargar.
        /// </summary>
        int PlayerLocationTop { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal WindowRankings()
            : base(ModalLevel.Window, BaseBounds.Bounds.Redim(), WindowType.Rankings)
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
        ~WindowRankings()
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
            /*TODO, botón volver*/


        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            /*TODO, botón volver*/


        }

        private void WorkerLoadRanking_DoWork(object sender, DoWorkEventArgs e)
        {
            AddItemsToPanel();
        }

        private void WorkerLoadRanking_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.SetValue(e.ProgressPercentage);
        }

        private void WorkerLoadRanking_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetTitle();
            base.LoadContent();/*Se pone el útimo para que el botón "Atrás" este por encima.*/
            Nuller.Null(ref progressBar);
            progressBar = null;
            NavigationPanelVertical.Locked = false;
            NavigationPanelVertical.Move(PlayerLocationTop);
            InteractiveObjectManager.SkipUpdate = InteractiveObjectManager.SkipDraw = false;
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {            
            SetPanel();            
            SubscribeEvents();            
            SetRanking();
            InteractiveObjectManager.Add(NavigationPanelVertical);/*Tiene que ir el último por el lanzamiento de LoadContent*/
        }

        void SetPanel()
        {
            NavigationPanelVertical = new(
                ModalLevel,
                new Rectangle(
                    BaseBounds.Limits.X,
                    TOP,
                    BaseBounds.Limits.Width,
                    ITEM_HEIGHT * rankingByGameMode.Count),
                TOP,
                BaseBounds.Limits.X + BaseBounds.Limits.Height - BaseBounds.Button.Height - 20, this)
            { Locked = true };
        }

        void SetTitle()
        {
            Label labelTitle = new(ModalLevel, BaseBounds.Title, $"{Resource.String.RANKING.GetString()} ({Statics.GetGameModeTitle()})", ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            Image titleBackground = new(
                ModalLevel,
                new(0, 0, BaseBounds.Bounds.Width, BaseBounds.Title.Bottom),
                TextureManager.Get(new(BaseBounds.Bounds.Width, BaseBounds.Title.Bottom + OFFSET_X), ColorManager.WindowBodyColor, CommonTextureType.Rectangle).Texture, false)
            { AllowFadeInFadeOut = false };
            InteractiveObjectManager.Add(titleBackground, labelTitle);
        }

        void SetRanking()
        {
            InteractiveObjectManager.SkipUpdate = InteractiveObjectManager.SkipDraw = true;
            progressBar = new(ModalLevel.MessageBox, 100, new(BaseBounds.Limits.X, BaseBounds.Bounds.Height.Half() - 25, BaseBounds.Limits.Width, 50)) { Color = Color.DarkCyan, DrawProgressString = true, IsPercent = true };
            workerLoadRanking = new()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };
            workerLoadRanking.DoWork += WorkerLoadRanking_DoWork;
            workerLoadRanking.ProgressChanged += WorkerLoadRanking_ProgressChanged;
            workerLoadRanking.RunWorkerCompleted += WorkerLoadRanking_RunWorkerCompleted;
            workerLoadRanking.RunWorkerAsync();
        }

        void AddItemsToPanel()
        {
            PlayerLocationTop = rankingByGameMode.FindIndex(t => t.IsPlayer) * ITEM_HEIGHT;

            for (int i = 0; i < rankingByGameMode.Count; i++)
            {
                Rectangle bounds = new(BaseBounds.Limits.X, TOP + i * ITEM_HEIGHT, BaseBounds.Limits.Width, ITEM_HEIGHT);/*Bounds del item*/
                PanelItem panelItem = new(ModalLevel, bounds, GetPositionLabel(rankingByGameMode[i].Position), GetNameLabel(rankingByGameMode[i].Name), GetFlagBackground(), GetFlagImage(rankingByGameMode[i].Country)) { Highlight = rankingByGameMode[i].IsPlayer };
                NavigationPanelVertical.Add(panelItem);
                workerLoadRanking.ReportProgress((int)Math.Ceiling(100f * i / rankingByGameMode.Count));
            }
        }

        Label GetPositionLabel(int position)
        {
            Rectangle bounds = new(0, OFFSET_Y, POSITION_WIDTH, INNER_OBJECT_HEIGHT); /*relativo al item*/
            Label label = new(ModalLevel, bounds, position.ToString(), ColorManager.HardGray, ColorManager.LightGray, AlignHorizontal.Center);
            return label;
        }

        Label GetNameLabel(string name)
        {
            Rectangle bounds = new(POSITION_WIDTH + OFFSET_X, OFFSET_Y, NAME_WIDTH, INNER_OBJECT_HEIGHT);/*relativo al item*/
            Label labelName = new(ModalLevel, bounds, name, ColorManager.HardGray, ColorManager.LightGray, AlignHorizontal.Center);
            return labelName;
        }

        Image GetFlagImage(string country)
        {
            Rectangle bounds = new(POSITION_WIDTH + NAME_WIDTH + OFFSET_X.Double(), OFFSET_Y, FLAG_WIDTH, INNER_OBJECT_HEIGHT);/*relativo al item*/
            Image image = new(ModalLevel, bounds, TextureManager.Flag(country), true, 0, false);/*No se hace Dispose*/
            return image;
        }

        Image GetFlagBackground()
        {
            Rectangle bounds = new(
                POSITION_WIDTH + NAME_WIDTH + OFFSET_X.Double() - Const.BUTTON_BORDER.Half(),
                OFFSET_Y - Const.BUTTON_BORDER.Half(),
                FLAG_WIDTH + Const.BUTTON_BORDER,
                INNER_OBJECT_HEIGHT + Const.BUTTON_BORDER);/*relativo al item*/
            Image image = new(ModalLevel, bounds, TextureManager.Get(bounds.ToSize(), ColorManager.HardGray, CommonTextureType.Rectangle).Texture);/*No se hace Dispose*/
            return image;
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            progressBar?.Draw(gameTime);
        }

        #endregion
    }
}