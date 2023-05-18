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
* DISPOSE CONTROL : STATIC
* 
*
* AUTHOR :
*
*
* CHANGES :
*
*
*/

using Microsoft.Xna.Framework.Content;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class GameContent
    {
        #region PROPERTIES

        static Visor Visor { get; set; }

        /// <summary>
        /// Gestiona los recursos de imagenes general para toda la aplicación.
        /// </summary>
        internal static ContentManager ContentImage { get; set; }

        /// <summary>
        /// Gestiona los recursos de tipos de letra para toda la aplicación.
        /// </summary>
        internal static ContentManager ContentFont { get; set; }

        /// <summary>
        /// Gestiona los recursos de música.
        /// </summary>
        internal static ContentManager ContentMusic { get; set; }

        /// <summary>
        /// Gestiona los recursos de sonido.
        /// </summary>
        internal static ContentManager ContentSound { get; set; }

        /// <summary>
        /// Content manager de los escenarios/niveles.
        /// Es como si fuera temporal, para poder descargar de memoria recursos que no se usan a menudo y ocupan mucho.
        /// Se cargan y descargan bajo demanda.
        /// </summary>
        internal static ContentManager ContentStage { get; set; }

        /// <summary>
        /// Content manager de las animaciones.
        /// Es como si fuera temporal, para poder descargar de memoria recursos que no se usan a menudo y ocupan mucho.
        /// Se cargan y descargan bajo demanda.
        /// </summary>
        internal static ContentManager ContentAnimation { get; set; }

        /// <summary>
        /// Content manager de los efectos de imagen.
        /// Es como si fuera temporal, para poder descargar de memoria recursos que no se usan a menudo y ocupan mucho.
        /// Se cargan y descargan bajo demanda.
        /// </summary>
        internal static ContentManager ContentShader { get; set; }

        #endregion

        #region METHODS

        internal static void SetVisor(Visor visor)
        {
            Visor = visor;
        }

        /// <summary>
        /// Reinicia el content de los recursos de imagen en general de la aplicación.
        /// </summary>
        internal static void ResetContentImage()
        {
            ContentImage = GetNewContentManagerInstance();
        }

        /// <summary>
        /// Reinicia el content de los recursos de tipo de letra de la aplicación.
        /// </summary>
        internal static void ResetContentFont()
        {
            ContentFont = GetNewContentManagerInstance();
        }

        /// <summary>
        /// Reinicia el content de los recursos de música.
        /// </summary>
        internal static void ResetContentMusic()
        {
            ContentMusic = GetNewContentManagerInstance();
        }

        /// <summary>
        /// Reinicia el content de los recursos de sonido.
        /// </summary>
        internal static void ResetContentSound()
        {
            ContentSound = GetNewContentManagerInstance();
        }

        /// <summary>
        /// Reinicia el content de los recursos de niveles/etapas.
        /// </summary>
        internal static void ResetContentStage()
        {
            ContentStage = GetNewContentManagerInstance();
        }

        /// <summary>
        /// Reinicia el content de los recursos de animaciones.
        /// </summary>
        internal static void ResetContentAnimation()
        {
            ContentAnimation = GetNewContentManagerInstance();
        }

        /// <summary>
        /// Reinicia el content de los recursos de efecto de imagen.
        /// </summary>
        internal static void ResetContentShader()
        {
            ContentShader = GetNewContentManagerInstance();
        }

        /// <summary>
        /// Decarga el contenido de recursos del juego.
        /// </summary>
        internal static void UnloadAllContent()
        {
            ContentImage.Unload();
            ContentFont.Unload();
            ContentMusic.Unload();
            ContentSound.Unload();
            ContentStage.Unload();
            ContentAnimation.Unload();
            ContentShader.Unload();
        }

        /// <summary>
        /// Crea una nueva instancia del gestor de contenidos.
        /// </summary>
        /// <returns></returns>
        static ContentManager GetNewContentManagerInstance()
        {
            ContentManager contentManager = new(Visor.Game.Content.ServiceProvider, Visor.Game.Content.RootDirectory)
            {
                RootDirectory = "Content"
            };

            return contentManager;
        }

        #endregion
    }
}