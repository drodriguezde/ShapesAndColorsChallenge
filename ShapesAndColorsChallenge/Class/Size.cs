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

using System;

namespace ShapesAndColorsChallenge.Class
{
    internal class Size : IDisposable, IEquatable<Size>
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

        internal int Width { get; private set; } = 0;

        internal int Height { get; private set; } = 0;

        #endregion

        #region CONSTRUCTORS

        internal Size(int width, int height)
        {
            Width = width;
            Height = height;
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
        ~Size()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS



        #endregion

        #region METHODS

        internal void Redim()
        {
            Width *= Screen.RedimMatrix.X.ToInt();
            Height *= Screen.RedimMatrix.Y.ToInt();
        }

        #endregion

        #region OPERATORS

        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Width == size2.Width && size1.Height == size2.Height;
        }

        public static bool operator !=(Size size1, Size size2)
        {
            return size1.Width != size2.Width || size1.Height != size2.Height;
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public bool Equals(Size other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            int hashCode = -658672633;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + disposed.GetHashCode();
            return hashCode;
        }

        #endregion
    }
}