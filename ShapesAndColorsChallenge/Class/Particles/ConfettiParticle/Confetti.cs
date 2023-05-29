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
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;

namespace ShapesAndColorsChallenge.Class.Particles.ConfettiParticle
{
    internal class Confetti
    {
        #region CONST

        const float SPEED_Y = 3f;
        readonly Vector2 ACCELERATION = new(0, 0.05f);

        #endregion

        #region VARS

        List<ConfettiPaper> papers = new();
        float scale = 1f;
        Vector2 startLocation = Vector2.Zero;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Tipo de confeti.
        /// </summary>
        ConfettiType ConfettiType { get; set; }

        /// <summary>
        /// Forma del papel.
        /// </summary>
        ShapeType ShapeType { get; set; }

        /// <summary>
        /// Color del papel.
        /// </summary>
        TileColor TileColor { get; set; }

        /// <summary>
        /// Color del papel.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Área de la pantalla dónde pueden estar.
        /// </summary>
        Rectangle LocationLimits { get; set; }

        /// <summary>
        /// Área de pantalla donde pueden aparecer.
        /// </summary>
        Rectangle StartLocationLimits { get; set; }

        /// <summary>
        /// Indica si se deben generar las formas de los papeles de forma aleatoria.
        /// </summary>
        bool RandomShape { get; set; }

        /// <summary>
        /// Indica si los colores de los papeles son aleatorios.
        /// </summary>
        bool RandomColor { get; set; }

        /// <summary>
        /// Cantidad de papeles del confeti.
        /// </summary>
        int PapersNumber { get; set; }

        /// <summary>
        /// Indica si está activa la particula.
        /// </summary>
        internal bool Running { get; private set; } = true;

        #endregion

        #region CONSTRUCTOR

        /// <summary>
        /// 
        /// </summary>
        /// <param name="confettiType"></param>
        /// <param name="shapeType"></param>
        /// <param name="tileColor"></param>
        /// <param name="locationLimits">Área de la pantalla dónde pueden aparecer</param>
        /// <param name="randomShape"></param>
        /// <param name="randomColor"></param>
        /// <param name="papersNumber"></param>
        internal Confetti(ConfettiType confettiType, ShapeType shapeType, TileColor tileColor, Rectangle startLocationLimits, Rectangle locationLimits, bool randomShape = false, bool randomColor = false, int papersNumber = 20)
        {
            ConfettiType = confettiType;
            ShapeType = shapeType;
            TileColor = tileColor;
            StartLocationLimits = startLocationLimits;
            LocationLimits = locationLimits;
            RandomShape = randomShape;
            RandomColor = randomColor;
            PapersNumber = papersNumber;            
            Initialize();
        }

        #endregion

        #region METHODS

        internal void Initialize()
        {
            if (ConfettiType == ConfettiType.Falling)
                InitializeFalling();
            else
                InitializeFireworks();
        }

        void InitializeFalling()
        { 
            for(int i = 0; i < PapersNumber; i++) 
            {
                SetPaper();
                float speedX = Statics.GetRandom(-10, 10) / 5f;
                Vector2 speed = new(speedX, SPEED_Y);

                papers.Add(new(
                    TextureManager.GetShapeMini(ShapeType),
                    startLocation,
                    ColorManager.GetShapeColor(TileColor),
                    new(scale, scale),
                    speed,
                    ACCELERATION));
            }
        }

        void InitializeFireworks()
        {
            Vector2 location = StartLocationLimits.GetRandomLocation();

            for (int i = 0; i < PapersNumber; i++)
            {
                SetPaper();
                float angle = i + 1;

                papers.Add(new(
                    TextureManager.GetShapeMini(ShapeType),
                    location,
                    ColorManager.GetShapeColor(TileColor),
                    new(scale, scale),
                    new(SPEED_Y * Math.Cos(angle).ToSingle(), SPEED_Y * Math.Sin(angle).ToSingle()),
                    ACCELERATION));
            }
        }

        void SetPaper()
        {
            if (RandomShape)
                ShapeType = (ShapeType)Statics.GetRandom(0, GameData.ShapesNumber(GameData.LEVELS, GameData.STAGES));

            if (RandomColor)
                TileColor = GameData.RandomColor(GameData.LEVELS, GameData.STAGES);

            startLocation = StartLocationLimits.GetRandomLocation();
            scale = Statics.GetRandom(5, 15) / 10f;
        }

        internal void Start()
        {
            Running = true;
        }

        internal void Stop()
        {
            Running = false;                           
        }

        internal void Update(GameTime gameTime)
        {
            if (!Running)
                return;

            bool active = false;

            for (int i = 0; i < papers.Count; i++)
                if (papers[i].Visible && LocationLimits.Contains(papers[i].Bounds))
                {
                    papers[i].Update(gameTime);
                    active = true;
                }
                else
                    papers[i].Visible = false;

            Running = active;/*Si ningún papel está en el área de pantalla se detiene*/
        }

        internal void Draw(GameTime gameTime)
        {
            if (!Running)
                return;

            for (int i = 0; i < papers.Count; i++)
                if(papers[i].Visible)
                    papers[i].Draw(gameTime);
        }

        #endregion
    }
}