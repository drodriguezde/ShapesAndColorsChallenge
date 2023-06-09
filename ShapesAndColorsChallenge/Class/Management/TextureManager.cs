﻿/***********************************************************************
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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.D2;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace ShapesAndColorsChallenge.Class.Management
{
    /// <summary>
    /// Esta clase contiene las texturas de uso habitual, no se destruye a lo largo del juego ya que se usan mucho y no ocupan apenas espacio.
    /// Tambien contiene la colección de texturas creadas "al vuelo" dependiendo de la resolución del dispositivo.
    /// </summary>
    internal static class TextureManager
    {
        #region VARS

        static BackgroundWorker workerLoadAsync;

        /// <summary>
        /// Indica que se están cargan de forma asíncrona.
        /// </summary>
        static bool shapesLoading = false;

        /// <summary>
        /// Indica que se están cargan de forma asíncrona.
        /// </summary>
        static bool perksLoading = false;

        /// <summary>
        /// Indica que se están cargan de forma asíncrona.
        /// </summary>
        static bool animatedStarLoading = false;

        #endregion

        #region PROPERTIES

        static List<CommonTexture> Textures { get; set; } = new List<CommonTexture>();

        internal static Texture2D TextureCheckBoxChecked { get; private set; }
        internal static Texture2D WhitePixel { get; private set; }
        internal static Texture2D BluePixel { get; private set; }
        internal static Texture2D GrayPixel { get; private set; }
        internal static Texture2D TextureCheckBoxUnChecked { get; private set; }
        internal static Texture2D TextureOptionBoxChecked { get; private set; }
        internal static Texture2D TextureOptionBoxUnChecked { get; private set; }
        internal static Texture2D TexturePadLock { get; private set; }
        internal static Texture2D TextureLogo { get; private set; }
        internal static Texture2D TextureStar { get; private set; }
        internal static Texture2D TextureStarMini { get; private set; }
        internal static Texture2D TextureStarGray { get; private set; }
        internal static Texture2D TextureEditIcon { get; private set; }
        internal static Texture2D TextureBackButton { get; private set; }
        internal static Texture2D TextureSettingsButton { get; private set; }
        internal static Texture2D TextureAcheivementsButton { get; private set; }
        internal static Texture2D TexturePauseButton { get; private set; }
        internal static Texture2D TextureOkButton { get; private set; }
        internal static Texture2D TextureCancelButton { get; private set; }
        internal static Texture2D TextureRankingButton { get; private set; }
        internal static Texture2D TextureChallengesButton { get; private set; }
        internal static Texture2D TextureHowToPlayButton { get; private set; }
        internal static Texture2D TexturePerkChange { get; private set; }
        internal static Texture2D TexturePerkReveal { get; private set; }
        internal static Texture2D TexturePerkTimeStop { get; private set; }
        internal static Texture2D TextureGift { get; private set; }
        internal static Texture2D TextureCompleted { get; private set; }
        internal static Texture2D TextureForbidden { get; private set; }
        internal static Texture2D TexturePlay { get; private set; }
        internal static Texture2D TextureRankingGlobal { get; private set; }
        internal static Texture2D TextureRankingLocal { get; private set; }
        internal static Texture2D TextureButtonUpload { get; private set; }
        internal static Texture2D TextureButtonDownload { get; private set; }
        internal static List<Texture2D> MiniShapes { get; private set; } = new();
        internal static Texture2D TextureGreenSquare { get; private set; }
        internal static Texture2D TextureRedSquare { get; private set; }

        static Hashtable LoadedFlags { get; set; } = new();

        #region SHAPES

        internal static Texture2D ShapeAsterisk { get; private set; }
        internal static Texture2D ShapeCircle { get; private set; }
        internal static Texture2D ShapeCross { get; private set; }
        internal static Texture2D ShapeDiamond { get; private set; }
        internal static Texture2D ShapeHeart { get; private set; }
        internal static Texture2D ShapeMoon { get; private set; }
        internal static Texture2D ShapeOval { get; private set; }
        internal static Texture2D ShapePentagon { get; private set; }
        internal static Texture2D ShapeRightTriangle { get; private set; }
        internal static Texture2D ShapeRombus { get; private set; }
        internal static Texture2D ShapeSixStar { get; private set; }
        internal static Texture2D ShapeSquare { get; private set; }
        internal static Texture2D ShapeStar { get; private set; }
        internal static Texture2D ShapeTriangle { get; private set; }

        #endregion

        #region ANIMATED STAR

        internal static List<Texture2D> AnimatedStar { get; private set; } = new();

        #endregion

        #region PERK CHANGE

        internal static List<Texture2D> PerkChange { get; private set; } = new();

        #endregion

        #region PERK REVEAL

        internal static List<Texture2D> PerkReveal { get; private set; } = new();

        #endregion

        #region PERK TIMESTOP

        internal static List<Texture2D> PerkTimeStop { get; private set; } = new();

        #endregion

        #region MODES

        internal static Texture2D ModeBlink { get; private set; }
        internal static Texture2D ModeClassic { get; private set; }
        internal static Texture2D ModeIncremental { get; private set; }
        internal static Texture2D ModeEndless { get; private set; }
        internal static Texture2D ModeMemory { get; private set; }
        internal static Texture2D ModeMove { get; private set; }
        internal static Texture2D ModeRotate { get; private set; }
        internal static Texture2D ModeTimeTrial { get; private set; }

        #endregion

        #endregion

        #region EVENTS

        static void WorkerLoadAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            shapesLoading = true;
            perksLoading = true;
            animatedStarLoading = true;
            LoadShapesAsync();
            LoadAnimatedPerksAsync();
            LoadAnimatedStarAsync();
            LoadFlagsAsync();
        }

        #endregion

        #region METHODS

        internal static void LoadContent()
        {
            LoadAsync();
            LoadTextures();
            LoadMiniShapes();
            LoadOthers();
            LoadModes();
        }

        static void LoadAsync()
        {
            workerLoadAsync = new()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            workerLoadAsync.DoWork += WorkerLoadAsync_DoWork;
            workerLoadAsync.RunWorkerAsync();
        }

        static void LoadModes()
        {
            ModeBlink = GameContent.ContentImage.Load<Texture2D>("Image/ModeBlink");
            ModeClassic = GameContent.ContentImage.Load<Texture2D>("Image/ModeClassic");
            ModeIncremental = GameContent.ContentImage.Load<Texture2D>("Image/ModeIncremental");
            ModeEndless = GameContent.ContentImage.Load<Texture2D>("Image/ModeEndless");
            ModeMemory = GameContent.ContentImage.Load<Texture2D>("Image/ModeMemory");
            ModeMove = GameContent.ContentImage.Load<Texture2D>("Image/ModeMove");
            ModeRotate = GameContent.ContentImage.Load<Texture2D>("Image/ModeRotate");
            ModeTimeTrial = GameContent.ContentImage.Load<Texture2D>("Image/ModeTimeTrial");
        }

        static void LoadTextures()
        {
            TextureCheckBoxChecked = GameContent.ContentImage.Load<Texture2D>("UI/Window/CheckBox/Checked");
            TextureCheckBoxUnChecked = GameContent.ContentImage.Load<Texture2D>("UI/Window/CheckBox/Unchecked");
            TextureOptionBoxChecked = GameContent.ContentImage.Load<Texture2D>("UI/Window/OptionBox/Checked");
            TextureOptionBoxUnChecked = GameContent.ContentImage.Load<Texture2D>("UI/Window/OptionBox/Unchecked");
            TexturePadLock = GameContent.ContentImage.Load<Texture2D>("Image/Padlock");
            TextureLogo = GameContent.ContentImage.Load<Texture2D>("Image/DanLogo");
            TextureStar = GameContent.ContentImage.Load<Texture2D>("Image/Star");
            TextureStarMini = GameContent.ContentImage.Load<Texture2D>("Image/StarMini");
            TextureStarGray = GameContent.ContentImage.Load<Texture2D>("Image/StarGray");
            TextureEditIcon = GameContent.ContentImage.Load<Texture2D>("Image/EditIcon");
            TextureBackButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonBack");
            TextureSettingsButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonSettings");
            TextureAcheivementsButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonAcheivements");
            TexturePauseButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonPause");
            TextureOkButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonOK");
            TextureCancelButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonCancel");
            TextureRankingButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonRanking");
            TextureChallengesButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonChallenges");
            TextureHowToPlayButton = GameContent.ContentImage.Load<Texture2D>("Image/ButtonInformation");
            TexturePerkChange = GameContent.ContentImage.Load<Texture2D>("Image/PerkChange");
            TexturePerkReveal = GameContent.ContentImage.Load<Texture2D>("Image/PerkReveal");
            TexturePerkTimeStop = GameContent.ContentImage.Load<Texture2D>("Image/PerkTimeStop");
            TextureGift = GameContent.ContentImage.Load<Texture2D>("Image/ButtonGift");
            TextureCompleted = GameContent.ContentImage.Load<Texture2D>("Image/Completed");
            TextureForbidden = GameContent.ContentImage.Load<Texture2D>("Image/Forbidden");
            TexturePlay = GameContent.ContentImage.Load<Texture2D>("Image/ButtonPlay");
            TextureRankingGlobal = GameContent.ContentImage.Load<Texture2D>("Image/RankingGlobal");
            TextureRankingLocal = GameContent.ContentImage.Load<Texture2D>("Image/RankingLocal");
            TextureButtonUpload = GameContent.ContentImage.Load<Texture2D>("Image/ButtonUpload");
            TextureButtonDownload = GameContent.ContentImage.Load<Texture2D>("Image/ButtonDownload");
        }

        static void LoadMiniShapes()
        {
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Image/ForbiddenMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeAsteriskMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeCircleMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeCrossMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeDiamondMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeHeartMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeMoonMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeOvalMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapePentagonMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeRightTriangleMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeRombusMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeSixStarMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeSquareMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeStarMini"));
            MiniShapes.Add(GameContent.ContentImage.Load<Texture2D>("Shape/ShapeTriangleMini"));
        }

        /// <summary>
        /// Devuelve una textura a demanda.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static Texture2D GetTexture(string path)
        {
            return GameContent.ContentImage.Load<Texture2D>(path);
        }

        /// <summary>
        /// Llamada externa, si no ha terminado el subproceso de carga interno debe esperar.
        /// </summary>
        internal static void LoadShapes()
        {
            while (shapesLoading)
                Thread.SpinWait(1);
        }

        /// <summary>
        /// Llamada interna por subproceso.
        /// </summary>
        static void LoadShapesAsync()
        {
            ShapeAsterisk = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeAsterisk");
            ShapeCircle = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeCircle");
            ShapeCross = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeCross");
            ShapeDiamond = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeDiamond");
            ShapeHeart = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeHeart");
            ShapeMoon = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeMoon");
            ShapeOval = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeOval");
            ShapePentagon = GameContent.ContentImage.Load<Texture2D>("Shape/ShapePentagon");
            ShapeRightTriangle = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeRightTriangle");
            ShapeRombus = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeRombus");
            ShapeSixStar = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeSixStar");
            ShapeSquare = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeSquare");
            ShapeStar = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeStar");
            ShapeTriangle = GameContent.ContentImage.Load<Texture2D>("Shape/ShapeTriangle");
            TextureGreenSquare = GameContent.ContentImage.Load<Texture2D>("Image/GreenSquare");
            TextureRedSquare = GameContent.ContentImage.Load<Texture2D>("Image/RedSquare");

            shapesLoading = false;
        }

        static void LoadOthers()
        {
            WhitePixel = Set(new Size(1, 1), Color.White);
            BluePixel = Set(new Size(1, 1), Color.Blue);
            GrayPixel = Set(new Size(1, 1), Color.Gray);
        }

        /// <summary>
        /// Llamada externa, si no ha terminado el subproceso de carga interno debe esperar.
        /// </summary>

        internal static void LoadAnimatedStar()
        {
            while (animatedStarLoading)
                Thread.SpinWait(1);
        }

        /// <summary>
        /// Llamada interna por subproceso.
        /// </summary>

        static void LoadAnimatedStarAsync()
        {
            for (int i = 1; i < 7; i++)
                AnimatedStar.Add(GameContent.ContentImage.Load<Texture2D>($"Animation/AnimatedStar/AnimatedStar{i}"));

            animatedStarLoading = false;
        }

        /// <summary>
        /// Llamada externa, si no ha terminado el subproceso de carga interno debe esperar.
        /// </summary>
        internal static Texture2D Flag(string code)
        {
            while (!LoadedFlags.ContainsKey(code))
                Thread.SpinWait(1);

            return LoadedFlags[code] as Texture2D;
        }

        /// <summary>
        /// Llamada interna por subproceso.
        /// </summary>

        static void LoadFlagsAsync()
        {
            foreach (string code in Const.ALL_FLAGS)
                LoadedFlags.Add(code, GameContent.ContentImage.Load<Texture2D>($"UI/Flag/flag_{code}"));
        }

        /// <summary>
        /// Llamada externa, si no ha terminado el subproceso de carga interno debe esperar.
        /// </summary>
        internal static void LoadAnimatedPerks()
        {
            while (perksLoading)
                Thread.SpinWait(1);
        }

        /// <summary>
        /// Llamada interna por subproceso.
        /// </summary>
        static void LoadAnimatedPerksAsync()
        {
            for (int i = 1; i < 21; i++)
                PerkChange.Add(GameContent.ContentImage.Load<Texture2D>($"Animation/PerkChange/Change{i.ToString().PadLeft(2, '0')}"));

            for (int i = 1; i < 21; i++)
                PerkReveal.Add(GameContent.ContentImage.Load<Texture2D>($"Animation/PerkReveal/Reveal{i.ToString().PadLeft(2, '0')}"));

            for (int i = 1; i < 16; i++)
                PerkTimeStop.Add(GameContent.ContentImage.Load<Texture2D>($"Animation/PerkTimeStop/TimeStop{i.ToString().PadLeft(2, '0')}"));

            perksLoading = false;
        }

        internal static Texture2D GetShape(ShapeType type)
        {
            return type switch
            {
                ShapeType.None => TextureForbidden,/*Para los modos plus*/
                ShapeType.Asterisk => ShapeAsterisk,
                ShapeType.Circle => ShapeCircle,
                ShapeType.Cross => ShapeCross,
                ShapeType.Diamond => ShapeDiamond,
                ShapeType.Heart => ShapeHeart,
                ShapeType.Moon => ShapeMoon,
                ShapeType.Oval => ShapeOval,
                ShapeType.Pentagon => ShapePentagon,
                ShapeType.RightTriangle => ShapeRightTriangle,
                ShapeType.Rombus => ShapeRombus,
                ShapeType.SixStar => ShapeSixStar,
                ShapeType.Square => ShapeSquare,
                ShapeType.Star => ShapeStar,
                ShapeType.Triangle => ShapeTriangle,
                _ => null,
            };
        }

        internal static Texture2D GetShapeMini(ShapeType type)
        {
            return MiniShapes[(int)type];
        }

        /// <summary>
        /// Crea una textura de un tipo base.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        static Texture2D Set(Size size, Color innerColor)
        {
            CommonTexture newTexture = new(size, innerColor);
            Deploy(newTexture);
            return newTexture.Texture;
        }

        /// <summary>
        /// Devuelve una textura para un objeto a una determinada resolución, si no existe la crea y la devuelve.
        /// Si la textura ya se ha creado en algún momento la devuelve directamente.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal static CommonTexture Get(Size size, Color color, CommonTextureType commonTextureType)
        {
            foreach (CommonTexture commonTexture in Textures)
                if (size == commonTexture.Size && color == commonTexture.Color && commonTextureType == commonTexture.CommonTextureType)
                    return commonTexture;

            CommonTexture newTexture = new(size, color, color, commonTextureType);
            Deploy(newTexture);
            Textures.Add(newTexture);
            return newTexture;
        }

        /// <summary>
        /// Devuelve una textura para un objeto a una determinada resolución, si no existe la crea y la devuelve.
        /// Si la textura ya se ha creado en algún momento la devuelve directamente.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal static CommonTexture Get(Size size, Color color, Color borderColor, CommonTextureType commonTextureType)
        {
            foreach (CommonTexture commonTexture in Textures)
                if (size == commonTexture.Size && color == commonTexture.Color && commonTexture.BorderColor == borderColor && commonTextureType == commonTexture.CommonTextureType)
                    return commonTexture;

            CommonTexture newTexture = new(size, color, borderColor, commonTextureType);
            Deploy(newTexture);
            Textures.Add(newTexture);
            return newTexture;
        }

        /// <summary>
        /// Crea una textura a petición.
        /// </summary>
        /// <param name="commonTexture"></param>
        static void Deploy(CommonTexture commonTexture)
        {
            switch (commonTexture.CommonTextureType)
            {
                case CommonTextureType.Circle:
                    commonTexture.Texture = ToCircle(commonTexture.Size.Width.Half(), commonTexture.Color, commonTexture.BorderColor);
                    break;
                case CommonTextureType.Rectangle:
                    commonTexture.Texture = ToBorderedRectangle(new Rectangle(0, 0, commonTexture.Size.Width, commonTexture.Size.Height), commonTexture.Color, commonTexture.BorderColor);
                    break;
                case CommonTextureType.RoundedRectangle:
                    commonTexture.Texture = Screen.SpriteBatch.DrawRoundedRectangle(commonTexture.Size.Width, commonTexture.Size.Height, 3, 25, 4, new List<Color>() { commonTexture.Color }, new List<Color>() { commonTexture.BorderColor }, 0.2f, 0.1f);
                    break;
            }
        }

        internal static Texture2D ToBorderedRectangle(Rectangle bounds, Color innerColor, Color borderColor)
        {
            Texture2D rectangle = new(Screen.GraphicsDevice, bounds.Width, bounds.Height);
            Color[] dataColor = new Color[bounds.Width * bounds.Height];

            for (int i = 0; i < dataColor.Length; i++)/*Todo del color interior*/
                dataColor[i] = innerColor;

            for (int i = 0; i < bounds.Width; i++)/*Borde superior*/
                dataColor[i] = borderColor;

            for (int i = dataColor.Length - 1; i > dataColor.Length - 1 - bounds.Width; i--)/*Borde inferior*/
                dataColor[i] = borderColor;

            for (int i = 0; i < dataColor.Length; i += bounds.Width)/*Borde derecho*/
                dataColor[i] = borderColor;

            for (int i = bounds.Width - 1; i < dataColor.Length; i += bounds.Width)/*Borde izquierdo*/
                dataColor[i] = borderColor;

            rectangle.SetData(dataColor);
            return rectangle;
        }

        internal static Texture2D ToCircle(int radius, Color color)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new(Screen.GraphicsDevice, outerRadius, outerRadius);
            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));
                data[y * outerRadius + x + 1] = color;
            }

            texture.SetData(data);
            return texture;
        }

        internal static Texture2D ToCircle(int radius, Color innerColor, Color borderColor)
        {
            int outerRadius = radius * 2 + 2; // So circle doesn't go out of bounds
            Texture2D texture = new(Screen.GraphicsDevice, outerRadius, outerRadius);
            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / radius;

            for (double angle = 0; angle < Math.PI * 2; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates
                int x = (int)Math.Round(radius + radius * Math.Cos(angle));
                int y = (int)Math.Round(radius + radius * Math.Sin(angle));

                data[y * outerRadius + x + 1] = borderColor;
            }

            //width
            for (int i = 0; i < outerRadius; i++)
            {
                int yStart = -1;
                int yEnd = -1;

                //loop through height to find start and end to fill
                for (int j = 0; j < outerRadius; j++)
                {
                    if (yStart == -1)
                    {
                        if (j == outerRadius - 1)
                        {
                            //last row so there is no row below to compare to
                            break;
                        }

                        //start is indicated by Color followed by Transparent
                        if (data[i + (j * outerRadius)] == borderColor && data[i + ((j + 1) * outerRadius)] == Color.Transparent)
                        {
                            yStart = j + 1;
                            continue;
                        }
                    }
                    else if (data[i + (j * outerRadius)] == borderColor)
                    {
                        yEnd = j;
                        break;
                    }
                }

                //if we found a valid start and end position
                if (yStart != -1 && yEnd != -1)
                    //height
                    for (int j = yStart; j < yEnd; j++)
                        data[i + (j * outerRadius)] = innerColor;
            }

            texture.SetData(data);
            return texture;
        }

        internal static Texture2D CreateTexture(int width, int height, Func<int, Color> paint)
        {
            Texture2D texture = new(Screen.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];

            for (int pixel = 0; pixel < data.Length; pixel++)
                data[pixel] = paint(pixel);

            texture.SetData(data);
            return texture;
        }

        #endregion
    }
}