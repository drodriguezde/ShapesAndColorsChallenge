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
    internal static class RestOperator
    {
        #region CONST

        const int TIME_OUT = 20000;

        #endregion

        #region VARS

        static BackgroundWorker worker = new();
        static Timer timer;

        #endregion

        #region EVENTS

        private static void Worker_DoWork(object sender, DoWorkEventArgs e)
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
                        Rest.UpsertRanking(gameMode, playerToken, score);
                        break;
                    }
            }

            if (!((BackgroundWorker)sender).CancellationPending)/*Se ha finalizado antes del timeout*/
                ((BackgroundWorker)sender).CancelAsync();
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

            timer?.Dispose();
            timer = new(TimeoutCallback, worker, TIME_OUT, Timeout.Infinite);

            worker = new();
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
        /// Se dispara si el timer llega al timeout, lo que cancelará el worker.
        /// </summary>
        /// <param name="state"></param>
        static void TimeoutCallback(object state)
        {
            BackgroundWorker worker = (BackgroundWorker)state;

            if (worker.IsBusy)
                worker.CancelAsync();
        }

        #endregion
    }
}
