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

using InputHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class NavigationPanelHorizontal : InteractiveObject, IDisposable
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS

        /// <summary>
        /// Separación que hay entre el último elemento del primer panel y los puntos de navegación.
        /// </summary>
        int dotsTopSeparation = 90.RedimY();

        /// <summary>
        /// Separación que hay entre los puntos de navegación.
        /// </summary>
        int dotsSeparation = 60.RedimX();

        /// <summary>
        /// Diametro del punto de navegación habilitado.
        /// </summary>
        int enabledDotDiameter = 36.RedimX();

        /// <summary>
        /// Diametro del punto de navegación deshabilitado.
        /// </summary>
        int disabledDotDiameter = 20.RedimX();

        /// <summary>
        /// Textura del punto de navegación del panel actualmente activo.
        /// </summary>
        Texture2D textureDotEnabled;/*No hay que hacer dispose*/

        /// <summary>
        /// Textura del punto de navegación del panel actualmente activo.
        /// </summary>
        Texture2D textureDotDisabled;/*No hay que hacer dispose*/

        #endregion

        #region PROPERTIES

        List<PanelObject> PanelObjects { get; set; } = new List<PanelObject>();

        int CurrentPanel { get; set; } = 1;

        List<Image> PanelDots { get; set; } = new List<Image>();

        /// <summary>
        /// Indica la posición más baja de entre todos los elementos.
        /// </summary>
        int Bottom { get; set; }

        /// <summary>
        /// Indica si hay una transición en curso y su dirección.
        /// </summary>
        TransitionPanelDirection TransitionPanelDirection { get; set; } = TransitionPanelDirection.None;

        /// <summary>
        /// Indica el índice del panel que se va a ocultar en la transición.
        /// </summary>
        int IndexHidePanelTransition { get; set; } = 0;

        /// <summary>
        /// Indica el índice del panel que se va a mostrar en la transición.
        /// </summary>
        int IndexShowPanelTransition { get; set; } = 0;

        /// <summary>
        /// Ventana a la que pertenece este Panel.
        /// </summary>
        Window Window { get; set; }

        /// <summary>
        /// Tiempo total transcurrido.
        /// </summary>
        long TotalTime { get; set; }

        /// <summary>
        /// Indica cuando va a aumentar o disminuir la transparencia o la opacidad.
        /// </summary>
        float DiffTransparency { get; set; } = 0f;

        internal bool PlaySoundOnSlide { get; set; } = true;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor de la clase, bounds debe estar redimensionada, está clase no lo hará.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        internal NavigationPanelHorizontal(ModalLevel modalLevel, Rectangle bounds, int bottom, Window window)
            : base(modalLevel, bounds)
        {
            Bottom = bottom;
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
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~NavigationPanelHorizontal()
        {
            Dispose(false);
        }

        #endregion

        #region EVENTS

        void SubscribeEvents()
        {
            TouchManager.OnDrag += TouchManager_OnDrag;
            TouchManager.OnDrop += TouchManager_OnDrop;
        }

        void UnsubscribeEvents()
        {
            TouchManager.OnDrag -= TouchManager_OnDrag;
            TouchManager.OnDrop -= TouchManager_OnDrop;
        }

        private void TouchManager_OnDrag(object sender, DragEventArgs e)
        {
            if (e.Delta == Vector2.Zero || !TopMost)
                return;

            if (TransitionPanelDirection != TransitionPanelDirection.None)
                return;

            float diff = e.Start.X - e.Current.X;

            if (diff >= Bounds.Width * 0.05)/*De derecha a izquierda más de un 5% del tamaño de la pantalla*/
                Move(diff);
            if (diff.Abs() >= Bounds.Width * 0.05)/*De izquierda a derecha más de un 5% del tamaño de la pantalla*/
                Move(diff);
            else if (diff < Bounds.Width * 0.05 && diff >= 0)
                Move(0);
        }

        private void TouchManager_OnDrop(object sender, DropEventArgs e)
        {
            if (TransitionPanelDirection != TransitionPanelDirection.None || !TopMost)
                return;

            float diff = e.Start.X - e.Drop.X;

            if (diff > Bounds.Width * 0.3)/*De derecha a izquierda más de un 30% del tamaño de la pantalla*/
                TransitioningPanels(TransitionPanelDirection.Right);
            else if (diff.Abs() > Bounds.Width * 0.3)/*De izquierda a derecha más de un 30% del tamaño de la pantalla*/
                TransitioningPanels(TransitionPanelDirection.Left);
            else
                RestoreCurrent();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Inicia el proceso de ocultar el panel actual y mostrar el siguiente de forma progresiva.
        /// </summary>
        void TransitioningPanels(TransitionPanelDirection transitionPanelDirection)
        {
            if (PlaySoundOnSlide)
                SoundManager.PanelSlide.PlaySound();

            TransitionPanelDirection = transitionPanelDirection;
            IndexHidePanelTransition = CurrentPanel;
            IndexShowPanelTransition = GetNextPanel(transitionPanelDirection);
            PanelObjects.Where(t => t.PanelIndex == IndexShowPanelTransition).ToList().ForEach(t =>
            {
                if((t.AddedObject as InteractiveObject).VisibleForNavigationPanel)
                    (t.AddedObject as InteractiveObject).Visible = true;

                MasterTransparency = 0f;
            });
            TotalTime = 0;
            DiffTransparency = 1f / 250;
            SetNavigationDots();
        }

        internal void Add(int panelIndex, params object[] addedObjects)
        {
            foreach (object o in addedObjects)
                PanelObjects.Add(new PanelObject(panelIndex, o));
        }

        /// <summary>
        /// Coloca en la posición original los elementos del panel actual.
        /// </summary>
        void RestoreCurrent()
        {
            foreach (PanelObject panelObject in PanelObjects.Where(t => t.PanelIndex == CurrentPanel))
            {
                if (panelObject.Type.Name == typeof(Button).Name)
                    (panelObject.AddedObject as Button).Location = new Vector2(panelObject.X1Original, panelObject.Y1Original);
                else if (panelObject.Type.Name == typeof(Label).Name)
                    (panelObject.AddedObject as Label).Location = new Vector2(panelObject.X1Original, panelObject.Y1Original);
                else if (panelObject.Type.Name == typeof(Image).Name)
                    (panelObject.AddedObject as Image).Location = new Vector2(panelObject.X1Original, panelObject.Y1Original);
                else if (panelObject.Type.Name == typeof(RectangleSquare).Name)
                    (panelObject.AddedObject as RectangleSquare).Location = new Vector2(panelObject.X1Original, panelObject.Y1Original);
                else if (panelObject.Type.Name == typeof(Line).Name)
                {
                    (panelObject.AddedObject as Line).P1X = panelObject.X1Original.ToInt();
                    (panelObject.AddedObject as Line).P2X = panelObject.X2Original.ToInt();
                }
            }
        }

        /// <summary>
        /// Mueve hacía la izquierda los elementos del panel actual.
        /// </summary>
        /// <param name="diff"></param>
        void Move(float diff)
        {
            foreach (PanelObject panelObject in PanelObjects.Where(t => t.PanelIndex == CurrentPanel))
            {
                if (panelObject.Type.Name == typeof(Button).Name)
                    (panelObject.AddedObject as Button).Location = new Vector2(panelObject.X1Original - diff, panelObject.Y1Original);
                else if (panelObject.Type.Name == typeof(Label).Name)
                    (panelObject.AddedObject as Label).Location = new Vector2(panelObject.X1Original - diff, panelObject.Y1Original);
                else if (panelObject.Type.Name == typeof(Image).Name)
                    (panelObject.AddedObject as Image).Location = new Vector2(panelObject.X1Original - diff, panelObject.Y1Original);
                else if (panelObject.Type.Name == typeof(RectangleSquare).Name)
                    (panelObject.AddedObject as RectangleSquare).Location = new Vector2(panelObject.X1Original - diff, panelObject.Y1Original);
                else if (panelObject.Type.Name == typeof(Line).Name)
                {
                    (panelObject.AddedObject as Line).P1X = (panelObject.X1Original - diff).ToInt();
                    (panelObject.AddedObject as Line).P2X = (panelObject.X2Original - diff).ToInt();
                }
            }
        }

        /// <summary>
        /// Devuelve el índice del panel que deberá sustituir al actual.
        /// </summary>
        /// <param name="transitionPanelDirection"></param>
        /// <returns></returns>
        int GetNextPanel(TransitionPanelDirection transitionPanelDirection)
        {
            if (transitionPanelDirection == TransitionPanelDirection.Left)
            {
                if (CurrentPanel == 1)
                    return PanelDots.Count;
                else
                    return CurrentPanel - 1;
            }
            else
            {
                if (CurrentPanel == PanelDots.Count)
                    return 1;
                else
                    return CurrentPanel + 1;
            }
        }

        internal void Set()
        {
            SetNavigationDots();
            SetObjectsPropertiesByActivePanel();
        }

        void SetNavigationDots()
        {
            if (PanelDots.Any())
            {
                Vector2 locationCurrentlyActive = PanelDots[IndexHidePanelTransition - 1].Location;
                Vector2 locationNextActive = PanelDots[IndexShowPanelTransition - 1].Location;
                Vector2 tempLocation = locationCurrentlyActive;
                PanelDots[IndexHidePanelTransition - 1].Bounds = new Rectangle((locationNextActive.X + disabledDotDiameter.Half() - enabledDotDiameter.Half()).ToInt(), (locationNextActive.Y + disabledDotDiameter.Half() - enabledDotDiameter.Half()).ToInt(), enabledDotDiameter, enabledDotDiameter);
                PanelDots[IndexHidePanelTransition - 1].Tag[0] = false;
                PanelDots[IndexShowPanelTransition - 1].Bounds = new Rectangle((tempLocation.X - disabledDotDiameter.Half() + enabledDotDiameter.Half()).ToInt(), (tempLocation.Y - disabledDotDiameter.Half() + enabledDotDiameter.Half()).ToInt(), disabledDotDiameter, disabledDotDiameter);
                PanelDots[IndexShowPanelTransition - 1].Tag[0] = true;
                Image image = PanelDots[IndexHidePanelTransition - 1];
                PanelDots[IndexHidePanelTransition - 1] = PanelDots[IndexShowPanelTransition - 1];
                PanelDots[IndexShowPanelTransition - 1] = image;
            }
            else/*La primera vez, no se han añadido*/
            {
                int dotsNumber = PanelObjects.Max(t => t.PanelIndex);
                int totalSpaceBetweenDots = (dotsNumber - 1) * dotsSeparation;
                int firstDotLeft = Bounds.Width.Half() - totalSpaceBetweenDots.Half();

                for (int i = 1; i < dotsNumber + 1; i++)
                {
                    bool active = i == 1;
                    int radius = active ? enabledDotDiameter.Half() : disabledDotDiameter.Half();
                    Vector2 location = new(firstDotLeft + dotsSeparation * (i - 1) - radius, Bottom + dotsTopSeparation - radius);
                    Image imageDot = new(ModalLevel, new Rectangle(location.X.ToInt(), location.Y.ToInt(), radius * 2, radius * 2), active ? textureDotEnabled : textureDotDisabled) { Tag = new() { active } };
                    PanelDots.Add(imageDot);
                    Window.InteractiveObjectManager.Add(imageDot);
                }
            }
        }

        void SetObjectsPropertiesByActivePanel()
        {
            for (int i = 0; i < PanelObjects.Count; i++)
                if ((PanelObjects[i].AddedObject as InteractiveObject).VisibleForNavigationPanel)
                {
                    (PanelObjects[i].AddedObject as InteractiveObject).Visible = PanelObjects[i].PanelIndex == CurrentPanel;
                    (PanelObjects[i].AddedObject as InteractiveObject).EnableOnClick = PanelObjects[i].PanelIndex == CurrentPanel;
                }
        }

        internal override void LoadContent()
        {
            base.LoadContent();
            LoadTextureDots();
            SubscribeEvents();
        }

        void LoadTextureDots()
        {
            textureDotEnabled = TextureManager.Get(new Size(enabledDotDiameter, enabledDotDiameter), Color.Cyan, Color.DarkBlue, CommonTextureType.Circle).Texture;
            textureDotDisabled = TextureManager.Get(new Size(disabledDotDiameter, disabledDotDiameter), ColorManager.MediumGray, Color.Black, CommonTextureType.Circle).Texture;
        }

        void DoTransition(GameTime gameTime)
        {
            TotalTime += gameTime.ElapsedGameTime.Milliseconds;

            float hideTransparency = 1f - (DiffTransparency * TotalTime);
            float showTransparency = DiffTransparency * TotalTime;

            if (hideTransparency < 0f)
                hideTransparency = 0f;

            if (showTransparency > 1f)
                showTransparency = 1f;

            PanelObjects.Where(t => t.PanelIndex == IndexHidePanelTransition).ToList().ForEach(t => (t.AddedObject as InteractiveObject).MasterTransparency = hideTransparency);
            PanelObjects.Where(t => t.PanelIndex == IndexShowPanelTransition).ToList().ForEach(t => (t.AddedObject as InteractiveObject).MasterTransparency = showTransparency);

            if (hideTransparency == 0f && showTransparency == 1f)
            {
                TransitionPanelDirection = TransitionPanelDirection.None;
                RestoreCurrent();
                CurrentPanel = IndexShowPanelTransition;
                SetObjectsPropertiesByActivePanel();
            }
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (TransitionPanelDirection != TransitionPanelDirection.None)
                DoTransition(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}