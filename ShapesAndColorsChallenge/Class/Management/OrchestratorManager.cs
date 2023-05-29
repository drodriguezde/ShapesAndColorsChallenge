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
using ShapesAndColorsChallenge.Class.EventArguments;
using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.Enum;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class OrchestratorManager
    {
        #region CONST

        const int SHORT_TRANSITION_TIME = 500;
        const int LONG_TRANSITION_TIME = 2000;

        #endregion

        #region VARS

        static WindowDanStudios windowDanStudios;
        static WindowTitle windowTitle;
        static WindowGameMode windowGameMode;
        static WindowStage windowStage;
        static WindowSettings windowSettings;
        static WindowLanguage windowLanguage;
        static WindowLevel windowLevel;
        static WindowAcheivements windowAcheivements;
        static WindowHowToPlay windowHowToPlay;
        static WindowRankings windowRankings;
        static WindowChallenges windowChallenges;
        static WindowNationality windowNationality;
        static WindowGame windowGame;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Modo de juego seleccionado actual.
        /// Si es None es que no estamos en ningún modo de juego.
        /// </summary>
        internal static GameMode GameMode { get; set; } = GameMode.None;

        /// <summary>
        /// Indica quñe pantalla es la que ha lanzado la ventana de juego.
        /// Se usa para volver al origen cuando se termina una partida.
        /// </summary>
        internal static WindowType GameWindowInvoker { get; set; } = WindowType.None;

        /// <summary>
        /// Etapa del modo de juego actualmente seleccionada.
        /// </summary>
        internal static int StageNumber { get; set; } = 0;

        /// <summary>
        /// Fase del modo de juego actualmente seleccionada.
        /// </summary>
        internal static int LevelNumber { get; set; } = 0;

        static WindowType CurrentWindow { get; set; } = WindowType.None;

        static WindowType NextWindow { get; set; } = WindowType.None;

        /// <summary>
        /// Indica la ventana a la que hay que volver si es de origen multiple como WindowGame.
        /// </summary>
        internal static WindowType BackWindow { get; set; } = WindowType.None;

        static bool Started { get; set; } = false;

        /// <summary>
        /// Parámetros que se pasarán a la nueva ventana.
        /// </summary>
        static object WindowParams { get; set; } = null;

        #endregion

        #region EVENTS

        /// <summary>
        /// Salta aquí cuando la ventana de animación ha terminado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void TransitionWindow_OnFinish(object sender, OnFinishTransitionEventArgs e)
        {
            /*Los messagebox no hacen fade out*/

            switch (CurrentWindow)
            {
                case WindowType.DanStudios:
                    /*Acaba de terminar la transición FadeIn y va a comenzar FadeOut*/
                    /*Cuando acabe FadeOut se lanzará la pantalla Title*/
                    if (NextWindow == WindowType.None)
                        (sender as Window).CloseMeAndOpenThis(WindowType.Title);
                    else
                        DeployWindow(NextWindow);
                    return;
                case WindowType.Title:
                    if (NextWindow == WindowType.None)
                    {
                        if (!SoundManager.IsPlaying/*Si ya está sonando la música no la cortamos para reponerla*/)/*Este if debe ir anidado*/
                            SoundManager.PlayMusic(Statics.GetRandom(1, 10) > 5 ? SoundManager.bgm_01 : SoundManager.bgm_02);
                    }
                    else
                        DeployWindow(NextWindow);
                    return;
                case WindowType.GameMode:
                case WindowType.Settings:
                case WindowType.Language:
                case WindowType.Stage:
                case WindowType.Level:
                case WindowType.Acheivements:
                case WindowType.HowToPlay:
                case WindowType.Rankings:
                case WindowType.Challenges:
                case WindowType.Nationality:
                    if (NextWindow == WindowType.None)
                    { /*Nada*/ }
                    else
                        DeployWindow(NextWindow);
                    return;
                case WindowType.Game:
                    if (NextWindow == WindowType.None)
                        SoundManager.PlayMusic(2000);/*Hace sonar un música diferente a la actual*/
                    else
                    {
                        DeployWindow(NextWindow);
                        SoundManager.PlayMusic(Statics.GetRandom(1, 10) > 5 ? SoundManager.bgm_01 : SoundManager.bgm_02);
                    }
                    return;
            }
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Si se pulsa el botón back en algún momento en la aplicación salta aquí para operar.
        /// Si no compete la operación a esta clase se devuelve True.
        /// </summary>
        /// <returns></returns>
        internal static bool BackButtonPressed()
        {
            switch (CurrentWindow)
            {
                case WindowType.DanStudios:
                    return true;
                case WindowType.Level:
                    windowLevel.CloseMeAndOpenThis(WindowType.Stage);
                    return true;
                case WindowType.Stage:
                    windowStage.CloseMeAndOpenThis(WindowType.GameMode);
                    return true;
                case WindowType.GameMode:
                    windowGameMode.CloseMeAndOpenThis(WindowType.Title);
                    return true;
                case WindowType.Settings:
                    windowSettings.CloseMeAndOpenThis(WindowType.GameMode);
                    return true;
                case WindowType.Language:
                    windowLanguage.CloseMeAndOpenThis(WindowType.Settings);
                    return true;
                case WindowType.Acheivements:
                    windowAcheivements.CloseMeAndOpenThis(WindowType.GameMode);
                    return true;
                case WindowType.HowToPlay:/*La ventana HowToPlay tendrá botón "Atrás" unicamente si se abre desde la pantalla de selcción de etapa (Stage)*/
                    windowHowToPlay.CloseMeAndOpenThis(WindowType.Stage);
                    return true;
                case WindowType.Rankings:
                    windowRankings.CloseMeAndOpenThis(WindowType.Stage);
                    return true;
                case WindowType.Challenges:
                    windowChallenges.CloseMeAndOpenThis(WindowType.Stage);
                    return true;
                case WindowType.Nationality:
                    windowNationality.CloseMeAndOpenThis(WindowType.Settings);
                    return true;
                case WindowType.Game:
                    windowGame.CloseMeAndOpenThis(BackWindow);
                    return true;
                    /*MessageBox no entra por aquí ya que no vuelve a ningún lado*/
            }

            return false;
        }

        /// <summary>
        /// Despliega la ventana con el logotipo de Dan Studios.
        /// </summary>
        internal static void DeployDanStudiosWindow()
        {
            CurrentWindow = WindowType.DanStudios;
            windowDanStudios = (WindowDanStudios)WindowManager.OpenCloseWindow(WindowType.DanStudios, ModalLevel.Window);
            windowDanStudios.OnFinishTransition += TransitionWindow_OnFinish;
            windowDanStudios.StartTransition(TransitionType.Show, LONG_TRANSITION_TIME);
        }

        internal static void OpenMessageBox(ref WindowMessageBox windowMessageBox, WindowMessageBoxParams parameters)
        {
            windowMessageBox = (WindowMessageBox)WindowManager.OpenCloseWindow(WindowType.MessageBox, ModalLevel.MessageBox, parameters);
            windowMessageBox.OnFinishTransition += TransitionWindow_OnFinish;
            windowMessageBox.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
        }

        internal static void OpenWindowRewardMessage(ref WindowReward windowReward, WindowRewardParams parameters)
        {
            windowReward = (WindowReward)WindowManager.OpenCloseWindow(WindowType.Reward, ModalLevel.MessageBox, parameters);
            windowReward.OnFinishTransition += TransitionWindow_OnFinish;
            windowReward.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
        }

        internal static void OpenWindowResultMessage(ref WindowResult windowResult, WindowResultParams parameters)
        {
            windowResult = (WindowResult)WindowManager.OpenCloseWindow(WindowType.Result, ModalLevel.MessageBox, parameters);
            /*windowResult.OnFinishTransition += TransitionWindow_OnFinish;*//*La ventana de result no necesita lanzar OnFinishTransition*/
            windowResult.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
        }

        /// <summary>
        /// Abre la ventana de pausa en el juego.
        /// No hace transición.
        /// </summary>
        /// <param name="windowPause"></param>
        internal static void OpenPause(ref WindowPause windowPause)
        {
            windowPause = (WindowPause)WindowManager.OpenCloseWindow(WindowType.Pause, ModalLevel.MessageBox);
        }

        /// <summary>
        /// Abre la ventana de selección de ranking en el juego.
        /// No hace transición.
        /// </summary>
        /// <param name="windowSelectRanking"></param>
        internal static void OpenSelectRanking(ref WindowSelectRanking windowSelectRanking)
        {
            windowSelectRanking = (WindowSelectRanking)WindowManager.OpenCloseWindow(WindowType.SelectRanking, ModalLevel.MessageBox);
        }

        /// <summary>
        /// Despliega una ventana.
        /// </summary>
        static void DeployWindow(WindowType windowType)
        {
            /*Por aquí no pasa MessageBox*/
            CloseCurrentWindow();

            switch (windowType)
            {
                case WindowType.Title:
                    windowTitle = (WindowTitle)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowTitle.OnFinishTransition += TransitionWindow_OnFinish;
                    windowTitle.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.GameMode:
                    windowGameMode = (WindowGameMode)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowGameMode.OnFinishTransition += TransitionWindow_OnFinish;
                    windowGameMode.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Settings:
                    windowSettings = (WindowSettings)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowSettings.OnFinishTransition += TransitionWindow_OnFinish;
                    windowSettings.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Language:
                    windowLanguage = (WindowLanguage)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowLanguage.OnFinishTransition += TransitionWindow_OnFinish;
                    windowLanguage.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Stage:
                    windowStage = (WindowStage)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowStage.OnFinishTransition += TransitionWindow_OnFinish;
                    windowStage.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Level:
                    windowLevel = (WindowLevel)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowLevel.OnFinishTransition += TransitionWindow_OnFinish;
                    windowLevel.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Acheivements:
                    windowAcheivements = (WindowAcheivements)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowAcheivements.OnFinishTransition += TransitionWindow_OnFinish;
                    windowAcheivements.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.HowToPlay:
                    windowHowToPlay = (WindowHowToPlay)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window, WindowParams);
                    windowHowToPlay.OnFinishTransition += TransitionWindow_OnFinish;
                    windowHowToPlay.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Rankings:
                    windowRankings = (WindowRankings)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window, WindowParams);
                    windowRankings.OnFinishTransition += TransitionWindow_OnFinish;
                    windowRankings.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Challenges:
                    windowChallenges = (WindowChallenges)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowChallenges.OnFinishTransition += TransitionWindow_OnFinish;
                    windowChallenges.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Nationality:
                    windowNationality = (WindowNationality)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowNationality.OnFinishTransition += TransitionWindow_OnFinish;
                    windowNationality.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
                case WindowType.Game:
                    windowGame = (WindowGame)WindowManager.OpenCloseWindow(CurrentWindow, ModalLevel.Window);
                    windowGame.OnFinishTransition += TransitionWindow_OnFinish;
                    windowGame.StartTransition(TransitionType.Show, SHORT_TRANSITION_TIME);
                    break;
            }
        }

        /// <summary>
        /// Orquesta mediante estados qué ventanas se muestran, se dejan de mostrar y cuales ocupan su lugar.
        /// </summary>
        /// <param name="gameTime"></param>
        internal static void Update(GameTime gameTime)
        {
            if (!Started)
            {
                DeployDanStudiosWindow();
                Started = true;
            }
            else
                CheckCloseWindows();
        }

        /// <summary>
        /// Se comienza la transición de la ventana actual a la especificada.
        /// 1º FadeOut de la actual.
        /// 2º Se desengancha la actual del evento de transición y se oculta.
        /// 3º FadeIn de la siguiente.
        /// 4º Se desengancha la siguiente del evento de transición.
        /// 5º Se quita la actual del Manager y se elimina.
        /// </summary>
        internal static void StartTransition(Window window, WindowType nextWindowType, object windowParams = null)
        {
            CurrentWindow = window.WindowType;
            NextWindow = nextWindowType;
            WindowParams = windowParams;
            window.StartTransition(TransitionType.Hide, SHORT_TRANSITION_TIME);
        }

        internal static void CloseCurrentWindow()
        {
            WindowManager.GetWindow(CurrentWindow).OnFinishTransition -= TransitionWindow_OnFinish;
            /*El cierre de la ventana se hace después, cuando ya ha aparecido la siguiente, para evitar parpadeos*/
            /*Cuando TitleWindow este Launched se cierra Dan, y así sucesivamente*/
            WindowManager.GetWindow(CurrentWindow).Visible = false;
            CurrentWindow = NextWindow;
            NextWindow = WindowType.None;
        }

        /// <summary>
        /// Comprueba que ventanas se pueden cerrar.
        /// Hay que hacerlo de esta manera, primero la ventana se oculta mediante TransitionWindow y cuando ya ha pasado todo el proceso se elimina.
        /// Se hace así para evitar parpadeos.
        /// </summary>
        static void CheckCloseWindows()
        {
            /*No se comprueba MessageBox, lo hacen sus propios invocadores*/

            if (windowDanStudios != null && windowDanStudios.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.DanStudios, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowDanStudios = null;
            }

            if (windowTitle != null && windowTitle.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Title, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowTitle = null;
            }

            if (windowGameMode != null && windowGameMode.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.GameMode, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowGameMode = null;
            }

            if (windowStage != null && windowStage.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Stage, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowStage = null;
            }

            if (windowLevel != null && windowLevel.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Level, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowLevel = null;
            }

            if (windowAcheivements != null && windowAcheivements.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Acheivements, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowAcheivements = null;
            }

            if (windowHowToPlay != null && windowHowToPlay.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.HowToPlay, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowHowToPlay = null;
            }

            if (windowRankings != null && windowRankings.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Rankings, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowRankings = null;
            }

            if (windowChallenges != null && windowChallenges.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Challenges, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowChallenges = null;
            }

            if (windowSettings != null && windowSettings.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Settings, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowSettings = null;
            }

            if (windowLanguage != null && windowLanguage.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Language, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowLanguage = null;
            }

            if (windowNationality != null && windowNationality.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Nationality, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowNationality = null;
            }

            if (windowGame != null && windowGame.Visible == false)
            {
                WindowManager.CloseAllTopMostWindows();/*Si hay algún MessageBox lo quitamos, no sacar esta linea de los if*/
                WindowManager.OpenCloseWindow(WindowType.Game, ModalLevel.Window);/*No necesita dispose aquí, ya se ha hecho en WindowManager*/
                windowGame = null;
            }
        }

        #endregion
    }
}