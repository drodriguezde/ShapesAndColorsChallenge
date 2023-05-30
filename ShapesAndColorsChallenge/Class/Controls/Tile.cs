using Microsoft.Xna.Framework;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class Tile : InteractiveObject
    {
        #region PROPERTIES

        internal int HorizontalLocation { get; private set; }
        internal int VerticalLocation { get; private set; }
        internal TileColor TileColor { get; private set; } = TileColor.None;
        internal ShapeType ShapeType { get; private set; } = ShapeType.None;

        /// <summary>
        /// Indica si la ficha está rellena o no.
        /// </summary>
        internal bool FilledShape { get; private set; }

        /// <summary>
        /// Imagen asociada a la ficha.
        /// </summary>
        internal Image Image { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal Tile() : base(ModalLevel.None, Rectangle.Empty)
        {
            Transparency = 0;
        }

        internal Tile(ModalLevel modalLevel, Rectangle bounds, TileColor tileColor, ShapeType shapeType) : base(modalLevel, bounds)
        {
            Transparency = 0;
            TileColor = tileColor;
            ShapeType = shapeType;
            SetImage(bounds, tileColor, shapeType);
        }

        internal Tile(ModalLevel modalLevel, Rectangle bounds, int horizontalLocation, int verticalLocation, TileColor tileColor, ShapeType shapeType) : base(modalLevel, bounds)
        {
            Transparency = 0;
            HorizontalLocation = horizontalLocation;
            VerticalLocation = verticalLocation;
            TileColor = tileColor;
            ShapeType = shapeType;
            SetImage(bounds, tileColor, shapeType);
        }

        #endregion

        #region METHODS

        void SetImage(Rectangle bounds, TileColor tileColor, ShapeType shapeType)
        {
            Image = new(ModalLevel, bounds, TextureManager.GetShape(shapeType), ColorManager.GetShapeColor(tileColor), true) { EnableOnClick = false };
        }

        #endregion
    }
}
