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

using Android.Icu.Number;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Animated;
using ShapesAndColorsChallenge.Class.Effects.Bloom;
using ShapesAndColorsChallenge.Class.Entities;
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class PerksPanel : Entity, IDisposable
    {
        #region CONST

        const int TIME_STOP_TIME = 5000;
        const int REVEAL_NUMBER = 2;

        #endregion

        #region DELEGATES

        internal event EventHandler OnPerkRevealStart;
        internal event EventHandler OnPerkChangeStart;
        internal event EventHandler OnPerkTimeStopStart;
        internal event EventHandler OnPerkRevealEnd;
        internal event EventHandler OnPerkChangeEnd;
        internal event EventHandler OnPerkTimeStopEnd;

        #endregion

        #region VARS

        Rectangle buttonChangeBounds = new Rectangle(BaseBounds.Limits.Right - BaseBounds.Perk.Width, BaseBounds.Limits.Bottom - BaseBounds.Perk.Height, BaseBounds.Perk.Width, BaseBounds.Perk.Height).Redim();
        Rectangle buttonRevealBounds = new Rectangle(BaseBounds.Limits.X + BaseBounds.Limits.Width.Half() - BaseBounds.Perk.Width.Half(), BaseBounds.Limits.Bottom - BaseBounds.Perk.Height, BaseBounds.Perk.Width, BaseBounds.Perk.Height).Redim();
        Rectangle buttonTimeStopBounds = new Rectangle(BaseBounds.Limits.X, BaseBounds.Limits.Bottom - BaseBounds.Perk.Height, BaseBounds.Perk.Width, BaseBounds.Perk.Height).Redim();
        Button buttonReveal, buttonChange, buttonTimeStop;
        Image imageReveal, imageChange, imageTimeStop;
        Image imageBubbleReveal, imageBubbleChange, imageBubbleTimeStop;
        Label labelReveal, labelChange, labelTimeStop;
        Texture2D textureInfoBubble;/*No se hace Dispose*/
        Animation animatedChange;
        Animation animatedReveal;
        Animation animatedTimeStop;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Ventana a la que pertenece este Panel.
        /// </summary>
        Window Window { get; set; }

        /// <summary>
        /// Indica que este potenciador está en marcha.
        /// </summary>
        internal bool IsRevealRunning { get; set; } = false;

        /// <summary>
        /// Indica que este potenciador está en marcha.
        /// </summary>
        internal bool IsChangeRunning { get; set; } = false;

        /// <summary>
        /// Indica que este potenciador está en marcha.
        /// </summary>
        internal bool IsTimeStopRunning { get; set; } = false;

        /// <summary>
        /// Tiempo que queda del potenciador.
        /// </summary>
        internal int TimeStopRemainingTime { get; set; } = TIME_STOP_TIME;

        bool IsSomePerkRunning
        {
            get { return IsRevealRunning || IsChangeRunning || IsTimeStopRunning; }
        }

        /// <summary>
        /// Contador de tiempo.
        /// </summary>
        Stopwatch Stopwatch { get; set; } = new Stopwatch();

        internal int RemainingReveal { get; set; }

        #endregion

        #region CONSTRUCTORS

        internal PerksPanel(ModalLevel modalLevel, Window window) : base(modalLevel)
        {
            Window = window;
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
                animatedChange.Dispose();
                animatedChange = null;
                animatedReveal.Dispose();
                animatedReveal = null;
                animatedTimeStop.Dispose();
                animatedTimeStop = null;
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~PerksPanel()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            buttonChange.OnClick += ButtonChange_OnClick;
            buttonReveal.OnClick += ButtonReveal_OnClick;            
            buttonTimeStop.OnClick += ButtonTimeStop_OnClick;
        }

        void UnsubscribeEvents()
        {
            buttonChange.OnClick -= ButtonChange_OnClick;
            buttonReveal.OnClick -= ButtonReveal_OnClick;
            buttonTimeStop.OnClick -= ButtonTimeStop_OnClick;
        }

        private void ButtonChange_OnClick(object sender, OnClickEventArgs e)
        {
            if (ControllerPerk.Get(PerkType.Change).Amount == 0 || animatedChange.Running/*Para evitar lanzarlo dos veces seguidas*/)
                return;

            SoundManager.PerkChange.PlaySound();
            ControllerPerk.Discount(PerkType.Change);
            animatedChange.RunOnce();
            OnPerkChangeStart?.Invoke(sender, e);
            RefreshInfo();
            IsChangeRunning = true;
        }

        private void ButtonReveal_OnClick(object sender, OnClickEventArgs e)
        {
            if (ControllerPerk.Get(PerkType.Reveal).Amount == 0 || animatedReveal.Running/*Para evitar lanzarlo dos veces seguidas*/)
                return;

            SoundManager.PerkReveal.PlaySound();
            ControllerPerk.Discount(PerkType.Reveal);
            animatedReveal.RunOnce();
            OnPerkRevealStart?.Invoke(sender, e);
            RefreshInfo();
            IsRevealRunning = true;
            RemainingReveal = REVEAL_NUMBER;
        }

        private void ButtonTimeStop_OnClick(object sender, OnClickEventArgs e)
        {
            if (ControllerPerk.Get(PerkType.TimeStop).Amount == 0 || animatedTimeStop.Running/*Para evitar lanzarlo dos veces seguidas*/)
                return;

            SoundManager.PerkTimeStop.PlaySound();
            ControllerPerk.Discount(PerkType.TimeStop);
            animatedTimeStop.RunOnce();
            OnPerkTimeStopStart?.Invoke(sender, e);
            RefreshInfo();
            IsTimeStopRunning = true;
            TimeStopRemainingTime = TIME_STOP_TIME;
            Stopwatch = new();
            Stopwatch.Start();
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            SetTextures();
            SetAnimations();
            SetButtonChange();
            SetButtonReveal();
            SetButtonTimeStop();
            SubscribeEvents();
            AddToManager();
        }

        void SetAnimations()
        {
            Vector2 diff = BaseBounds.PerkImage.ToVector2() / 2 - BaseBounds.Perk.ToVector2() / 2;
            animatedChange = new Animation(TextureManager.PerkChange, Const.ANIMATED_PERK_CHANGE, (buttonChangeBounds.Location.ToVector2() - diff).Redim()) { Visible = false };
            animatedReveal = new Animation(TextureManager.PerkReveal, Const.ANIMATED_PERK_REVEAL, (buttonRevealBounds.Location.ToVector2() - diff).Redim()) { Visible = false };
            animatedTimeStop = new Animation(TextureManager.PerkTimeStop, Const.ANIMATED_PERK_TIME_STOP, (buttonTimeStopBounds.Location.ToVector2() - diff).Redim()) { Visible = false };
        }

        /// <summary>
        /// Actualiza los textos de la cantidad de potenciadores de cada clase.
        /// </summary>
        void RefreshInfo()
        {
            List<Perk> perks = ControllerPerk.Get();
            labelChange.Text = perks.Single(t => t.Type == PerkType.Change).Amount.ToString();
            labelReveal.Text = perks.Single(t => t.Type == PerkType.Reveal).Amount.ToString();
            labelTimeStop.Text = perks.Single(t => t.Type == PerkType.TimeStop).Amount.ToString();
        }

        void SetTextures()
        {
            textureInfoBubble = TextureManager.Get(new Size(Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER), Color.Orange, Color.Black, CommonTextureType.Circle).Texture;
            TextureManager.LoadAnimatedPerks();
        }

        void SetButtonChange()
        {
            buttonChange = new Button(ModalLevel, buttonChangeBounds);
            imageChange = new(ModalLevel, buttonChangeBounds, TextureManager.TexturePerkChange, Color.DarkGray, Color.DarkGray, true, 25);
            SetInfoBubble(buttonChangeBounds, ref labelChange, ControllerPerk.Get(PerkType.Change).Amount.ToString(), ref imageBubbleChange);
        }

        void SetButtonReveal()
        {
            buttonReveal = new Button(ModalLevel, buttonRevealBounds);
            imageReveal = new(ModalLevel, buttonRevealBounds, TextureManager.TexturePerkReveal, Color.DarkGray, Color.DarkGray, true, 25);
            SetInfoBubble(buttonRevealBounds, ref labelReveal, ControllerPerk.Get(PerkType.Reveal).Amount.ToString(), ref imageBubbleReveal);
        }

        void SetButtonTimeStop()
        {
            buttonTimeStop = new Button(ModalLevel, buttonTimeStopBounds);
            imageTimeStop = new(ModalLevel, buttonTimeStopBounds, TextureManager.TexturePerkTimeStop, Color.DarkGray, Color.DarkGray, true, 25);
            SetInfoBubble(buttonTimeStopBounds, ref labelTimeStop, ControllerPerk.Get(PerkType.TimeStop).Amount.ToString(), ref imageBubbleTimeStop);
        }

        void SetInfoBubble(Rectangle buttonBounds, ref Label label, string labelString, ref Image imageBubble)
        {
            Rectangle infoBubbleBounds = new(buttonBounds.Right - Const.BUBBLE_INFO_DIAMETER.Half(), buttonBounds.Top - Const.BUBBLE_INFO_DIAMETER.Half(), Const.BUBBLE_INFO_DIAMETER, Const.BUBBLE_INFO_DIAMETER);
            imageBubble = new(ModalLevel, infoBubbleBounds, textureInfoBubble);
            label = new(ModalLevel, infoBubbleBounds, labelString, Color.White, Color.Black, AlignHorizontal.Center);
        }

        void AddToManager()
        {
            Window.InteractiveObjectManager.Add(buttonReveal);
            Window.InteractiveObjectManager.Add(buttonChange);
            Window.InteractiveObjectManager.Add(buttonTimeStop);
            Window.InteractiveObjectManager.Add(imageReveal);
            Window.InteractiveObjectManager.Add(imageChange);
            Window.InteractiveObjectManager.Add(imageTimeStop);
            Window.InteractiveObjectManager.Add(imageBubbleReveal);
            Window.InteractiveObjectManager.Add(imageBubbleChange);
            Window.InteractiveObjectManager.Add(imageBubbleTimeStop);
            Window.InteractiveObjectManager.Add(labelReveal);
            Window.InteractiveObjectManager.Add(labelChange);
            Window.InteractiveObjectManager.Add(labelTimeStop);
        }

        internal void Pause(bool isGamePaused)
        {
            if (isGamePaused)
            {
                if (IsTimeStopRunning)
                    Stopwatch.Stop();
            }
            else
            {
                if (IsTimeStopRunning)
                    Stopwatch.Start();
            }
        }

        internal void Enable()
        {
            buttonChange.Active = true;
            buttonReveal.Active = true;
            buttonTimeStop.Active = true;
        }

        internal void Disable()
        {
            buttonChange.Active = false;
            buttonReveal.Active = false;
            buttonTimeStop.Active = false;
        }

        internal override void Update(GameTime gameTime)
        {
            UpdatePerksAnimations(gameTime);
            UpdateRunningPerks(gameTime);
        }

        void UpdatePerksAnimations(GameTime gameTime)
        {
            animatedChange.Update(gameTime);
            animatedReveal.Update(gameTime);
            animatedTimeStop.Update(gameTime);
        }

        void UpdateRunningPerks(GameTime gameTime)
        {
            UpdateRunningTimeStop();
        }

        void UpdateRunningTimeStop()
        {
            if (!IsTimeStopRunning)
                return;

            long timeElapsed = Stopwatch.ElapsedMilliseconds;
            TimeStopRemainingTime = TIME_STOP_TIME - (int)timeElapsed;

            if (TimeStopRemainingTime <= 0)
            {
                IsTimeStopRunning = false;
                OnPerkTimeStopEnd?.Invoke(null, null);
            }
        }

        internal override void Draw(GameTime gameTime)
        {
            DrawPerksAnimations(gameTime);
            DrawRunningPerks(gameTime);
        }

        void DrawPerksAnimations(GameTime gameTime)
        {
            animatedChange.Draw(gameTime);
            animatedReveal.Draw(gameTime);
            animatedTimeStop.Draw(gameTime);
        }

        void DrawRunningPerks(GameTime gameTime)
        {
            DrawRunningTimeStop(gameTime);
        }

        void DrawRunningTimeStop(GameTime gameTime)
        {
            if (!IsTimeStopRunning)
                return;

            string text = $"{TimeStopRemainingTime / 1000 + 1}";
            FontManager.DrawString(text, buttonTimeStopBounds, FontManager.GetScaleToFit(text, buttonTimeStopBounds.Size.ToVector2()), Color.Cyan, 1, AlignHorizontal.Center);
        }

        #endregion
    }
}