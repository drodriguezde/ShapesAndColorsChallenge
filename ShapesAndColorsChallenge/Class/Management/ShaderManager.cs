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

using ShapesAndColorsChallenge.Class.Effects.Bloom;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class ShaderManager
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

        internal static BloomFilter BloomFilter { get; private set; }

        #endregion

        #region EVENTS

        internal static void LoadContent()
        {
            BloomFilter = new();
            BloomFilter.Load(Screen.GraphicsDevice, GameContent.ContentShader, Screen.Resolution.Width, Screen.Resolution.Height);
        }

        #endregion

        #region METHODS



        #endregion
    }
}