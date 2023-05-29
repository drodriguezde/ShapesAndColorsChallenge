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
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class WindowManager
    {
        #region CONST



        #endregion

        #region IMPORTS



        #endregion

        #region DELEGATES



        #endregion

        #region VARS

        readonly static Array modalLevels = System.Enum.GetValues(typeof(ModalLevel));

        static WindowMessageBox simpleMessage;

        #endregion

        #region PROPERTIES

        static internal List<Window> Windows { get; private set; } = new List<Window>();

        /// <summary>
        /// Obtiene la ventana TopMost, no puede ser Form.
        /// </summary>
        static internal WindowType GetTopMostWindowOpened
        {
            get
            {
                ModalLevel modalLevel = ModalLevel.None;/*El nivel más bajo*/
                WindowType windowType = WindowType.None;

                for (int i = 0; i < Windows.Count; i++)
                    if ((byte)Windows[i].ModalLevel > (byte)modalLevel)
                    {
                        modalLevel = Windows[i].ModalLevel;
                        windowType = Windows[i].WindowType;
                    }

                return (byte)modalLevel > (byte)ModalLevel.Window
                    ? windowType
                    : WindowType.None;
            }
        }

        static internal ModalLevel GetTopModalLevel
        {
            get
            {
                ModalLevel modalLevel = ModalLevel.None;/*El nivel más bajo*/

                for (int i = 0; i < Windows.Count; i++)
                    if ((byte)Windows[i].ModalLevel > (byte)modalLevel)
                        modalLevel = Windows[i].ModalLevel;

                return modalLevel;
            }
        }

        #endregion

        #region CONSTRUCTORS



        #endregion

        #region DESTRUCTOR



        #endregion

        #region EVENTS

        static void Window_OnClose(object sender, EventArgs e)
        {
            Remove((sender as Window).ID);
        }

        static void SimpleMessage_OnAccept(object sender, EventArgs e)
        {
            simpleMessage.OnAccept -= SimpleMessage_OnAccept;

            if (IsThisWindowOpened(WindowType.MessageBox))
                CloseTopMostWindow();

            simpleMessage = null;
        }

        #endregion

        #region METHODS

        internal static void CloseTopMostWindow()
        {
            WindowType topMostWindowOpened = GetTopMostWindowOpened;
            Remove(topMostWindowOpened);
        }

        internal static void CloseAllTopMostWindows()
        {
            while (GetTopMostWindowOpened != WindowType.None)
                CloseTopMostWindow();
        }

        /// <summary>
        /// Abre o cierra una determinada ventana.
        /// </summary>
        /// <param name="windowType"></param>
        /// <param name="modalLevel"></param>
        /// <returns></returns>
        internal static Window OpenCloseWindow(WindowType windowType, ModalLevel modalLevel, object parameters = null)
        {
            if (!IsThisWindowOpened(windowType))/*No está abierta esta ventana*/
            {
                if ((byte)modalLevel < 2)/*Las de nivel 0 y 1 se abren siempre si no existen, ya que se lanzan una única vez al comienzo*/
                {
                    return Add(windowType, parameters);
                }
                else if (GetTopModalLevel < modalLevel)/*No hay abierta una de nivel igual y superior*/
                {
                    return Add(windowType, parameters);
                }
            }
            else
                Remove(windowType);

            return null;
        }

        internal static bool IsThisWindowOpened(WindowType windowType)
        {
            return Windows.Any(t => t.WindowType == windowType);
        }

        /// <summary>
        /// Añade una ventana de un tipo determinado a la colección de ventanas.
        /// Hay ventanas que están al mismo nivel como Main, Battlefield y últimos objetos caidos.
        /// </summary>
        /// <param name="windowType"></param>
        static Window Add(WindowType windowType, object parameters = null)
        {
            if (IsThisWindowOpened(windowType))
                return null;

            Window window = WindowDeployer.Deploy(windowType, parameters);
            window.OnClose += Window_OnClose;/*Lo disparará el botón cerrar de las ventanas*/
            window.LoadContent();
            Windows.Add(window);
            return window;
        }

        static void Remove(WindowType windowType)
        {
            if (!IsThisWindowOpened(windowType))
                return;

            Window window = Windows.FirstOrDefault(t => t.WindowType == windowType);

            if (window != null)
                Remove(window);
        }

        internal static void Remove(long id)
        {
            Window window = Windows.FirstOrDefault(t => t.ID == id);

            if (window != null)
            {
                window.Visible = false;
                Remove(window);
            }
        }

        internal static void Remove(Window window)
        {
            window.Visible = false;
            Windows.Remove(window);
            Nuller.Null(ref window);
        }

        /// <summary>
        /// Quita todas las ventanas de la colección y las destruye.
        /// </summary>
        internal static void DisposeAllWindows()
        {
            for (int i = Windows.Count - 1; i >= 0; i--)
                Remove(Windows[i]);

            Windows.Clear();
        }

        internal static Window GetWindow(WindowType windowType)
        {
            return Windows.FirstOrDefault(t => t.WindowType == windowType);
        }

        internal static bool IsOpen(WindowType windowType)
        {
            return Windows.Exists(t => t.WindowType == windowType);
        }

        /// <summary>
        /// Comprueba si una ventana es la superior.
        /// </summary>
        /// <param name="modalLevel"></param>
        /// <returns></returns>
        internal static bool ItsMeTheTopMost(ModalLevel modalLevel)
        {
            return modalLevel >= GetTopModalLevel;
        }

        internal static void LoadContent()
        {

        }

        internal static void Update(GameTime gameTime)
        {
            lock (Windows)
                for (int i = 0; i < Windows.Count; i++)
                    Windows[i].Update(gameTime);
        }

        internal static void Draw(GameTime gameTime)
        {
            foreach (ModalLevel modalLevel in modalLevels)
                for (int i = 0; i < Windows.Count; i++)
                    if (Windows[i].ModalLevel == modalLevel && Windows[i].Visible)
                        Windows[i].Draw(gameTime);
        }

        #endregion
    }
}