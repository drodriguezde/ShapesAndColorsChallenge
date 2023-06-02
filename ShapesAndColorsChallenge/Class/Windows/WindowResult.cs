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
using ShapesAndColorsChallenge.Class.Controls;
using ShapesAndColorsChallenge.Class.D2;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.Class.Particles;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Windows
{
    public class WindowResult : Window, IDisposable
    {
        #region CONST

        const int OFFSET = 50;
        const int MESSAGE_LABEL_HEIGHT = 120;

        /// <summary>
        /// Rotación de las estrellas en grados.
        /// </summary>
        const float ROTATION_STAR = 25f;

        #endregion

        #region DELEGATES

        internal event EventHandler OnAccept;

        #endregion

        #region VARS

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        Button buttonAccept;
        PerkType perkType = (PerkType)Statics.GetRandom(1, 3);

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Tamaño y posición de botoón ok cuando está junto con el botón cancelar para la resolución base.
        /// </summary>
        Rectangle ButtonOKBounds { get; set; } = new Rectangle(0, 0, 300, 256);

        WindowResultParams WindowResultParams { get; set; }

        bool VoicePlayed { get; set; } = false;

        #endregion

        #region CONSTRUCTORS

        internal WindowResult(WindowResultParams windowResultParams) : base(ModalLevel.MessageBox, Rectangle.Empty, WindowType.Result)
        {
            WindowResultParams = windowResultParams;
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
        ~WindowResult()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        /// <summary>
        /// Engancha los eventos de los objetos interactivos.
        /// </summary>
        void SubscribeEvents()
        {
            if (buttonAccept != null)
                buttonAccept.OnClick += ButtonAccept_OnClick;
        }

        /// <summary>
        /// Desengancha los eventos de los objetos interactivos.
        /// </summary>
        void UnsubscribeEvents()
        {
            if (buttonAccept != null)
                buttonAccept.OnClick -= ButtonAccept_OnClick;
        }

        private void ButtonAccept_OnClick(object sender, EventArgs e)
        {
            ParticleEngine.End();
            OnAccept?.Invoke(sender, e);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            Bounds = GetBounds();
            SetColorMode();
            SetButtonOK();

            if (WindowResultParams.IsChallenge)
            {
                SetMessageChallenge();
                SetReward();
            }
            else
            {
                SetMessage();
                SetPoints();
                SetStars();
            }

            SubscribeEvents();
        }

        internal override void SetColorMode()
        {
            BodyTexture = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), ColorManager.WindowBodyColor, ColorManager.WindowBorderColor, CommonTextureType.RoundedRectangle).Texture;
        }

        Rectangle GetBounds()
        {
            int height = OFFSET + MESSAGE_LABEL_HEIGHT + OFFSET + BaseBounds.Button.Height + OFFSET + ButtonOKBounds.Height + OFFSET;

            return new Rectangle(BaseBounds.Limits.X + 50, BaseBounds.Bounds.Height.Half() - height.Half(), BaseBounds.Limits.Width - 100, height);
        }

        void SetButtonOK()
        {
            Rectangle bounds = GetBounds();
            bounds.Y = bounds.Bottom - OFFSET - ButtonOKBounds.Height;
            bounds.X = bounds.X + bounds.Width.Half() - ButtonOKBounds.Width.Half();
            bounds.Width = ButtonOKBounds.Width;
            bounds.Height = ButtonOKBounds.Height;
            buttonAccept = new Button(ModalLevel, bounds);
            Image imageAccept = new(ModalLevel, bounds, TextureManager.TextureOkButton, ColorManager.HardGray, ColorManager.HardGray, true);
            InteractiveObjectManager.Add(buttonAccept, imageAccept);
        }

        void SetMessage()
        {
            if (!WindowResultParams.NewRecord)
                return;

            Rectangle bounds = new(Bounds.X, Bounds.Y + 340, Bounds.Width, 100);
            Label label = new(ModalLevel, bounds, Resource.String.NEW_RECORD.GetString(), ColorManager.LightGreen, ColorManager.LightGreen, AlignHorizontal.Center);
            InteractiveObjectManager.Add(label);
        }

        void SetMessageChallenge()
        {
            Rectangle bounds = new(Bounds.X, Bounds.Y + 340, Bounds.Width, 100);
            Label label;

            if (WindowResultParams.ChallengeCompleted)
                label = new(ModalLevel, bounds, Resource.String.CHALLENGE_COMPLETED.GetString(), ColorManager.LightGreen, ColorManager.LightGreen, AlignHorizontal.Center);
            else
                label = new(ModalLevel, bounds, Resource.String.CHALLENGE_FAILED.GetString(), ColorManager.Red, ColorManager.Red, AlignHorizontal.Center);

            InteractiveObjectManager.Add(label);
        }

        void SetPoints()
        {
            string text = $"{Resource.String.POINTS.GetString()} {WindowResultParams.Points}";
            Rectangle bounds = new(Bounds.X, Bounds.Y + 250, Bounds.Width, 100);
            Label label = new(ModalLevel, bounds, text, ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center);
            InteractiveObjectManager.Add(label);
        }

        void SetStars()
        {
            Image[] image = new Image[3];
            Vector2 origin = TextureManager.TextureStar.Bounds.Size.ToVector2() / 2f;

            Rectangle rectangleImage = new(
                Bounds.X + Bounds.Width.Half() - 200.Half() - 100,
                Bounds.Y + 150,
                140,
                140);
            image[0] = new Image(ModalLevel, rectangleImage, WindowResultParams.Stars > 0 ? TextureManager.TextureStar : TextureManager.TextureStarGray, -ROTATION_STAR.ToRadians(), origin, true);

            rectangleImage = new Rectangle(
                Bounds.X + Bounds.Width.Half() - 200.Half(),
                Bounds.Y + 30,
                200,
                200);
            image[1] = new Image(ModalLevel, rectangleImage, WindowResultParams.Stars > 2 ? TextureManager.TextureStar : TextureManager.TextureStarGray, true);

            rectangleImage = new Rectangle(
                Bounds.X + Bounds.Width.Half() + 200,
                Bounds.Y + 150,
                140,
                140);
            image[2] = new Image(ModalLevel, rectangleImage, WindowResultParams.Stars > 1 ? TextureManager.TextureStar : TextureManager.TextureStarGray, ROTATION_STAR.ToRadians(), origin, true);

            InteractiveObjectManager.Add(image[0], image[1], image[2]);
        }

        void SetReward()
        {
            Rectangle bounds = new(Bounds.X + Bounds.Width.Half() - 250.Half(), Bounds.Y + 30, 200, 200);
            Rectangle imageBounds = new(BaseBounds.Bounds.Width.Half() - BaseBounds.Perk.Width + 50,
                Bounds.Top + OFFSET,
                BaseBounds.Perk.Width,
                BaseBounds.Perk.Height);
            Image imagePerk = new(ModalLevel, imageBounds, Statics.GetPerkImage(perkType), ColorManager.HardGray, ColorManager.HardGray, true);
            Rectangle labelBounds = new(
                BaseBounds.Bounds.Width.Half() + 100,
                imageBounds.Top + imageBounds.Height.Half() - MESSAGE_LABEL_HEIGHT.Half(),
                300,
                MESSAGE_LABEL_HEIGHT);
            Label labelAmount;

            if (WindowResultParams.ChallengeCompleted)
                labelAmount = new(ModalLevel, labelBounds, "x 1", ColorManager.Green, ColorManager.Green);
            else
                labelAmount = new(ModalLevel, labelBounds, "x 0", ColorManager.Red, ColorManager.Red);

            AcheivementsManager.Refresh();
            InteractiveObjectManager.Add(imagePerk, labelAmount);
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (WindowResultParams.IsChallenge && !VoicePlayed)/*Si es un reto.*/
            {
                VoicePlayed = true;

                if (WindowResultParams.ChallengeCompleted)
                {
                    Perk perk = ControllerPerk.Get().Single(t => t.Type == perkType);
                    perk.Amount++;
                    SoundManager.VoiceYouWin.PlayVoice();
                    ParticleEngine.Start(ParticleType.Fireworks);
                    ControllerPerk.Update(perk);
                    ChallengesManager.ChallengeSuccess(WindowResultParams.Challenge);
                }
                else
                {
                    SoundManager.VoiceYouLose.PlayVoice();
                    ChallengesManager.ChallengeFailed(WindowResultParams.Challenge);
                }
            }
            else
            {
                if (!VoicePlayed && WindowResultParams.NewRecord)
                {
                    VoicePlayed = true;
                    SoundManager.VoiceNewHighScore.PlayVoice();
                    ParticleEngine.Start(ParticleType.Fireworks);
                }
            }

            ParticleEngine.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            Screen.SpriteBatch.FillRectangle(Screen.BacklayerBounds, Color.Black * BacklayerTransparency);
            Screen.SpriteBatch.Draw(BodyTexture, Location, Color.White * BodyTransparency);
            base.Draw(gameTime);
            ParticleEngine.Draw(gameTime);
        }

        #endregion
    }
}