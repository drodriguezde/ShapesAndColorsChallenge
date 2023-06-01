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
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Windows
{
    public class WindowReward : Window, IDisposable
    {
        #region CONST

        const int OFFSET = 50;
        const int MESSAGE_LABEL_HEIGHT = 120;

        #endregion

        #region DELEGATES

        internal event EventHandler OnAccept;

        #endregion

        #region VARS

        /*No se le hace dispose directamente ya que estarán contenidos en InteractiveObjectManager*/

        Button buttonAccept;
        Label labelMessage;

        #endregion

        #region PROPERTIES

        int LinesNumber { get; set; } = 1;

        /// <summary>
        /// Tamaño y posición de botoón ok cuando está junto con el botón cancelar para la resolución base.
        /// </summary>
        Rectangle ButtonOKBounds { get; set; } = new Rectangle(0, 0, 300, 256);

        WindowRewardParams WindowRewardParams { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal WindowReward(WindowRewardParams windowRewardParams)
            : base(ModalLevel.MessageBox, Rectangle.Empty, WindowType.Reward)
        {
            WindowRewardParams = windowRewardParams;
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
        ~WindowReward()
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
            OnAccept?.Invoke(sender, e);
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            Bounds = GetBounds();
            SetColorMode();
            SetBackLayer();
            SetButtonOK();
            SetMessage();
            SetRewards();
            SubscribeEvents();
        }

        internal override void SetColorMode()
        {
            BodyTexture = TextureManager.Get(new Size(Bounds.Width, Bounds.Height), ColorManager.WindowBodyColor, ColorManager.WindowBorderColor, CommonTextureType.RoundedRectangle).Texture;
        }

        Rectangle GetBounds()
        {
            int height = OFFSET + MESSAGE_LABEL_HEIGHT + OFFSET + BaseBounds.Button.Height + OFFSET + ButtonOKBounds.Height + OFFSET;

            if (RewardTypeCounter() == 2)
            {
                height += OFFSET + BaseBounds.Button.Height;
                return new Rectangle(BaseBounds.Limits.X + 50, BaseBounds.Bounds.Height.Half() - height.Half(), BaseBounds.Limits.Width - 100, height);
            }

            if (RewardTypeCounter() == 3)
            {
                height += (OFFSET + BaseBounds.Button.Height).Double();
                return new Rectangle(BaseBounds.Limits.X + 50, BaseBounds.Bounds.Height.Half() - height.Half(), BaseBounds.Limits.Width - 100, height);
            }

            return new Rectangle(BaseBounds.Limits.X + 50, BaseBounds.Bounds.Height.Half() - height.Half(), BaseBounds.Limits.Width - 100, height);
        }

        void SetBackLayer()
        {
            TextureBackLayer = TextureManager.Get(new Size(1, 1), Color.Black, CommonTextureType.Rectangle).Texture;
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
            Rectangle bounds = GetBounds();
            bounds.Y += OFFSET;
            bounds.X += OFFSET.Half();
            bounds.Width -= OFFSET;
            bounds.Height = MESSAGE_LABEL_HEIGHT;
            labelMessage = new Label(ModalLevel, bounds, Resource.String.REWARD.GetString(), ColorManager.HardGray, ColorManager.HardGray, AlignHorizontal.Center, LinesNumber);
            InteractiveObjectManager.Add(labelMessage);
        }

        int RewardTypeCounter()
        {
            return WindowRewardParams.Amount1.NotIsZero().ToInt() + WindowRewardParams.Amount2.NotIsZero().ToInt() + WindowRewardParams.Amount3.NotIsZero().ToInt();
        }

        void SetRewards()
        {
            Rectangle bounds = GetBounds();
            Rectangle imageBounds = new(
                bounds.X + bounds.Width.Half() - BaseBounds.Button.Width - OFFSET,
                labelMessage.Bounds.Bottom + OFFSET,
                BaseBounds.Button.Width,
                BaseBounds.Button.Height);
            Image imagePerk = new(ModalLevel, imageBounds, Statics.GetPerkImage(WindowRewardParams.Reward1), ColorManager.HardGray, ColorManager.HardGray, true);
            Rectangle labelBounds = new(
                bounds.X + bounds.Width.Half() + OFFSET.Double(),
                labelMessage.Bounds.Bottom + OFFSET + BaseBounds.Button.Height.Half() - MESSAGE_LABEL_HEIGHT.Half(),
                300,
                MESSAGE_LABEL_HEIGHT);
            Label labelAmount = new(ModalLevel, labelBounds, $"x {WindowRewardParams.Amount1}", ColorManager.HardGray, ColorManager.HardGray);
            InteractiveObjectManager.Add(imagePerk, labelAmount);

            SetReward2(imageBounds, labelBounds);
            SetReward3(imageBounds, labelBounds);
        }

        void SetReward2(Rectangle imageBounds, Rectangle labelBounds)
        {
            if (WindowRewardParams.Amount2.IsZero())
                return;

            Image imagePerk = new(ModalLevel, new(imageBounds.X, imageBounds.Y + BaseBounds.Button.Height + OFFSET, imageBounds.Width, imageBounds.Height), Statics.GetPerkImage(WindowRewardParams.Reward2), ColorManager.HardGray, ColorManager.HardGray, true);
            Label labelAmount = new(ModalLevel, new(labelBounds.X, labelBounds.Y + BaseBounds.Button.Height + OFFSET, labelBounds.Width, labelBounds.Height), $"x {WindowRewardParams.Amount2}", ColorManager.HardGray, ColorManager.HardGray);
            InteractiveObjectManager.Add(imagePerk, labelAmount);
        }

        void SetReward3(Rectangle imageBounds, Rectangle labelBounds)
        {
            if (WindowRewardParams.Amount3.IsZero())
                return;

            Image imagePerk = new(ModalLevel, new(imageBounds.X, imageBounds.Y + (BaseBounds.Button.Height + OFFSET).Double(), imageBounds.Width, imageBounds.Height), Statics.GetPerkImage(WindowRewardParams.Reward3), ColorManager.HardGray, ColorManager.HardGray, true);
            Label labelAmount = new(ModalLevel, new(labelBounds.X, labelBounds.Y + (BaseBounds.Button.Height + OFFSET).Double(), labelBounds.Width, labelBounds.Height), $"x {WindowRewardParams.Amount3}", ColorManager.HardGray, ColorManager.HardGray);
            InteractiveObjectManager.Add(imagePerk, labelAmount);

        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            if (!Visible)
                return;

            Screen.SpriteBatch.Draw(TextureBackLayer, Screen.BacklayerBounds, Color.White * BacklayerTransparency);
            Screen.SpriteBatch.Draw(BodyTexture, Location, Color.White * BodyTransparency);
            base.Draw(gameTime);
        }

        #endregion
    }
}