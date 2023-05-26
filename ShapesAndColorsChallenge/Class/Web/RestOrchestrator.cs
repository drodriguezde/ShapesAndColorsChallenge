using ShapesAndColorsChallenge.Class.Windows;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Web
{
    internal static class RestOrchestrator
    {
        #region VARS

        static BackgroundWorker worker = null;

        #endregion

        #region EVENTS

        static void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                object[] parameters = (object[])e.Argument;
                WebOperationType webOperationType = (WebOperationType)parameters[0];

                switch (webOperationType)
                {
                    case WebOperationType.SaveScore:
                        {
                            GameMode gameMode = (GameMode)parameters[1];
                            long score = ControllerScore.Get(gameMode).Sum(t => t.UserScore);
                            string playerToken = ControllerSettings.Get().PlayerToken;
                            Rest.UpdateUserScore(gameMode, playerToken, score);
                            break;
                        }
                    case WebOperationType.GetRanking:
                        {
                            GameMode gameMode = (GameMode)parameters[1];
                            ((WindowRankings)parameters[2]).SetGlobalRanking(Rest.GetRanking(gameMode));
                            break;
                        }
                }

                if (!((BackgroundWorker)sender).CancellationPending)/*Se ha finalizado antes del timeout*/
                    ((BackgroundWorker)sender).CancelAsync();
            }
            catch 
            {
            }
        }

        #endregion

        #region METHOS

        /// <summary>
        /// Reestablecemos la operativa.
        /// </summary>
        static void Reset()
        {
            if (worker != null)
            {
                worker.CancelAsync();
                worker.DoWork -= Worker_DoWork;
                worker.Dispose();
            }

            worker = new() { WorkerSupportsCancellation = true };
            worker.DoWork += Worker_DoWork;
        }

        /// <summary>
        /// Intentará guardar un record en la nube.
        /// </summary>
        /// <param name="gameMode"></param>
        internal static void TryToSaveScore(GameMode gameMode)
        {
            Reset();
            worker.RunWorkerAsync(new object[] { WebOperationType.SaveScore, gameMode });
        }

        /// <summary>
        /// Intentará obtener el ranking.
        /// </summary>
        /// <param name="gameMode"></param>
        /// <param name="windowRankings"></param>
        internal static void TryToGetRanking(GameMode gameMode, WindowRankings windowRankings)
        {
            Reset();
            worker.RunWorkerAsync(new object[] { WebOperationType.GetRanking, gameMode, windowRankings });
        }

        #endregion
    }
}
