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
using ShapesAndColorsChallenge.Class.Effects.Bloom;
using ShapesAndColorsChallenge.Class.Management;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Controls
{
    internal class NavigationPanelVertical : InteractiveObject, IDisposable
    {
        #region VARS

        float yStart = 0;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Ventana a la que pertenece este Panel.
        /// </summary>
        Window Window { get; set; }

        List<PanelItem> PanelItems { get; set; } = new List<PanelItem>();

        /// <summary>
        /// Límite de movimiento por arriba.
        /// </summary>
        int TopLimit { get; set; }

        /// <summary>
        /// Límite de movimiento por abajo.
        /// </summary>
        int BottomLimit { get; set; }

        /// <summary>
        /// Indica que no se puede mover el panel.
        /// </summary>
        internal bool Locked { get; set; } = false;

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor de la clase, bounds debe estar redimensionada, está clase no lo hará.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <param name="bounds"></param>
        internal NavigationPanelVertical(ModalLevel modalLevel, Rectangle bounds, int topLimit, int bottomLimit, Window window)
            : base(modalLevel, bounds)
        {
            TopLimit = topLimit;
            BottomLimit = bottomLimit;
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
        ~NavigationPanelVertical()
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
            if (e.Delta == Vector2.Zero || !WindowManager.ItsMeTheTopMost(ModalLevel))
                return;

            if (yStart == 0)
                yStart = e.Start.Y;

            float diff = yStart - e.Current.Y;
            Move(diff);
            yStart = e.Current.Y;
        }

        private void TouchManager_OnDrop(object sender, DropEventArgs e)
        {
            if (!WindowManager.ItsMeTheTopMost(ModalLevel))
                return;

            yStart = 0;
        }

        #endregion

        #region METHODS

        internal override void LoadContent()
        {
            base.LoadContent();
            SubscribeEvents();
        }

        internal void Add(PanelItem panelItem)
        {
            PanelItems.Add(panelItem);
            Window.InteractiveObjectManager.Add(panelItem);

            foreach (InteractiveObject interactiveObject in panelItem.InteractiveObjects)
                Window.InteractiveObjectManager.Add(interactiveObject);
        }

        /// <summary>
        /// Elimina uno de los objetos de un Item del panel.
        /// Devuelve el item contenedor.
        /// </summary>
        /// <param name="id"></param>
        internal void RemoveObjectInItem(long id)
        {
            for (int i = 0; i < PanelItems.Count; i++)
                for (int j = 0; j < PanelItems[i].InteractiveObjects.Count; j++)
                {
                    PanelItems[i].InteractiveObjects.RemoveAt(j);
                    Window.InteractiveObjectManager.Remove(id);
                }
        }

        /// <summary>
        /// Devuelve el item contenedor de un objeto
        /// </summary>
        /// <param name="id"></param>
        internal PanelItem GetPanelByItemInside(long id)
        {
            for (int i = 0; i < PanelItems.Count; i++)
                for (int j = 0; j < PanelItems[i].InteractiveObjects.Count; j++)
                    if (PanelItems[i].InteractiveObjects[j].ID == id)
                        return PanelItems[i];

            return null;
        }

        /// <summary>
        /// Mueve hacía arriba o abajo los elementos del panel actual.
        /// </summary>
        /// <param name="diff"></param>
        internal void Move(float diff = 0)
        {
            if (Locked)
                return;

            if (CheckFirstLastVisible(diff))
                return;

            foreach (PanelItem panelItem in PanelItems)
            {
                panelItem.Location = new(panelItem.Location.X, panelItem.Location.Y - diff);
                ItemVisibleMove(panelItem);
            }
        }

        /// <summary>
        /// Coloca los elementos si hemos llegado al principio o al final.
        /// </summary>
        bool CheckFirstLastVisible(float diff)
        {
            float yFirst = PanelItems.First().Location.Y - diff;
            float yBottom = PanelItems.Last().Location.Y + PanelItems.Last().Bounds.Height - diff;

            if (yFirst > TopLimit)/*Hemos llegado al primero haciendo scroll*/
            {
                for (int i = 0; i < PanelItems.Count; i++)
                {
                    PanelItems[i].Location = new(PanelItems[i].Location.X, TopLimit + PanelItems[i].Bounds.Height * i);
                    ItemVisibleMove(PanelItems[i]);
                }

                return true;
            }

            if (yBottom < BottomLimit)/*Hemos llegado al último haciendo scroll*/
            {
                int top = BottomLimit - PanelItems.Count * PanelItems.Last().Bounds.Height;

                for (int i = 0; i < PanelItems.Count; i++)
                {
                    PanelItems[i].Location = new(PanelItems[i].Location.X, top + PanelItems[i].Bounds.Height * i);
                    ItemVisibleMove(PanelItems[i]);
                }

                return true;
            }

            return false;
        }

        void ItemVisibleMove(PanelItem panelItem)
        {
            bool visible =
                (panelItem.Location.Y + panelItem.Bounds.Height <= BaseBounds.Bounds.Height && panelItem.Location.Y > 0) ||
                (panelItem.Location.Y >= 0 && panelItem.Location.Y + panelItem.Bounds.Height < BaseBounds.Bounds.Height);
            panelItem.Visible = visible;

            foreach (InteractiveObject interactiveObject in panelItem.InteractiveObjects)
            {
                if(!interactiveObject.VisibleForNavigationPanel)
                    interactiveObject.Visible = false;
                else
                    interactiveObject.Visible = visible;
            }

            panelItem.Move();
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #endregion
    }
}