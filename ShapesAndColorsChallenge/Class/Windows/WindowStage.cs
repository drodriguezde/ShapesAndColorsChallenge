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
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Animated;
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.DataBase.Types;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using static ShapesAndColorsChallenge.Class.Management.DataBaseManager;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal class WindowStage : Window, IDisposable
    {
        #region CONST

        readonly int SEPARATOR_TITLE_LINE = 5.RedimY();
        const int TOTAL_STARS_TOP = 250;/*Es para una imagen, no necesita Redim*/

        #endregion

        #region VARS

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        readonly Button[] buttonStage = new Button[12];
        readonly Image[] imageLock = new Image[12];

        Button buttonRanking;
        Button buttonChallenge;
        Button buttonHowToPlay;
        Texture2D textureInfoBubble;/*No se hace Dispose*/
        AnimationShake shakeAnimation;
        readonly List<StarsByStage> starsByStage = GetUserStarsGroupByStage(OrchestratorManager.GameMode);

        readonly Rectangle buttonRankingBounds = new Rectangle(
            BaseBounds.Limits.X + (BaseBounds.Limits.Width - BaseBounds.Button.Width * 4) / 3 + BaseBounds.Button.Width, 
            BaseBounds.Limits.Bottom - BaseBounds.Button.Height, 
            BaseBounds.Button.Width, 
            BaseBounds.Button.Height).Redim();

        readonly Rectangle buttonChallengesBounds = new Rectangle(
            BaseBounds.Limits.X + ((BaseBounds.Limits.Width - BaseBounds.Button.Width * 4) / 3).Double() + BaseBounds.Button.Width.Double(),
            BaseBounds.Limits.Bottom - BaseBounds.Button.Height,
            BaseBounds.Button.Width,
            BaseBounds.Button.Height).Redim();

        readonly Rectangle buttonHowToPlayBounds = new Rectangle(
            BaseBounds.Limits.X + (BaseBounds.Limits.Width - BaseBounds.Button.Width * 4) + BaseBounds.Button.Width.Multi(3),
            BaseBounds.Limits.Bottom - BaseBounds.Button.Height,
            BaseBounds.Button.Width,
            BaseBounds.Button.Height).Redim();

        readonly Rectangle[] buttonStageBounds = new Rectangle[12]
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

        readonly Rectangle[] labelStageBounds = new Rectangle[12]
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

        internal WindowStage()
            : base(ModalLevel.Window, BaseBounds.Bounds.Redim(), WindowType.Stage)
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
        ~WindowStage()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            for (int i = 0; i < GameData.STAGES; i++)
                buttonStage[i].OnClick += ButtonStage_OnClick;

            buttonRanking.OnClick += ButtonRanking_OnClick;
            buttonChallenge.OnClick += ButtonChallenge_OnClick;
            buttonHowToPlay.OnClick += ButtonHowToPlay_OnClick;
        }

        void UnsubscribeEvents()
        {
            for (int i = 0; i < GameData.STAGES; i++)
                buttonStage[i].OnClick -= ButtonStage_OnClick;

            buttonRanking.OnClick -= ButtonRanking_OnClick;
            buttonChallenge.OnClick -= ButtonChallenge_OnClick;
            buttonHowToPlay.OnClick -= ButtonHowToPlay_OnClick;
        }

        void ButtonStage_OnClick(object sender, OnClickEventArgs e)
        {
            int stage = (int)(sender as Button).Tag[0];
            CheckLockedStage(stage);
        }

        void ButtonHowToPlay_OnClick(object sender, OnClickEventArgs e)
        {
            CloseMeAndOpenThis(WindowType.HowToPlay, new WindowHowToPlayParams(OrchestratorManager.GameMode, true));
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
            SetTextures();
            SetTitle();
            SetStages();
            SetTotalStars();
            SetButtonRanking();
            SetButtonChallenges();
            SetButtonHowToPlay();
            SubscribeEvents();
        }

        void SetTextures()
        {
            textureInfoBubble = TextureManager.Get(new Size(Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER), Color.Orange, Color.Black, CommonTextureType.Circle).Texture;
        }

        void SetTitle()
        {
            Label labelTitle = new(ModalLevel, BaseBounds.Title, Statics.GetGameModeTitle(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(labelTitle);
        }

        void SetStages()
        {
            for (int i = 0; i < GameData.STAGES; i++)
                SetStage(i);
        }

        void SetTotalStars()
        {
            int currentStars = starsByStage.Sum(t => t.Stars);
            Rectangle bounds = new(BaseBounds.Bounds.Width.Half() - 80, TOTAL_STARS_TOP, 80, 80);
            Image imageStar = new(ModalLevel, bounds, TextureManager.TextureStar, true);
            Label labelStars = new(ModalLevel, new Rectangle(bounds.Right + 10, TOTAL_STARS_TOP - 5, 100, 100).Redim(), currentStars.ToString(), Color.GreenYellow, Color.GreenYellow, AlignHorizontal.Left);
            InteractiveObjectManager.Add(imageStar, labelStars);
        }

        void SetButtonRanking()
        {
            buttonRanking = new Button(ModalLevel, buttonRankingBounds);
            Image image = new(ModalLevel, buttonRankingBounds, TextureManager.TextureRankingButton, Color.DarkGray, Color.DarkGray, true, 25);
            InteractiveObjectManager.Add(buttonRanking, image);
        }

        void SetButtonChallenges()
        {
            buttonChallenge = new Button(ModalLevel, buttonChallengesBounds);
            Image image = new(ModalLevel, buttonChallengesBounds, TextureManager.TextureChallengesButton, Color.DarkGray, Color.DarkGray, true, 25);
            InteractiveObjectManager.Add(buttonChallenge, image);
            SetChallengesBubble();
        }

        void SetChallengesBubble()
        {
            int challenges = Statics.GetRandom(0, 9);

            if (challenges.IsZero())
                return;

            Rectangle infoBubbleBounds = new(buttonChallengesBounds.X - Const.BUBBLE_INFO_DIAMETER.Half(), buttonChallengesBounds.Top - Const.BUBBLE_INFO_DIAMETER.Half(), Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER);
            Image imageChallenges = new(ModalLevel, infoBubbleBounds, textureInfoBubble) { Visible = challenges.NotIsZero() };
            Label labelChallenges = new(ModalLevel, infoBubbleBounds, challenges.ToString(), Color.White, Color.Black, AlignHorizontal.Center);
            InteractiveObjectManager.Add(imageChallenges, labelChallenges);
        }

        void SetButtonHowToPlay()
        {
            buttonHowToPlay = new Button(ModalLevel, buttonHowToPlayBounds);
            Image image = new(ModalLevel, buttonHowToPlayBounds, TextureManager.TextureHowToPlayButton, Color.DarkGray, Color.DarkGray, true, 25);
            InteractiveObjectManager.Add(buttonHowToPlay, image);
        }

        void SetStage(int index)
        {
            buttonStage[index] = new Button(ModalLevel, buttonStageBounds[index]) { Tag = new() { index + 1 } };
            Label label = new(ModalLevel, labelStageBounds[index], GetLevelInterval(index + 1), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            Line line = new(ModalLevel, new Point(labelStageBounds[index].Left + SEPARATOR_TITLE_LINE.Double(), labelStageBounds[index].Bottom - SEPARATOR_TITLE_LINE), new Point(labelStageBounds[index].Right - SEPARATOR_TITLE_LINE.Double(), labelStageBounds[index].Bottom - SEPARATOR_TITLE_LINE), ColorManager.HardGray, ColorManager.HardGray, 1);
            InteractiveObjectManager.Add(buttonStage[index], label, line);
            SetStarsPadLock(index);
        }

        static string GetLevelInterval(int stage)
        {
            return $"{(stage - 1) * 12 + 1}-{stage * 12}";
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
        /// Comprueba si una etapa está bloqueada.
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        bool IsLocked(int stage)
        {
            if (stage == 1)/*La primera etapa nunca está bloqueada*/
                return false;

            int currentStarsStage = starsByStage.Sum(t => t.Stars);
            return currentStarsStage < GameData.StarsToUnlockStage(stage);
        }

        /// <summary>
        /// Pone la cantidad de estrellas obtenidas para la etapa en cuestión.
        /// </summary>
        void SetStars(int index)
        {
            Rectangle rectangle = new(
                starsBounds[index].X + starsBounds[index].Width.Half() - 90,
                starsBounds[index].Y - 10,
                90,
                90);
            Image image = new (ModalLevel, rectangle, TextureManager.TextureStar, true);

            rectangle = new(rectangle.X + 110, rectangle.Y - 5, 200, 110);
            Label label = new(ModalLevel, rectangle, starsByStage.Where(t => t.Stage == index + 1).Sum(t => t.Stars).ToString(), Color.GreenYellow, Color.GreenYellow, AlignHorizontal.Left);
            InteractiveObjectManager.Add(image, label);
        }

        /// <summary>
        /// Comprueba si un modo de juego podría estar bloqueado.
        /// </summary>
        void CheckLockedStage(int stage)
        {
            if (IsLocked(stage))/*Está bloqueada*/
            {
                shakeAnimation?.Stop();
                shakeAnimation = new AnimationShake(GetImagePadlockByStage(stage), 700);
                shakeAnimation.Start();
                Statics.ShowToast(Resource.String.STAGE_LOCKED.GetString(GameData.StarsToUnlockStage(stage)), Android.Widget.ToastLength.Short);
                SoundManager.Padlock.PlaySound();/*En estos botones hay que hacer sonar el sonido dependiendo de si se puede abrir el modo de juego o no*/
                Statics.Vibrate(VibrationDuration.Long);
            }
            else/*No está bloqueado*/
            {
                OrchestratorManager.StageNumber = stage;
                CloseMeAndOpenThis(WindowType.Level);
            }
        }

        Image GetImagePadlockByStage(int stage)
        {
            return imageLock[stage - 1];
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