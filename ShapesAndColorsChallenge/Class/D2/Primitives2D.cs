/***********************************************************************
* DESCRIPTION :
*https://github.com/DoogeJ/MonoGame.Primitives2D
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.Management;
using System;
using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class.D2
{
    internal static class Primitives2D
    {
        #region DELEGATES



        #endregion

        #region VARS

        static readonly Dictionary<string, List<Vector2>> circleCache = new();
        //private static readonly Dictionary<String, List<Vector2>> arcCache = new Dictionary<string, List<Vector2>>();

        #endregion

        #region METHODS

        #region Private Methods

        /// <summary>
        /// Draws a list of connecting points
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="position">Where to position the points</param>
        /// <param name="points">The points to connect with lines</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the lines</param>
        static void DrawPoints(SpriteBatch spriteBatch, Vector2 position, List<Vector2> points, Color color, float transparency, int thickness)
        {
            if (points.Count < 2)
                return;

            for (int i = 1; i < points.Count; i++)
                DrawLine(
                    spriteBatch,
                    (points[i - 1] + position).ToPoint(),
                    (points[i] + position).ToPoint(),
                    color,
                    transparency,
                    thickness);
        }

        /// <summary>
        /// Creates a list of vectors that represents a circle
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <returns>A list of vectors that, if connected, will create a circle</returns>
        static List<Vector2> CreateCircle(double radius, int sides)
        {
            // Look for a cached version of this circle
            string circleKey = radius + "x" + sides;

            if (circleCache.ContainsKey(circleKey))
                return circleCache[circleKey];

            List<Vector2> vectors = new();
            const double max = 2.0 * Math.PI;
            double step = max / sides;

            for (double theta = 0.0; theta < max; theta += step)
                vectors.Add(new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta))));

            // then add the first vector again so it's a complete loop
            vectors.Add(new Vector2((float)(radius * Math.Cos(0)), (float)(radius * Math.Sin(0))));

            // Cache this circle so that it can be quickly drawn next time
            circleCache.Add(circleKey, vectors);

            return vectors;
        }

        /// <summary>
        /// Creates a list of vectors that represents an arc
        /// </summary>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides to generate in the circle that this will cut out from</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, increasing as you go clockwise</param>
        /// <param name="radians">The radians to draw, clockwise from the starting angle</param>
        /// <returns>A list of vectors that, if connected, will create an arc</returns>
        static List<Vector2> CreateArc(float radius, int sides, float startingAngle, float radians)
        {
            List<Vector2> points = new();
            points.AddRange(CreateCircle(radius, sides));
            points.RemoveAt(points.Count - 1); // remove the last point because it's a duplicate of the first

            // The circle starts at (radius, 0)
            double curAngle = 0.0;
            double anglePerSide = MathHelper.TwoPi / sides;

            // "Rotate" to the starting point
            while ((curAngle + (anglePerSide / 2.0)) < startingAngle)
            {
                curAngle += anglePerSide;

                // move the first point to the end
                points.Add(points[0]);
                points.RemoveAt(0);
            }

            // Add the first point, just in case we make a full circle
            points.Add(points[0]);

            // Now remove the points at the end of the circle to create the arc
            int sidesInArc = (int)((radians / anglePerSide) + 0.5);
            points.RemoveRange(sidesInArc + 1, points.Count - sidesInArc - 1);

            return points;
        }

        #endregion

        #region FillRectangle

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        internal static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            // Simply use the function already there
            spriteBatch.Draw(TextureManager.WhitePixel, rect, color);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="angle">The angle in radians to draw the rectangle at</param>
        internal static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float angle)
        {
            spriteBatch.Draw(TextureManager.WhitePixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        internal static void FillRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color)
        {
            FillRectangle(spriteBatch, location, size, color, 0.0f);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="angle">The angle in radians to draw the rectangle at</param>
        /// <param name="color">The color to draw the rectangle in</param>
        internal static void FillRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color, float angle)
        {
            // stretch the pixel between the two vectors
            spriteBatch.Draw(TextureManager.WhitePixel,
                             location,
                             null,
                             color,
                             angle,
                             Vector2.Zero,
                             size,
                             SpriteEffects.None,
                             0);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x">The X coord of the left side</param>
        /// <param name="y">The Y coord of the upper side</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="color">The color to draw the rectangle in</param>
        internal static void FillRectangle(this SpriteBatch spriteBatch, float x, float y, float w, float h, Color color)
        {
            FillRectangle(spriteBatch, new Vector2(x, y), new Vector2(w, h), color, 0.0f);
        }

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x">The X coord of the left side</param>
        /// <param name="y">The Y coord of the upper side</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="angle">The angle of the rectangle in radians</param>
        internal static void FillRectangle(this SpriteBatch spriteBatch, float x, float y, float w, float h, Color color, float angle)
        {
            FillRectangle(spriteBatch, new Vector2(x, y), new Vector2(w, h), color, angle);
        }

        #endregion

        #region DrawRectangle

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        internal static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float transparency)
        {
            DrawRectangle(spriteBatch, rect, color, transparency, 1);
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the lines</param>
        internal static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float transparency, int thickness)
        {
            // TODO: Handle rotations
            // TODO: Figure out the pattern for the offsets required and then handle it in the line instead of here

            DrawLine(spriteBatch, new Point(rect.X, rect.Y), new Point(rect.Right, rect.Y), color, transparency, thickness); // top
            DrawLine(spriteBatch, new Point(rect.X + 1, rect.Y), new Point(rect.X + 1, rect.Bottom + thickness), color, transparency, thickness); // left
            DrawLine(spriteBatch, new Point(rect.X, rect.Bottom), new Point(rect.Right, rect.Bottom), color, transparency, thickness); // bottom
            DrawLine(spriteBatch, new Point(rect.Right + 1, rect.Y), new Point(rect.Right + 1, rect.Bottom + thickness), color, transparency, thickness); // right
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        internal static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 location, Point size, Color color)
        {
            DrawRectangle(spriteBatch, new Rectangle((int)location.X, (int)location.Y, size.X, size.Y), color, 1);
        }

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the line</param>
        internal static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 location, Point size, Color color, int thickness)
        {
            DrawRectangle(spriteBatch, new Rectangle((int)location.X, (int)location.Y, size.X, size.Y), color, thickness);
        }

        #endregion

        #region DrawLine

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        internal static void DrawLine(this SpriteBatch spriteBatch, Line line, Color color, int thickness)
        {
            spriteBatch.Draw(TextureManager.WhitePixel,
                new Rectangle(
                    line.P1X,
                    line.P1Y,
                    line.Distance,
                    thickness), null, color, line.Angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point">The starting point</param>
        /// <param name="length">The length of the line</param>
        /// <param name="angle">The angle of this line from the starting point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        internal static void DrawLine(this SpriteBatch spriteBatch, Vector2 v1, Vector2 v2, Color color, int thickness)
        {
            spriteBatch.Draw(TextureManager.WhitePixel,
                new Rectangle(v1.X.ToInt(), v1.Y.ToInt(), Vector2.Distance(v1, v2).ToInt(), thickness),
                null, color, Statics.GetAngleLines(v1, v2), Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point">The starting point</param>
        /// <param name="length">The length of the line</param>
        /// <param name="angle">The angle of this line from the starting point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        internal static void DrawLine(this SpriteBatch spriteBatch, Line line, Color color, float transparency, int thickness)
        {
            spriteBatch.Draw(TextureManager.WhitePixel,
                new Rectangle(
                    line.P1X,
                    line.P1Y,
                    line.Distance,
                    thickness), null, color * transparency, line.Angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point">The starting point</param>
        /// <param name="length">The length of the line</param>
        /// <param name="angle">The angle of this line from the starting point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        internal static void DrawLine(this SpriteBatch spriteBatch, int x1, int y1, int x2, int y2, Color color, float transparency, int thickness)
        {
            spriteBatch.Draw(TextureManager.WhitePixel,
                new Rectangle(
                    x1,
                    y1,
                    Statics.GetDistance(x1, x2, y1, y2),
                    thickness), null, color * transparency, Statics.GetAngleLines(x1, x2, y1, y2), Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a line from point1 to point2 with an offset
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point">The starting point</param>
        /// <param name="length">The length of the line</param>
        /// <param name="angle">The angle of this line from the starting point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        internal static void DrawLine(this SpriteBatch spriteBatch, Point p1, Point p2, Color color, float transparency, int thickness)
        {
            spriteBatch.Draw(TextureManager.WhitePixel,
                new Rectangle(
                    p1.X,
                    p1.Y,
                    Statics.GetDistance(p1.X, p2.X, p1.Y, p2.Y),
                    thickness), null, color * transparency, Statics.GetAngleLines(p1.X, p2.X, p1.Y, p2.Y), Vector2.Zero, SpriteEffects.None, 0);
        }

        #endregion

        #region PutPixel

        internal static void PutPixel(this SpriteBatch spriteBatch, float x, float y, Color color)
        {
            PutPixel(spriteBatch, new Vector2(x, y), color);
        }

        internal static void PutPixel(this SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            spriteBatch.Draw(TextureManager.WhitePixel, position, color);
        }

        #endregion

        #region DrawCircle

        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="color">The color of the circle</param>
        internal static void DrawCircle(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Color color, float transparency)
        {
            DrawPoints(spriteBatch, center, CreateCircle(radius, sides), color, transparency, 1);
        }

        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="color">The color of the circle</param>
        /// <param name="thickness">The thickness of the lines used</param>
        internal static void DrawCircle(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, Color color, float transparency, int thickness)
        {
            DrawPoints(spriteBatch, center, CreateCircle(radius, sides), color, transparency, thickness);
        }

        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x">The center X of the circle</param>
        /// <param name="y">The center Y of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="color">The color of the circle</param>
        internal static void DrawCircle(this SpriteBatch spriteBatch, float x, float y, float radius, int sides, Color color, float transparency)
        {
            DrawPoints(spriteBatch, new Vector2(x, y), CreateCircle(radius, sides), color, transparency, 1);
        }

        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x">The center X of the circle</param>
        /// <param name="y">The center Y of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="color">The color of the circle</param>
        /// <param name="thickness">The thickness of the lines used</param>
        internal static void DrawCircle(this SpriteBatch spriteBatch, float x, float y, float radius, int sides, Color color, float transparency, int thickness)
        {
            DrawPoints(spriteBatch, new Vector2(x, y), CreateCircle(radius, sides), color, transparency, thickness);
        }

        #endregion

        #region DrawArc

        /// <summary>
        /// Draw a arc
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the arc</param>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, increasing as you go clockwise</param>
        /// <param name="radians">The number of radians to draw, clockwise from the starting angle</param>
        /// <param name="color">The color of the arc</param>
        internal static void DrawArc(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float startingAngle, float radians, Color color, float transparency)
        {
            DrawArc(spriteBatch, center, radius, sides, startingAngle, radians, color, transparency, 1);
        }

        /// <summary>
        /// Draw a arc
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the arc</param>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, increasing as you go clockwise</param>
        /// <param name="radians">The number of radians to draw, clockwise from the starting angle</param>
        /// <param name="color">The color of the arc</param>
        /// <param name="thickness">The thickness of the arc</param>
        internal static void DrawArc(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides, float startingAngle, float radians, Color color, float transparency, int thickness)
        {
            List<Vector2> arc = CreateArc(radius, sides, startingAngle, radians);
            //List<Vector2> arc = CreateArc2(radius, sides, startingAngle, degrees);
            DrawPoints(spriteBatch, center, arc, color, transparency, thickness);
        }

        #endregion

        #region RoundedRectangle

        public static Texture2D DrawRoundedRectangle(this SpriteBatch spriteBatch, int width, int height, int borderThickness, int borderRadius, int borderShadow, List<Color> backgroundColors, List<Color> borderColors, float initialShadowIntensity, float finalShadowIntensity)
        {
            if (backgroundColors == null || backgroundColors.Count == 0) throw new ArgumentException("Must define at least one background color (up to four).");
            if (borderColors == null || borderColors.Count == 0) throw new ArgumentException("Must define at least one border color (up to three).");
            if (borderRadius < 1) throw new ArgumentException("Must define a border radius (rounds off edges).");
            if (borderThickness < 1) throw new ArgumentException("Must define border thikness.");
            if (borderThickness + borderRadius > height / 2 || borderThickness + borderRadius > width / 2) throw new ArgumentException("Border will be too thick and/or rounded to fit on the texture.");
            if (borderShadow > borderRadius) throw new ArgumentException("Border shadow must be lesser in magnitude than the border radius (suggeted: shadow <= 0.25 * radius).");

            Texture2D texture = new(spriteBatch.GraphicsDevice, width, height, false, SurfaceFormat.Color);
            Color[] color = new Color[width * height];

            for (int x = 0; x < texture.Width; x++)
            {
                for (int y = 0; y < texture.Height; y++)
                {
                    switch (backgroundColors.Count)
                    {
                        case 4:
                            Color leftColor0 = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)y / (width - 1)));
                            Color rightColor0 = Color.Lerp(backgroundColors[2], backgroundColors[3], ((float)y / (height - 1)));
                            color[x + width * y] = Color.Lerp(leftColor0, rightColor0, ((float)x / (width - 1)));
                            break;
                        case 3:
                            Color leftColor1 = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)y / (width - 1)));
                            Color rightColor1 = Color.Lerp(backgroundColors[1], backgroundColors[2], ((float)y / (height - 1)));
                            color[x + width * y] = Color.Lerp(leftColor1, rightColor1, ((float)x / (width - 1)));
                            break;
                        case 2:
                            color[x + width * y] = Color.Lerp(backgroundColors[0], backgroundColors[1], ((float)x / (width - 1)));
                            break;
                        default:
                            color[x + width * y] = backgroundColors[0];
                            break;
                    }

                    color[x + width * y] = ColorBorder(x, y, width, height, borderThickness, borderRadius, borderShadow, color[x + width * y], borderColors, initialShadowIntensity, finalShadowIntensity);
                }
            }

            texture.SetData(color);
            return texture;
        }

        private static Color ColorBorder(int x, int y, int width, int height, int borderThickness, int borderRadius, int borderShadow, Color initialColor, List<Color> borderColors, float initialShadowIntensity, float finalShadowIntensity)
        {
            Rectangle internalRectangle = new((borderThickness + borderRadius), (borderThickness + borderRadius), width - 2 * (borderThickness + borderRadius), height - 2 * (borderThickness + borderRadius));

            if (internalRectangle.Contains(x, y)) return initialColor;

            Vector2 origin = Vector2.Zero;
            Vector2 point = new(x, y);

            if (x < borderThickness + borderRadius)
            {
                if (y < borderRadius + borderThickness)
                    origin = new(borderRadius + borderThickness, borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new(borderRadius + borderThickness, height - (borderRadius + borderThickness));
                else
                    origin = new(borderRadius + borderThickness, y);
            }
            else if (x > width - (borderRadius + borderThickness))
            {
                if (y < borderRadius + borderThickness)
                    origin = new(width - (borderRadius + borderThickness), borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new(width - (borderRadius + borderThickness), height - (borderRadius + borderThickness));
                else
                    origin = new(width - (borderRadius + borderThickness), y);
            }
            else
            {
                if (y < borderRadius + borderThickness)
                    origin = new(x, borderRadius + borderThickness);
                else if (y > height - (borderRadius + borderThickness))
                    origin = new(x, height - (borderRadius + borderThickness));
            }

            if (!origin.Equals(Vector2.Zero))
            {
                float distance = Vector2.Distance(point, origin);

                if (distance > borderRadius + borderThickness + 1)
                {
                    return Color.Transparent;
                }
                else if (distance > borderRadius + 1)
                {
                    if (borderColors.Count > 2)
                    {
                        float modNum = distance - borderRadius;

                        return modNum < borderThickness / 2
                            ? Color.Lerp(borderColors[2], borderColors[1], (float)((modNum) / (borderThickness / 2.0)))
                            : Color.Lerp(borderColors[1], borderColors[0], (float)((modNum - (borderThickness / 2.0)) / (borderThickness / 2.0)));
                    }

                    if (borderColors.Count > 0)
                        return borderColors[0];
                }
                else if (distance > borderRadius - borderShadow + 1)
                {
                    float mod = (distance - (borderRadius - borderShadow)) / borderShadow;
                    float shadowDiff = initialShadowIntensity - finalShadowIntensity;
                    return DarkenColor(initialColor, ((shadowDiff * mod) + finalShadowIntensity));
                }
            }

            return initialColor;
        }

        private static Color DarkenColor(Color color, float shadowIntensity)
        {
            return Color.Lerp(color, Color.Black, shadowIntensity);
        }

        #endregion

        #endregion
    }

}