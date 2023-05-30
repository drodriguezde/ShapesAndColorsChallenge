using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Entities;
using ShapesAndColorsChallenge.Class.Management;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class HighlightTile : Entity
    {
        #region CONST

        /// <summary>
        /// Tiempo que tarda en desaparecer el resaltado.
        /// </summary>
        const int FADE_OUT_FRAMES = 30;

        #endregion

        #region PROPERTIES

        int Left { get; set; }

        int Top { get; set; }

        /// <summary>
        /// Indica si hay que mostrar el resaltado de ficha correcta o incorrecta.
        /// </summary>
        bool IsCorrectHighlight { get; set; }

        Vector2 LocationHighligth { get; set; } = Vector2.Zero;

        Rectangle ImageBounds { get; set; }

        internal float Transparency { get; private set; } = 0f;

        Vector2 Scale { get; set; } = Vector2.One;

        #endregion

        #region CONSTRUCTORS

        public HighlightTile()
        {
        }

        internal HighlightTile(Tile tile, bool isCorrect, int left, int top)
        {
            Transparency = 1f;
            ImageBounds = new(tile.Bounds.X, tile.Bounds.Y, tile.Bounds.Width, tile.Bounds.Height);
            IsCorrectHighlight = isCorrect;
            Left = left;
            Top = top;
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {

        }

        internal override void Update(GameTime gameTime)
        {
            if (Transparency >= 0f)
            {
                Rectangle bounds = new(Left + Const.TILE_PADDING, Top + Const.TILE_PADDING, BaseBounds.TileSize.Width - Const.TILE_PADDING.Double(), BaseBounds.TileSize.Width - Const.TILE_PADDING.Double());
                bounds = new(bounds.X.ToInt() - Const.TILE_PADDING - 24, bounds.Y.ToInt() - Const.TILE_PADDING - 24, bounds.Width + Const.TILE_PADDING.Double(), bounds.Height + Const.TILE_PADDING.Double());
                LocationHighligth = bounds.Location.ToVector2();
                float scale = (ImageBounds.Width.ToSingle() + 68) / TextureManager.TextureGreenSquare.Width.ToSingle();
                Scale = new(scale, scale);
                Transparency -= 1f / FADE_OUT_FRAMES;
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            if (Transparency != 0f)
                Screen.SpriteBatch.Draw(
                    IsCorrectHighlight ? TextureManager.TextureGreenSquare : TextureManager.TextureRedSquare,
                    LocationHighligth,
                    null,
                    Color.White * Transparency,
                    0f,
                    Vector2.Zero,
                    Scale,
                    SpriteEffects.None,
                    0f);
        }

        #endregion
    }
}
