using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Params
{
    internal class MusicFadeOutParams
    {
        #region PROPERTIES

        /// <summary>
        /// Indica si se debe repetir la canción en bucle.
        /// </summary>
        internal bool Repeat { get; set; }

        /// <summary>
        /// Canción que se debe reproducir.
        /// </summary>
        internal string Song { get; set; }

        /// <summary>
        /// Indica si se debe reproducir una canción aleatoria.
        /// </summary>
        internal bool Random { get; set; }

        /// <summary>
        /// Indica si hay que detener la música sin hacer sonar nada después.
        /// </summary>
        internal bool OnlyStop { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal MusicFadeOutParams(bool repeat, string song, bool random, bool onlyStop)
        { 
            Repeat = repeat;
            Song = song;
            Random = random;
            OnlyStop = onlyStop;
        }

        #endregion
    }
}
