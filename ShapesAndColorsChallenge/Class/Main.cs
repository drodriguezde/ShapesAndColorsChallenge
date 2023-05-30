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
using ShapesAndColorsChallenge.Class.Management;
using System;

namespace ShapesAndColorsChallenge.Class
{
    public class Main : Game
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS



        #endregion

        #region PROPERTIES

        Visor Visor { get; set; }

        #endregion

        #region CONSTRUCTORS

        public Main()
        {
            Visor = new Visor(this);
            GameContent.SetVisor(Visor);
            Screen.Initialize(this);
            DataBaseManager.Initialize();/*Debe ser la primera en inicializar*/
            UserSettingsManager.Initialize();/*Debe estar después de DataBaseManager.Initialize*/
            TouchManager.Initialize(this);
#if DEBUG
            DebugManager.Initialize(this);
#endif
            ExitManager.Initialize(this);
        }

        #endregion

        #region DESTRUCTOR



        #endregion

        #region EVENTS

        protected override void OnExiting(object sender, EventArgs args)
        {
            UnloadContent();
            base.OnExiting(sender, args);
        }

        /// <summary>
        /// Se dispara cuando el juego pasa a primer plano.
        /// </summary>
        protected override void OnActivated(object sender, EventArgs args)
        {
            Screen.IsActive = true;
            base.OnActivated(sender, args);
        }

        /// <summary>
        /// Se dispara cuando el juego pasa a segundo plano.
        /// </summary>
        protected override void OnDeactivated(object sender, EventArgs args)
        {
            /*TODO, hay que lanzar un hilo para que siga contando el tiempo y no se hagan trampas
             * al porder ver la aplicación en el panel del dispositivo cunado está en segundo plano*/

            Screen.IsActive = false;
            base.OnDeactivated(sender, args);
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Obtiene una textura con la imagen actual mostrada por el juego.
        /// </summary>
        internal void TakeScreenshot(GameTime gameTime)
        {
            Draw(gameTime);
        }

        protected override void Initialize()
        {
            Visor.Initialize();
            Screen.ToggleFullScreen();/*Es necesario ya que las dimensiones y coordenadas iniciales no son adecuadas*/
            Screen.DeploySpriteBatch();
            base.Initialize();
        }

        /// <summary>
        /// Carga el contenido inicial.
        /// </summary>
        protected override void LoadContent()
        {
            AcheivementsManager.Refresh();
            ChallengesManager.Refresh();/*Añadimos retos si es posible*/
            FontManager.LoadContent();/*Se mantiene durante toda la vida la aplicación*/
            TextureManager.LoadContent();/*Se mantiene durante toda la vida la aplicación*/
            SoundManager.LoadContent();/*Cada vez que se reproduce una canción se reinicia el content de música para reducir el consumo de memoria*/
        }

        /// <summary>
        /// Decarga el contenido de recursos del juego.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            /*TODO, descargar recursos*/
        }

        protected override void Update(GameTime gameTime)
        {
            TouchManager.Update(gameTime);
            WindowManager.Update(gameTime);/*Actualiza los elemento de la interfaz*/
            ExitManager.Update(gameTime);
#if DEBUG
            DebugManager.Update(gameTime);
#endif
            OrchestratorManager.Update(gameTime);/*Esta linea la última antes de base.Update(gameTime);, en caso contrario no fucniona correctamente el botón back*/
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Screen.GraphicsDevice.Clear(Color.FromNonPremultiplied(ColorManager.WindowBodyColor.ToVector4()));/*Limpiamos la pantalla*/
            Screen.SpriteBatchBeginUI();
            WindowManager.Draw(gameTime);
#if DEBUG
            DebugManager.Draw(gameTime, false, false, false, false, false, false, false);
#endif
            Screen.SpriteBatchEnd();
            base.Draw(gameTime);
        }

        #endregion
    }
}
