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

namespace ShapesAndColorsChallenge.Class
{
    internal class FrameRateCounter
    {
        double frames = 0;
        double updates = 0;
        double elapsed = 0;
        double last = 0;
        double now = 0;
        internal double msgFrequency = 1.0f;
        internal string msg = "";

        internal void Update(GameTime gameTime)
        {
            now = gameTime.TotalGameTime.TotalSeconds;
            elapsed = now - last;

            if (elapsed > msgFrequency)
            {
                msg = $" Fps: {(frames / elapsed).Round1()} \n Elapsed time: {elapsed.Round1()} \n Updates: {updates.Round1()} \n Frames: {frames.Round1()}";
                //Console.WriteLine(msg);
                elapsed = 0;
                frames = 0;
                updates = 0;
                last = now;
            }

            updates++;
        }

        internal void DrawFps(Vector2 position, Color color)
        {
            FontManager.GetFont().Write(msg, position, FontBuddyLib.Justify.Left, 1f, color, Screen.SpriteBatch, null);
            frames++;
        }
    }
}