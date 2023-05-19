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
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Entities
{
    public abstract class Entity : IDisposable
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

        /// <summary>
        /// Orden en que se dibujará esa entidad.
        /// Se usa habitualmente para las ventanas, todos los hijos de la ventana se pintan cuando la ventana.
        /// Los hijos de la ventana tienen su modal level.
        /// </summary>
        internal ModalLevel ModalLevel { get; private set; }

        internal Rectangle Bounds { get; set; } = Rectangle.Empty;

        internal Vector2 Location
        {
            get
            {
                return Bounds.Location.ToVector2();
            }
            set
            {
                Bounds = new Rectangle(value.X.ToInt(), value.Y.ToInt(), Bounds.Width, Bounds.Height);
            }
        }

        /// <summary>
        /// Tamaño y ubicación original e inmutable.
        /// </summary>
        internal Rectangle OriginalBounds { get; private set; }

        /// <summary>
        /// Ubicación original e inmutable.
        /// </summary>
        internal Vector2 OriginalLocation { get { return OriginalBounds.Location.ToVector2(); } }

        /// <summary>
        /// Identificador único global de la entidad.
        /// </summary>
        internal long ID { get; private set; }

        internal bool Visible { get; set; } = true;

        /// <summary>
        /// Indica que si este objeto está dentro de un panel de navegación se debe tener en cuenta este valor por encima del Visible.
        /// </summary>
        internal bool VisibleForNavigationPanel { get; set; } = true;

        /// <summary>
        /// Se usa para indicar si la entidad esta activa o no aunque este cargada.
        /// </summary>
        internal bool Active { get; set; } = true;

        /// <summary>
        /// Indica si al iniciar la ventana está activo el modo oscuro, por si cambia a mitad de la sesión de juego.
        /// </summary>
        internal bool IsDarkModeCurrentlyActive { get; set; } = false;

        #endregion

        #region CONSTRUCTORS

        public Entity()
        {
            ID = Statics.NewID();
        }

        public Entity(ModalLevel modalLevel)
        {
            ID = Statics.NewID();
            ModalLevel = modalLevel;
        }

        public Entity(ModalLevel modalLevel, Rectangle bounds)
        {
            ID = Statics.NewID();
            ModalLevel = modalLevel;
            Bounds = bounds;
            OriginalBounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
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
        protected virtual void Dispose(bool disposing)
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
        ~Entity()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS

        internal abstract void LoadContent();

        internal abstract void Update(GameTime gameTime);

        internal abstract void Draw(GameTime gameTime);

        internal virtual void SetColorMode()
        {

        }

        #endregion
    }
}