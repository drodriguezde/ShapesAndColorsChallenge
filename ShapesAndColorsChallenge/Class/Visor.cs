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
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Management;
using System;

namespace ShapesAndColorsChallenge.Class
{
    internal class Visor : IDisposable
    {
        #region CONST

        internal static readonly Size Resolution1920x1080 = new Size(1920, 1080);

        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS



        #endregion

        #region PROPERTIES

        internal Main Game { get; private set; }

        #endregion

        #region CONSTRUCTORS

        internal Visor(Main game)
        {
            Game = game;
            Deploy();
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
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos no administrados que utiliza, y libera los recursos administrados de forma opcional.
        /// </summary>
        /// <param name="disposing">True si se quiere liberar los recursos administrados.</param>
        protected void Dispose(bool disposing)
        {
            if (disposed)
                return;

            /*Objetos administrados aquí*/
            if (disposing)
            {

            }

            /*Objetos no administrados aquí*/

            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Visor()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        /// <summary>
        /// Este evento se dispara cuando se ha reseteado el dispositivo gráfico.
        /// Contiene la resolución después del reseteo.
        /// Establece la resolución real adoptada por el visor y la que inicialmente se le pidió.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Graphics_DeviceReset(object sender, EventArgs e)
        {
            Screen.SetResolution();
        }

        #endregion

        #region METHODS

        internal void Initialize()
        {
            try
            {
                /*Estos 3 se usan para establecer el redibujado*/
                Game.IsFixedTimeStep = true;/*Si se establece en falso, desvincula la actualización y el dibujado, lo que permite que se llamen por separado*/
                Screen.Graphics.SynchronizeWithVerticalRetrace = false;/*False no limita los fps, True limita los fps (redibujado) a la tasa de refresco de la pantalla*/
                Game.TargetElapsedTime = new TimeSpan((long)(1000d / 60 * 10000d));/*Establece la tasa de fps a 60*/
                /**/
                Screen.Graphics.PreferMultiSampling = true;/*Activa el anti-aliasing*/
                Game.GraphicsDevice.PresentationParameters.MultiSampleCount = 8;
                Game.GraphicsDevice.PresentationParameters.PresentationInterval = PresentInterval.One;
                Screen.Graphics.DeviceReset += Graphics_DeviceReset;
                Screen.Graphics.ApplyChanges();
                Screen.GraphicsDevice = Game.GraphicsDevice;
                GameContent.ContentImage = Game.Content;
                GameContent.ContentFont = Game.Content;
                GameContent.ContentMusic = Game.Content;
                GameContent.ContentSound = Game.Content;
                GameContent.ContentStage = Game.Content;
                GameContent.ContentAnimation = Game.Content;
                GameContent.ContentShader = Game.Content;
                Screen.SetResolution();
                ResetAllContent();
            }
            catch (Exception exception)
            {
                Statics.TraceException(exception.Message);
            }
        }

        void ResetAllContent()
        {
            GameContent.ResetContentStage();
            GameContent.ResetContentFont();
            GameContent.ResetContentMusic();
            GameContent.ResetContentSound();
            GameContent.ResetContentStage();
            GameContent.ResetContentAnimation();
            GameContent.ResetContentShader();
        }

        void Deploy()
        {
            try
            {
                DeployGraphics();
                DeployContent();
                ShowMouse(false);
            }
            catch (Exception exception)
            {
                Statics.TraceException(exception.Message);
            }
        }

        internal void DeployGraphics()
        {
            try
            {
                Screen.Graphics = new GraphicsDeviceManager(Game)
                {
                    IsFullScreen = false,
                    GraphicsProfile = GraphicsProfile.HiDef
                };

                Screen.Graphics.ApplyChanges();
            }
            catch (Exception exception)
            {
                Statics.TraceException(exception.Message);
            }
        }

        void DeployContent()
        {
            try
            {
                Game.Content.RootDirectory = "Content";
            }
            catch (Exception exception)
            {
                Statics.TraceException(exception.Message);
            }
        }

        internal void ShowMouse(bool showMouse)
        {
            Game.IsMouseVisible = showMouse;
        }

        #endregion
    }
}