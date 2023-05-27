using Microsoft.Xna.Framework;
using ShapesAndColorsChallenge.Class.D2;
using ShapesAndColorsChallenge.Class.Entities;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class Grid : Entity, IDisposable
    {
        #region CONST

        const int MAX_GRID_WIDTH = 980;
        const int MAX_GRID_HEIGHT = 1120;

        /// <summary>
        /// Padding de la imagen de la ficha con respecto a su contenedor.
        /// </summary>
        const int TILE_PADDING = 10;

        /// <summary>
        /// Indica donde se empezarán a colocar las fichas por arriba.
        /// </summary>
        const int TopLimit = 730;/*No hay que hacer redim aquí*/

        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS



        #endregion

        #region PROPERTIES

        /// <summary>
        /// Gestor de objetos de la ventana Game.
        /// </summary>
        InteractiveObjectManager InteractiveObjectManager { get { return WindowGame.InteractiveObjectManager; } }

        WindowGame WindowGame { get; set; }

        /// <summary>
        /// Punto desde el que se comenzarán a pintar las fichas en la pantalla.
        /// </summary>
        Vector2 DrawStartLocation { get; set; } = Vector2.Zero;

        /// <summary>
        /// Tamaño de ficha.
        /// </summary>
        int TileSize { get; set; } = BaseBounds.TileSize.Width;

        GameMode GameMode { get; set; }

        /// <summary>
        /// Etapa del juego, se modificará para el modo incremental.
        /// </summary>
        int Stage { get; set; }

        /// <summary>
        /// Nivel del juego, se modificará para el modo incremental.
        /// </summary>
        int Level { get; set; }

        /// <summary>
        /// Fichas de la parrilla.
        /// </summary>
        internal List<Tile> Tiles = new();

        #endregion

        #region CONSTRUCTORS

        internal Grid(WindowGame windowGame, ModalLevel modalLevel) : base(modalLevel)
        {
            WindowGame = windowGame;
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
        internal new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos no administrados que utiliza, y libera los recursos administrados de forma opcional.
        /// </summary>
        /// <param name="disposing">True si se quiere liberar los recursos administrados.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            /*Objetos administrados aquí*/
            if (disposing)
            {
                UnsubscribeEvents();
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Grid()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            for (int i = 0; i < Tiles.Count; i++)
                Tiles[i].OnClick -= WindowGame.Tile_OnClick;
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {

        }

        internal void Set(GameMode gameMode, int stage, int level)
        {
            GameMode = gameMode;
            Stage = stage;
            Level = level;
            Clear();/*Para el modo de juego incremental es necesario hacer un clear previo.*/
            SetTiles();
        }

        void Clear()
        {
            UnsubscribeEvents();

            for (int i = 0; i < Tiles.Count; i++)
                InteractiveObjectManager.Remove(Tiles[i].Image.ID);

            Tiles.Clear();
        }

        /// <summary>
        /// Establece la apariencia del grid la primera vez.
        /// </summary>
        void SetTiles()
        {
            TileSize = GetTileSize();
            int horizontalTiles = GameData.HorizontalTilesNumber(Stage, Level);
            int verticalTiles = GameData.VerticalTilesNumber(Stage, Level);
            int positionPlusTile = Statics.GetRandom(0, horizontalTiles * verticalTiles - 1);/*Indica el lugar dónde se añadirá la ficha del modo plus "Ficha no presente.", en los modos plus puede que la ficha maestra no esté por lo que hay que añadir está ficha siempre*/
            int positionCurrentTile = 0;

            for (int h = 0; h < horizontalTiles; h++)
                for (int v = 0; v < verticalTiles; v++)
                {
                    (ShapeType shapeType, TileColor tileColor) = GetColorAndShape(positionPlusTile, positionCurrentTile);                    
                    Rectangle bounds = new Rectangle(GetTileLocationLeft(h) + TILE_PADDING, GetTileLocationTop(v) + TILE_PADDING, TileSize - TILE_PADDING.Double(), TileSize - TILE_PADDING.Double()).Redim();
                    Tile tile = new(ModalLevel, bounds, h, v, tileColor, shapeType) { AllowClickWhenNotVisible = true };
                    Image imageTile = new(ModalLevel, bounds, TextureManager.GetShape(shapeType), ColorManager.GetShapeColor(tileColor), true) { EnableOnClick = false };
                    tile.OnClick += WindowGame.Tile_OnClick;
                    Tiles.Add(tile);
                    InteractiveObjectManager.Add(tile.Image);
                    positionCurrentTile++;
                }

            DrawStartLocation = new(GetTileLocationLeft(0), GetTileLocationTop(0));
        }

        /// <summary>
        /// Oculta todas las fichas.
        /// Se usa en el modo memoria.
        /// </summary>
        internal void HideTiles()
        {
            Tiles.ForEach(t => t.Image.Visible = false);
        }

        /// <summary>
        /// Muestra todas las fichas.
        /// Se usa en el modo memoria.
        /// </summary>
        internal void ShowTiles()
        {
            Tiles.ForEach(t => t.Image.Visible = true);
        }

        /// <summary>
        /// Deshabilita el click de las fichas.
        /// Se usa en el modo memoria.
        /// </summary>
        internal void DisableTilesClick()
        {
            Tiles.ForEach(t => t.EnableOnClick = false);
        }

        /// <summary>
        /// Habilita el click de las fichas.
        /// Se usa en el modo memoria.
        /// </summary>
        internal void EnableTilesClick()
        {
            Tiles.ForEach(t => t.EnableOnClick = true);
        }

        /// <summary>
        /// No se puede dar que haya dos fichas con el mismo color/forma
        /// </summary>
        /// <returns></returns>
        (ShapeType, TileColor) GetColorAndShape(int positionPlusTile, int positionCurrentTile)
        {
            if (GameMode.IsPlus() && positionCurrentTile == positionPlusTile)
                return (ShapeType.None, TileColor.Red);

            while (true)
            {
                ShapeType shapeType = GameData.RandomShape(Stage, Level, OrchestratorManager.GameMode);
                TileColor tileColor = GameData.RandomColor(Stage, Level);

                if (!IsTileInMe(shapeType, tileColor))
                    return (shapeType, tileColor);
            }
        }

        /// <summary>
        /// Indica si una ficha se encontra en el grid.
        /// </summary>
        /// <returns></returns>
        internal bool IsTileInMe(Tile tile)
        {
            return IsTileInMe(tile.ShapeType, tile.TileColor);
        }

        /// <summary>
        /// Indica si una ficha se encontra en el grid.
        /// </summary>
        /// <returns></returns>
        internal bool IsTileInMe(ShapeType shapeType, TileColor tileColor)
        {
            return Tiles.Any(t => t.ShapeType == shapeType && t.TileColor == tileColor);
        }

        /// <summary>
        /// Devuelve la posición con respecto a la izquierda de la ficha que ocupa el lugar indicado contando desde la izquierda.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int GetTileLocationLeft(int index)
        {
            return BaseBounds.Bounds.Width.Half() - (GameData.HorizontalTilesNumber(Stage, Level) * TileSize).Half() + index * TileSize;
        }

        /// <summary>
        /// Devuelve la posición con respecto a arriba de la ficha que ocupa el lugar indicado contando desde arriba.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        int GetTileLocationTop(int index)
        {
            return GetTopLimit() + index * TileSize;
        }

        /// <summary>
        /// Deveulve el límite por arriba en que se puede comenzar a pintar la parrilla.
        /// </summary>
        /// <returns></returns>
        int GetTopLimit()
        {
            int top = BaseBounds.Bounds.Height.Half() - (GameData.VerticalTilesNumber(Stage, Level) * TileSize).Half();

            return top < TopLimit ? TopLimit : top;
        }

        /// <summary>
        /// El tamaño de la ficha en la parrilla va a depender de cuantas fichas deben aparecer en su ancho.
        /// </summary>
        /// <returns></returns>
        int GetTileSize()
        {
            return BaseBounds.TileSize.Width.Min(MAX_GRID_HEIGHT / GameData.VerticalTilesNumber(Stage, Level), MAX_GRID_WIDTH / GameData.HorizontalTilesNumber(Stage, Level));
        }

        internal void Enable()
        { 
            for(int i = 0; i < Tiles.Count; i++)
                Tiles[i].Active = true;
        }

        internal void Disable()
        {
            for (int i = 0; i < Tiles.Count; i++)
                Tiles[i].Active = false;
        }

        /// <summary>
        /// Obtiene la ubicación de la figura que hay que buscar.
        /// Si la figura está girando en pantalla hay que ajustar el origen.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal Rectangle GetThisTileLocation(Tile tile, int offset)
        {
            Rectangle tileLocation = Rectangle.Empty;

            for(int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i].ShapeType == tile.ShapeType && Tiles[i].TileColor == tile.TileColor)
                {
                    Rectangle bounds = new Rectangle(GetTileLocationLeft(Tiles[i].HorizontalLocation) + TILE_PADDING, GetTileLocationTop(Tiles[i].VerticalLocation) + TILE_PADDING, TileSize - TILE_PADDING.Double(), TileSize - TILE_PADDING.Double()).Redim();
                    return new(bounds.X.ToInt() - TILE_PADDING + offset, bounds.Y.ToInt() - TILE_PADDING - offset.Half(), bounds.Width + TILE_PADDING.Double(), bounds.Height + TILE_PADDING.Double());
                }

                if (Tiles[i].ShapeType == ShapeType.None)/*Se devolverá la última en caso de no haber otra.*/
                {
                    Rectangle bounds = new Rectangle(GetTileLocationLeft(Tiles[i].HorizontalLocation) + TILE_PADDING, GetTileLocationTop(Tiles[i].VerticalLocation) + TILE_PADDING, TileSize - TILE_PADDING.Double(), TileSize - TILE_PADDING.Double()).Redim();
                    tileLocation = new(bounds.X.ToInt() - TILE_PADDING + offset, bounds.Y.ToInt() - TILE_PADDING - offset.Half(), bounds.Width + TILE_PADDING.Double(), bounds.Height + TILE_PADDING.Double());
                }
            }

            return tileLocation;
        }

        internal override void Draw(GameTime gameTime)
        {
            for (int i = 0; i <= GameData.VerticalTilesNumber(Stage, Level); i++)
                Screen.SpriteBatch.DrawLine(
                    new Vector2(DrawStartLocation.X, DrawStartLocation.Y + i * TileSize).Redim(),
                    new Vector2(DrawStartLocation.X + GameData.HorizontalTilesNumber(Stage, Level) * TileSize, DrawStartLocation.Y + i * TileSize).Redim(),
                    ColorManager.VeryHardGray, 1);

            for (int i = 0; i <= GameData.HorizontalTilesNumber(Stage, Level); i++)
                Screen.SpriteBatch.DrawLine(
                    new Vector2(DrawStartLocation.X + i * TileSize, DrawStartLocation.Y).Redim(),
                    new Vector2(DrawStartLocation.X + i * TileSize, DrawStartLocation.Y + GameData.VerticalTilesNumber(Stage, Level) * TileSize).Redim(),
                    ColorManager.VeryHardGray, 1);
        }

        internal override void Update(GameTime gameTime)
        {
            
        }

        #endregion
    }
}
