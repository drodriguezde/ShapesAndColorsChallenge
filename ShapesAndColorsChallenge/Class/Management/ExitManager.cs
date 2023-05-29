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

using Android.OS;
using Microsoft.Xna.Framework;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;
using System;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class ExitManager
    {
        #region VARS

        public static WindowMessageBox windowMessageExit;

        static int timeCounter = 1000;

        #endregion

        #region PROPERTIES

        static Game Game { get; set; }
        internal static bool BackButtonPressed { get; set; }

        #endregion

        #region CONSTRUCTORS


        #endregion

        #region EVENTS

        /// <summary>
        /// Se dispara este evento si se pulsa el botón OK en el mensaje emergente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void WindowMessageExit_OnAccept(object sender, EventArgs e)
        {
            GameContent.UnloadAllContent();
            Game.Exit();/*No es un cierre definitivo de la aplicación*/
            Game.Activity.FinishAndRemoveTask();/*Cierra la aplicación definitivamente*/
            Process.KillProcess(Process.MyPid());/*Mata el proceso*/
            //Activity.FinishAndRemoveTask();/*Cierra la aplicación definitivamente*/
            //Activity.MoveTaskToBack(true);/*Envía la aplicación a segundo plano pero no la cierra*/
        }

        /// <summary>
        /// Se dispara cuando se pulsa el botón cancelar en el mensaje emergente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void WindowMessageExit_OnCancel(object sender, EventArgs e)
        {
            WindowManager.Remove(windowMessageExit.ID);
            windowMessageExit = null;
        }

        #endregion

        #region METHODS

        internal static void Initialize(Game game)
        {
            Game = game;
        }

        /// <summary>
        /// Pregunta al usuario si quiere salir del juego.
        /// Si hay una formulario o un cuadro de mensaje en primer plano lo cierra.
        /// </summary>
        static void ExitGame()
        {
            /*Si hay algún MessageBox lo cerramos*/
            if (WindowManager.GetTopModalLevel == ModalLevel.MessageBox)
            {
                WindowManager.CloseTopMostWindow();
                return;
            }

            /*No hay MessageBox, comprobamos si podemos ir a la ventana anterior*/
            if (OrchestratorManager.BackButtonPressed())/*Si la operación de ir atrás es cambiar de pantalla*/
                return;

            OrchestratorManager.OpenMessageBox(ref windowMessageExit, new(Resource.String.EXIT_GAME_QUESTION.GetString(), MessageBoxButton.AcceptCancel));
            windowMessageExit.OnCancel += WindowMessageExit_OnCancel;
            windowMessageExit.OnAccept += WindowMessageExit_OnAccept;
        }

        internal static void Update(GameTime gameTime)
        {
            if (timeCounter < Const.TIME_BETWEEN_BACK_BUTTON_CLICK)/*Evita multiples pulsaciones*/
            {
                timeCounter += gameTime.ElapsedGameTime.Milliseconds;
                return;
            }

            if (windowMessageExit != null)/*Hay un mensaje que pregunta si salir abierto*/
                return;

            if (/*GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Back) || */BackButtonPressed)
            {
                BackButtonPressed = false;
                timeCounter = 0;
                ExitGame();
            }
        }

        #endregion
    }
}