using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.DataBase.Types;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class ChallengesManager
    {
        #region VARS

        static BackgroundWorker workerRefreshAsync;
        static BackgroundWorker workerStephAsync;

        #endregion

        #region EVENTS

        static void WorkerRefreshAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            TryToAddChallenge();
        }

        static void WorkerStepAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            StepPlayerRanking(e.Argument);
        }

        #endregion

        #region METHODS

        internal static void Refresh()
        {
            workerRefreshAsync = new()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            workerRefreshAsync.DoWork += WorkerRefreshAsync_DoWork;
            workerRefreshAsync.RunWorkerAsync();
        }

        static void Step(Challenge challenge)
        {
            workerStephAsync = new()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            workerStephAsync.DoWork += WorkerStepAsync_DoWork;
            workerStephAsync.RunWorkerAsync(challenge);
        }

        static void TryToAddChallenge()
        {
            List<GameMode> gameModes = ControllerScore.Get().Where(t => t.UserScore > 0).Select(t => t.GameMode).Distinct().ToList();/*Modos de juego jugados por el jugador*/
            List<ChallengesByGameMode> challengesByGameModes = DataBaseManager.GetUserChallengesByGameMode();/*Cantidad de retos por modo de juego*/
            List<GameMode> noChallengesMode = gameModes.Select(t => t).Except(challengesByGameModes.Select(t => t.GameMode)).ToList();/*Los que están jugados pero no tienen reto*/
            GameMode challengeGameMode = GameMode.None;
            List<GameMode> selectableGameModes = new();

            if (noChallengesMode.Any())/*Hay modos jugados sin reto*/
            {
                if (noChallengesMode.Count == 1)/*Si solo hay uno creamos un reto para ese modo*/
                {
                    selectableGameModes.Add(noChallengesMode[0]);
                }
                else/*De los modos no jugados cogemos uno aleatorio*/
                {
                    selectableGameModes.Add(noChallengesMode[Statics.GetRandom(0, noChallengesMode.Count - 1)]);
                }
            }
            else/*De entre los modos de juego que tienen menos de 9 retos*/
            {
                for (int i = 0; i < gameModes.Count; i++)
                {
                    if (challengesByGameModes.Any(t => t.GameMode == gameModes[i]))/*Hay algún reto activo para este modo de juego*/
                    {
                        if (challengesByGameModes.Single(t => t.GameMode == gameModes[i]).Counter < 9)/*Si tiene menos de 9 lo añadimos a los posibles*/
                            selectableGameModes.Add(gameModes[i]);
                    }
                    else
                        selectableGameModes.Add(gameModes[i]);/*Si hay ninguno activo para ese modo*/
                }
            }

            if (!selectableGameModes.Any()) /*Ya tiene el máximo de retos por modo de juego*/
                return;
            else if (selectableGameModes.Count == 1)
                challengeGameMode = selectableGameModes[0];
            else
                challengeGameMode = selectableGameModes[Statics.GetRandom(0, selectableGameModes.Count - 1)];

            GetChallengeData(challengeGameMode, out int playerID, out ChallengeType challengeType, out Score score);

            if (challengeType == ChallengeType.NoMistakes && challengeGameMode.IsIncremental())
                return;/*Los modos incrementales no pueden basarse en fallos, ya que al primer fallo se acaba la partida*/

            InsertChallenge(playerID, challengeType, challengeGameMode, score);
        }

        static void GetChallengeData(GameMode gameMode, out int playerID, out ChallengeType challengeType, out Score score)
        {
            challengeType = GetChallengeTypeByGameMode(gameMode);
            score = GetStageLevelByGameMode(gameMode);
            playerID = GetPlayerChallenger(gameMode);
        }

        static void InsertChallenge(int playerID, ChallengeType challengeType, GameMode challengeGameMode, Score score)
        {
            if (playerID != 0)
            {
                Challenge challenge = new()
                {
                    PlayerID = playerID,
                    ChallengeType = challengeType,
                    GameMode = challengeGameMode,
                    StageNumber = score.StageNumber,
                    LevelNumber = score.LevelNumber,
                    StartDate = DateTime.Now,
                    IsActive = true,
                    Win = false,
                };

                ControllerChallenge.Insert(challenge);
            }
        }

        /// <summary>
        /// Devuelve un jugador retador.
        /// </summary>
        /// <returns></returns>
        static int GetPlayerChallenger(GameMode gameMode)
        {
            List<RankingByGameMode> rankingByGameModes = ControllerRanking.GetWithPlayers(gameMode);
            int playerIndex = rankingByGameModes.FindIndex(t => t.IsPlayer);

            if (playerIndex < 20)
                return rankingByGameModes[Statics.GetRandom(playerIndex + 1, playerIndex + 10)].PlayerID;
            else if (playerIndex > rankingByGameModes.Count - 20)
                return rankingByGameModes[Statics.GetRandom(playerIndex - 10, playerIndex - 1)].PlayerID;
            else
                return rankingByGameModes[Statics.GetRandom(playerIndex + 1, playerIndex + 10)].PlayerID;
        }

        /// <summary>
        /// Devuelve una etapa y un nivel ya jugados por el jugador en un determinado modo de juego.
        /// </summary>
        /// <returns></returns>
        static Score GetStageLevelByGameMode(GameMode gameMode)
        {
            List<Score> scores = ControllerScore.Get(gameMode).Where(t => t.UserScore > 0).ToList();

            if (scores.Count == 1)/*Si hemos llegado hasta aquí es que al menos hay uno*/
                return scores[0];
            else
                return scores[Statics.GetRandom(0, scores.Count - 1)];

        }

        /// <summary>
        /// Devuelve un tipo de reto aleatorio de un modo de juego.
        /// </summary>
        /// <returns></returns>
        static ChallengeType GetChallengeTypeByGameMode(GameMode gameMode)
        {
            int number = Statics.GetRandom(1, 50);

            if (number <= 10)
                return ChallengeType.Stars;
            else if (number <= 20)
                return ChallengeType.Points;
            else if (number <= 30)
                return ChallengeType.NoPowerUps;
            else if (number <= 40)
                return ChallengeType.NoMistakes;
            else
                return gameMode.HasUnlimitedTiles() ? ChallengeType.Shapes : ChallengeType.Points;/*Si no tiene número total de fichas el modo*/
        }

        /// <summary>
        /// Obtiene la descripción del reto.
        /// </summary>
        /// <param name="challenge"></param>
        /// <returns></returns>
        internal static string GetDescription(Challenge challenge)
        {
            return challenge.ChallengeType switch
            {
                ChallengeType.Stars => Resource.String.CHALLENGE_STARS.GetString(challenge.StageNumber, challenge.LevelNumber),
                ChallengeType.Points => Resource.String.CHALLENGE_POINTS.GetString(challenge.StageNumber, challenge.LevelNumber, ControllerScore.Get(challenge.GameMode, challenge.StageNumber, challenge.LevelNumber).UserScore.ToInt()),
                ChallengeType.NoPowerUps => Resource.String.CHALLENGE_NOPOWERUPS.GetString(challenge.StageNumber, challenge.LevelNumber),
                ChallengeType.NoMistakes => Resource.String.CHALLENGE_NOMISTAKES.GetString(challenge.StageNumber, challenge.LevelNumber),
                ChallengeType.Shapes => Resource.String.CHALLENGE_SHAPES.GetString(challenge.StageNumber, challenge.LevelNumber),
                _ => "",
            };
        }

        /// <summary>
        /// Comprueba si un reto ha sido superado.
        /// </summary>
        /// <returns></returns>
        internal static bool IsChallengeCompleted(Challenge challenge, long points, int stars, int tilesFinded, int userMistakes, int powerUpsUsed)
        {
            if (challenge == null)
                return false;

            return challenge.ChallengeType switch
            {
                ChallengeType.Stars => stars == 3,
                ChallengeType.Points => ControllerScore.Get(challenge.GameMode, challenge.StageNumber, challenge.LevelNumber).UserScore <= points,/* <= porque los puntos se actualizan antes*/
                ChallengeType.NoPowerUps => powerUpsUsed.IsZero() && stars >= 2,
                ChallengeType.NoMistakes => userMistakes.IsZero() && stars >= 2,
                ChallengeType.Shapes => ControllerScore.Get(challenge.GameMode, challenge.StageNumber, challenge.LevelNumber).TilesFinded <= tilesFinded,/* <= porque los puntos se actualizan antes*/
                _ => false,
            };
        }

        /// <summary>
        /// El jugador gana 3 puntos y el retador pierde 2.
        /// </summary>
        internal static void ChallengeSuccess(this Challenge challenge)
        {
            challenge.IsActive = false;
            challenge.Win = true;
            ControllerChallenge.Update(challenge);
            Ranking ranking = ControllerRanking.Get(challenge.GameMode, 1/*El jugador siempre tiene id = 1*/);
            ranking.Win++;
            ControllerRanking.Update(ranking);
            ranking = ControllerRanking.Get(challenge.GameMode, challenge.PlayerID);
            ranking.Lose++;
            ControllerRanking.Update(ranking);
            Step(challenge);
        }

        /// <summary>
        /// El retador gana 3 puntos y el jugador pierde 2.
        /// </summary>
        internal static void ChallengeFailed(this Challenge challenge)
        {
            challenge.IsActive = false;
            challenge.Win = false;
            ControllerChallenge.Update(challenge);
            Ranking ranking = ControllerRanking.Get(challenge.GameMode, 1/*El jugador siempre tiene id = 1*/);
            ranking.Lose++;
            ControllerRanking.Update(ranking);
            ranking = ControllerRanking.Get(challenge.GameMode, challenge.PlayerID);
            ranking.Win++;
            ControllerRanking.Update(ranking);
            Step(challenge);
        }

        /// <summary>
        /// Actualiza el ranking del modo de juego como si todos hubieran jugado menos el del propio jugador y su retador que se habrá hecho en ChallengeFailed y ChallengeSuccess.
        /// </summary>
        static void StepPlayerRanking(object param)
        {
            Challenge challenge = (Challenge)param;/*El id del retador no hay que actualizar, se habrá hecho antes*/
            /*El jugador no hay que actualizarlo, se habrá hecho antes*/

            List<Ranking> rankings = ControllerRanking.Get(challenge.GameMode);
            List<RankingByGameMode> orderedRanking = ControllerRanking.GetWithPlayers(challenge.GameMode);
            bool win = false;

            DataBaseManager.Connection.BeginTransaction();/*Usamos una transacción para insertar todos los registros de golpe en bulk*/

            for (int i = 0; i < orderedRanking.Count; i++)
            {
                if (orderedRanking[i].PlayerID == challenge.PlayerID/*El retador*/ || orderedRanking[i].PlayerID == 1/*El jugador*/)
                    continue;

                if (i < 10)/*Los 10 primeros tienen una probabilidad mayor de obtener una vistoria*/
                    win = Statics.GetRandom(1, 10) > 3;
                else
                    win = Statics.GetRandom(1, 10) > 5;

                Ranking ranking = rankings.Single(t => t.PlayerID == orderedRanking[i].PlayerID);

                if (win)
                    ranking.Win++;
                else
                    ranking.Lose++;

                ControllerRanking.Update(ranking);
                /*Como también hay abandonos no es necesario que haya igual número de victorias que de derrotas*/
            }

            DataBaseManager.Connection.Commit();
        }

        #endregion
    }
}
