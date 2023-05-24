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
using ShapesAndColorsChallenge.Class.Animated;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowLevel : Window, IDisposable
    {
        #region CONST

        readonly int SEPARATOR_TITLE_LINE = 5.RedimY();
        const int TOTAL_STARS_TOP = 250;/*Es para una imagen, no necesita Redim*/

        /// <summary>
        /// Rotación de las estrellas en grados.
        /// </summary>
        const float ROTATION_STAR = 25f;

        #endregion

        #region VARS        

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        readonly Button[] buttonLevel = new Button[12];
        readonly Image[] imageLock = new Image[12];
        AnimationShake shakeAnimation;
        readonly List<Score> stageScore = ControllerScore.Get(OrchestratorManager.GameMode, OrchestratorManager.StageNumber);

        readonly Rectangle[] buttonLevelBounds = new Rectangle[12]
        {
            new Rectangle(50, 400, 465, 210).Redim(),
            new Rectangle(565, 400, 465, 210).Redim(),
            new Rectangle(50, 660, 465, 210).Redim(),
            new Rectangle(565, 660, 465, 210).Redim(),
            new Rectangle(50, 920, 465, 210).Redim(),
            new Rectangle(565, 920, 465, 210).Redim(),
            new Rectangle(50, 1180, 465, 210).Redim(),
            new Rectangle(565, 1180, 465, 210).Redim(),
            new Rectangle(50, 1440, 465, 210).Redim(),
            new Rectangle(565, 1440, 465, 210).Redim(),
            new Rectangle(50, 1700, 465, 210).Redim(),
            new Rectangle(565, 1700, 465, 210).Redim()
        };

        readonly Rectangle[] labelLevelBounds = new Rectangle[12]
        {
            new Rectangle(60, 405, 445, 90).Redim(),
            new Rectangle(575, 405, 445, 90).Redim(),
            new Rectangle(60, 665, 445, 90).Redim(),
            new Rectangle(575, 665, 445, 90).Redim(),
            new Rectangle(60, 925, 445, 90).Redim(),
            new Rectangle(575, 925, 445, 90).Redim(),
            new Rectangle(60, 1185, 445, 90).Redim(),
            new Rectangle(575, 1185, 445, 90).Redim(),
            new Rectangle(60, 1445, 445, 90).Redim(),
            new Rectangle(575, 1445, 445, 90).Redim(),
            new Rectangle(60, 1705, 445, 90).Redim(),
            new Rectangle(575, 1705, 445, 90).Redim()
        };

        readonly Rectangle[] starsBounds = new Rectangle[12]
        {
            new Rectangle(70, 510, 425, 80),
            new Rectangle(585, 510, 425, 80),
            new Rectangle(70, 770, 425, 80),
            new Rectangle(585, 770, 425, 80),
            new Rectangle(70, 1030, 425, 80),
            new Rectangle(585, 1030, 425, 80),
            new Rectangle(70, 1290, 425, 80),
            new Rectangle(585, 1290, 425, 80),
            new Rectangle(70, 1550, 425, 80),
            new Rectangle(585, 1550, 425, 80),
            new Rectangle(70, 1810, 425, 80),
            new Rectangle(585, 1810, 425, 80)
        };
       
        #endregion

        #region CONSTRUCTORS

        internal WindowLevel()
            : base(ModalLevel.Window, BaseBounds.Bounds.Redim(), WindowType.Level)
        {

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
        ~WindowLevel()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            for (int i = 0; i < GameData.LEVELS; i++)
                buttonLevel[i].OnClick += ButtonStage_OnClick;
        }

        void UnsubscribeEvents()
        {
            for (int i = 0; i < GameData.LEVELS; i++)
                buttonLevel[i].OnClick -= ButtonStage_OnClick;
        }

        void ButtonStage_OnClick(object sender, OnClickEventArgs e)
        {
            int level = (int)(sender as Button).Tag[0];
            CheckLockedLevel(level);
        }

        void ButtonHowToPlay_OnClick(object sender, OnClickEventArgs e)
        {

        }

        void ButtonChallenge_OnClick(object sender, OnClickEventArgs e)
        {
            CloseMeAndOpenThis(WindowType.Challenges);
        }

        void ButtonRanking_OnClick(object sender, OnClickEventArgs e)
        {
            CloseMeAndOpenThis(WindowType.Rankings);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SetTitle();
            SetStages();
            SetTotalStars();
            SubscribeEvents();
        }

        void SetTitle()
        {
            Label labelTitle = new(ModalLevel, BaseBounds.Title, Statics.GetGameModeTitle(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelTitle);
        }

        void SetStages()
        {
            for(int i = 0; i < GameData.LEVELS; i++)
                SetLevel(i);
        }

        void SetTotalStars()
        {
            int currentStars = stageScore.Sum(t => t.Stars);
            Rectangle bounds = new(BaseBounds.Bounds.Width.Half() - 80, TOTAL_STARS_TOP, 80, 80);
            Image imageStar = new(ModalLevel, bounds, TextureManager.TextureStar, true);
            Label labelStars = new(ModalLevel, new Rectangle(bounds.Right + 10, TOTAL_STARS_TOP - 5, 100, 100).Redim(), currentStars.ToString(), Color.GreenYellow, Color.GreenYellow, AlignHorizontal.Left);
            InteractiveObjectManager.Add(imageStar, labelStars);
        }

        /// <summary>
        /// Establece los botones de las etapas.
        /// </summary>
        void SetLevel(int index)
        {
            buttonLevel[index] = new Button(ModalLevel, buttonLevelBounds[index]) { Tag = new() { index + 1 } };
            Label label = new(ModalLevel, labelLevelBounds[index], $"{GetLevelStageNumber(index + 1)}", ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            Line line = new(ModalLevel, new Point(labelLevelBounds[index].Left + SEPARATOR_TITLE_LINE.Double(), labelLevelBounds[index].Bottom - SEPARATOR_TITLE_LINE), new Point(labelLevelBounds[index].Right - SEPARATOR_TITLE_LINE.Double(), labelLevelBounds[index].Bottom - SEPARATOR_TITLE_LINE), ColorManager.HardGray, ColorManager.HardGray, 1);
            InteractiveObjectManager.Add(buttonLevel[index], label, line);
            SetStarsPadLock(index);
        }

        static int GetLevelStageNumber(int level)
        {
            return (OrchestratorManager.StageNumber - 1) * GameData.LEVELS + level;
        }

        void SetStarsPadLock(int index)
        {
            if (IsLocked(index + 1))/*Muestra la imagen de bloqueado*/
            {
                Rectangle lockBounds = new(
                    starsBounds[index].X + starsBounds[index].Width.Half() - TextureManager.TexturePadLock.Width.Half() + 50,
                    starsBounds[index].Y + starsBounds[index].Height.Half() - TextureManager.TexturePadLock.Height.Half() + 50,
                    100,
                    100);
                imageLock[index] = new Image(ModalLevel, lockBounds, TextureManager.TexturePadLock, true);/*Las imagenes no tienen Redim*/
                InteractiveObjectManager.Add(imageLock[index]);
            }
            else/*Muestra la imagen de la estrellas y su contador*/
                SetStars(index);
        }

        /// <summary>
        /// Comprueba si un nivel está bloqueado.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool IsLocked(int level)
        {
            if (level == 1)/*El primer nivel nunca está bloqueado*/
                return false;

            int currentStarsStage = stageScore.Sum(t => t.Stars);
            return currentStarsStage < GameData.StarsToUnlockLevel(level);
        }

        /// <summary>
        /// Pone la cantidad de estrellas obtenidas para la fase en cuestión.
        /// </summary>
        void SetStars(int index)
        {
            Image[] image = new Image[3];
            int currentStarsLevel = stageScore.Single(t => t.LevelNumber == index + 1).Stars;
            Vector2 origin = TextureManager.TextureStar.Bounds.Size.ToVector2() / 2f;

            Rectangle rectangleImage = new(
                starsBounds[index].X + 88, 
                starsBounds[index].Y + 40, 
                80, 
                80);
            image[0] = new Image(ModalLevel, rectangleImage, currentStarsLevel > 0 ? TextureManager.TextureStar : TextureManager.TextureStarGray, -ROTATION_STAR.ToRadians(), origin, true);

            rectangleImage = new Rectangle(
                starsBounds[index].X + starsBounds[index].Width.Half() - 100.Half(),
                starsBounds[index].Y - 10,
                100,
                100);
            image[1] = new Image(ModalLevel, rectangleImage, currentStarsLevel > 2 ? TextureManager.TextureStar : TextureManager.TextureStarGray, true);

            rectangleImage = new Rectangle(
                starsBounds[index].X + starsBounds[index].Width.Half() + 125,
                starsBounds[index].Y + 40,
                80,
                80);
            image[2] = new Image(ModalLevel, rectangleImage, currentStarsLevel > 1 ? TextureManager.TextureStar : TextureManager.TextureStarGray, ROTATION_STAR.ToRadians(), origin, true);

            InteractiveObjectManager.Add(image[0], image[1], image[2]);
        }

        /// <summary>
        /// Comprueba si un nivel está bloqueado para mostrar al usuario información o permitirle continuar.
        /// </summary>
        void CheckLockedLevel(int level)
        {
            if (IsLocked(level))/*Está bloqueado*/
            {
                shakeAnimation?.Stop();
                shakeAnimation = new AnimationShake(GetImagePadlockByLevel(level), 700);
                shakeAnimation.Start();
                Statics.ShowToast(Resource.String.LEVEL_LOCKED.GetString(GameData.StarsToUnlockLevel(level)), Android.Widget.ToastLength.Short);
                SoundManager.Padlock.PlaySound();/*En estos botones hay que hacer sonar el sonido dependiendo de si se puede abrir el modo de juego o no*/
                Statics.Vibrate(VibrationDuration.Long);
            }
            else/*No está bloqueado*/
            {
                OrchestratorManager.LevelNumber = level;
                OrchestratorManager.BackWindow = WindowType;
                OrchestratorManager.GameWindowInvoker = WindowType.Level;

                if(ControllerSettings.GetShowHowToPlay(OrchestratorManager.GameMode))
                    CloseMeAndOpenThis(WindowType.Game);                
                else
                    CloseMeAndOpenThis(WindowType.HowToPlay, new WindowHowToPlayParams(OrchestratorManager.GameMode, false));
            }
        }

        Image GetImagePadlockByLevel(int level)
        {
            return imageLock[level - 1];
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            shakeAnimation?.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}