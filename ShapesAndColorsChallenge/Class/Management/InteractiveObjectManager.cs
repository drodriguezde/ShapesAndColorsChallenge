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
using ShapesAndColorsChallenge.Class.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal class InteractiveObjectManager : Entity, IDisposable
    {
        #region VARS

        List<InteractiveObject> interactiveObjects = new();

        #endregion

        #region PROPERTIES

        internal List<InteractiveObject> InteractiveObjects
        {
            get => interactiveObjects;
            private set => interactiveObjects = value;
        }

        internal int Count
        {
            get
            {
                return InteractiveObjects.Count;
            }
        }

        internal bool SkipDraw { get; set; } = false;
        internal bool SkipUpdate { get; set; } = false;

        #endregion

        #region CONSTRUCTORS

        internal InteractiveObjectManager()
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
                Nuller.Null(ref interactiveObjects);
            }

            /*Objetos no administrados aquí*/

            base.Dispose(disposing);
            disposed = true;
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~InteractiveObjectManager()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        internal void Add(params InteractiveObject[] interactiveObjects)
        {
            lock (InteractiveObjects)
                foreach (InteractiveObject interactiveObject in interactiveObjects)
                {
                    interactiveObject.LoadContent();
                    InteractiveObjects.Add(interactiveObject);
                }
        }

        internal void Add(InteractiveObject interactiveObject)
        {
            interactiveObject.LoadContent();
            InteractiveObjects.Add(interactiveObject);
        }

        internal void Insert(int index, InteractiveObject interactiveObject)
        {
            interactiveObject.LoadContent();
            InteractiveObjects.Insert(index, interactiveObject);
        }

        internal void Remove(long id)
        {
            lock (InteractiveObjects)
            {
                InteractiveObject interactiveObject = InteractiveObjects.FirstOrDefault(t => t.ID == id);

                if (interactiveObject != null)
                {
                    InteractiveObjects.Remove(interactiveObject);
                    Nuller.Null(ref interactiveObject);
                }
            }
        }

        internal void SetTopMost(bool state)
        {
            for (int i = 0; i < InteractiveObjects.Count; i++)
                InteractiveObjects[i].TopMost = state;
        }

        internal InteractiveObject Get(long id)
        {
            return InteractiveObjects.FirstOrDefault(t => t.ID == id);
        }

        internal void SetMasterTransparency(float transparency)
        {
            for (int i = 0; i < InteractiveObjects.Count; i++)
                if (InteractiveObjects[i].Visible && InteractiveObjects[i].AllowFadeInFadeOut)
                    InteractiveObjects[i].MasterTransparency = transparency;
        }

        internal override void LoadContent()
        {
            for (int i = 0; i < InteractiveObjects.Count; i++)
                InteractiveObjects[i].LoadContent();
        }

        internal override void Update(GameTime gameTime)
        {
            if(SkipUpdate) 
                return;

            lock (InteractiveObjects)
                for (int i = 0; i < InteractiveObjects.Count; i++)
                    if (InteractiveObjects[i].Visible)
                        InteractiveObjects[i].Update(gameTime);
        }

        internal override void Draw(GameTime gameTime)
        {
            if (SkipDraw)
                return;

            for (int i = 0; i < InteractiveObjects.Count; i++)
                if (InteractiveObjects[i].Visible)
                    InteractiveObjects[i].Draw(gameTime);
        }

        #endregion
    }
}