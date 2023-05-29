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

using ShapesAndColorsChallenge.Class.Params;
using ShapesAndColorsChallenge.DataBase.Controllers;
using ShapesAndColorsChallenge.DataBase.Tables;
using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace ShapesAndColorsChallenge.Class.Management
{
    internal static class AcheivementsManager
    {
        #region VARS

        static List<AcheivementResume> AcheivementResumes;
        static BackgroundWorker workerLoadAsync;
        static bool loading = false;

        #endregion

        #region PROPERTIES

        internal static List<AcheivementResume> GetResume
        {
            get
            {
                if (loading)
                    Thread.SpinWait(1);

                return AcheivementResumes;
            }
        }

        #endregion

        #region EVENTS

        private static void WorkerLoadAsync_DoWork(object sender, DoWorkEventArgs e)
        {
            loading = true;
            SetResume();
            loading = false;
        }

        #endregion

        #region METHODS

        internal static void Refresh()
        {
            workerLoadAsync = new()
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = false
            };
            workerLoadAsync.DoWork += WorkerLoadAsync_DoWork;
            workerLoadAsync.RunWorkerAsync();
        }

        /// <summary>
        /// Obtiene la cantidad de logros pendientes de reclamar por el usuario.
        /// </summary>
        /// <returns></returns>
        internal static int GetTotalPendingToClaim()
        {
            while (loading)
                Thread.SpinWait(1);

            return AcheivementResumes.Count(t => t.Pending > 0);
        }

        /// <summary>
        /// Establece la información de todos los logros.
        /// </summary>
        /// <returns></returns>
        static void SetResume()
        {
            List<Acheivement> acheivements = ControllerAcheivement.Get();
            List<Score> scores = ControllerScore.Get();

            AcheivementResumes = new()
            {
                FirstStar(acheivements, scores),
                StarCollector(acheivements, scores),
                AllInMode(acheivements, scores, AcheivementType.AllInClassic, GameMode.Classic),
                AllInMode(acheivements, scores, AcheivementType.AllInEndless, GameMode.Endless),
                AllInMode(acheivements, scores, AcheivementType.AllInMemory, GameMode.Memory),
                AllInMode(acheivements, scores, AcheivementType.AllInTimeTrial, GameMode.TimeTrial),
                AllInMode(acheivements, scores, AcheivementType.AllInIncremental, GameMode.Incremental),
                AllInMode(acheivements, scores, AcheivementType.AllInMove, GameMode.Move),
                AllInMode(acheivements, scores, AcheivementType.AllInBlink, GameMode.Blink),
                AllInMode(acheivements, scores, AcheivementType.AllInRotate, GameMode.Rotate),
                AllInMode(acheivements, scores, AcheivementType.AllInClassicPlus, GameMode.ClassicPlus),
                AllInMode(acheivements, scores, AcheivementType.AllInEndlessPlus, GameMode.EndlessPlus),
                AllInMode(acheivements, scores, AcheivementType.AllInMemoryPlus, GameMode.MemoryPlus),
                AllInMode(acheivements, scores, AcheivementType.AllInTimeTrialPlus, GameMode.TimeTrialPlus),
                AllInMode(acheivements, scores, AcheivementType.AllInIncrementalPlus, GameMode.IncrementalPlus),
                AllInMode(acheivements, scores, AcheivementType.AllInMovePlus, GameMode.MovePlus),
                AllInMode(acheivements, scores, AcheivementType.AllInBlinkPlus, GameMode.BlinkPlus),
                AllInMode(acheivements, scores, AcheivementType.AllInRotatePlus, GameMode.RotatePlus),
                AllInNormal(acheivements, scores),
                AllInPlus(acheivements, scores),
                AllInAll(acheivements, scores),
                FirstLevel(acheivements, scores),
                FirstInMode(acheivements, scores, AcheivementType.FirstInClassic, GameMode.Classic),
                FirstInMode(acheivements, scores, AcheivementType.FirstInEndless, GameMode.Endless),
                FirstInMode(acheivements, scores, AcheivementType.FirstInMemory, GameMode.Memory),
                FirstInMode(acheivements, scores, AcheivementType.FirstInTimeTrial, GameMode.TimeTrial),
                FirstInMode(acheivements, scores, AcheivementType.FirstInIncremental, GameMode.Incremental),
                FirstInMode(acheivements, scores, AcheivementType.FirstInMove, GameMode.Move),
                FirstInMode(acheivements, scores, AcheivementType.FirstInBlink, GameMode.Blink),
                FirstInMode(acheivements, scores, AcheivementType.FirstInRotate, GameMode.Rotate),
                FirstInMode(acheivements, scores, AcheivementType.FirstInClassicPlus, GameMode.ClassicPlus),
                FirstInMode(acheivements, scores, AcheivementType.FirstInEndlessPlus, GameMode.EndlessPlus),
                FirstInMode(acheivements, scores, AcheivementType.FirstInMemoryPlus, GameMode.MemoryPlus),
                FirstInMode(acheivements, scores, AcheivementType.FirstInTimeTrialPlus, GameMode.TimeTrialPlus),
                FirstInMode(acheivements, scores, AcheivementType.FirstInIncrementalPlus, GameMode.IncrementalPlus),
                FirstInMode(acheivements, scores, AcheivementType.FirstInMovePlus, GameMode.MovePlus),
                FirstInMode(acheivements, scores, AcheivementType.FirstInBlinkPlus, GameMode.BlinkPlus),
                FirstInMode(acheivements, scores, AcheivementType.FirstInRotatePlus, GameMode.RotatePlus),
                FirstInNormal(acheivements, scores),
                FirstInPlus(acheivements, scores),
                FirstInAll(acheivements, scores),
                FirstChallenge(acheivements),
                Challenger(acheivements)
            };

            AcheivementResumes = AcheivementResumes.OrderByDescending(t => t.Pending).ThenBy(t => t.Completed).ToList();
        }

        internal static AcheivementResume GetAcheivementResume(AcheivementType acheivementType)
        {
            List<Acheivement> acheivements = ControllerAcheivement.Get();
            List<Score> scores = ControllerScore.Get();

            return acheivementType switch
            {
                AcheivementType.FirstStar => FirstStar(acheivements, scores),
                AcheivementType.StarCollector => StarCollector(acheivements, scores),
                AcheivementType.AllInClassic => AllInMode(acheivements, scores, AcheivementType.AllInClassic, GameMode.Classic),
                AcheivementType.AllInEndless => AllInMode(acheivements, scores, AcheivementType.AllInEndless, GameMode.Endless),
                AcheivementType.AllInMemory => AllInMode(acheivements, scores, AcheivementType.AllInMemory, GameMode.Memory),
                AcheivementType.AllInTimeTrial => AllInMode(acheivements, scores, AcheivementType.AllInTimeTrial, GameMode.TimeTrial),
                AcheivementType.AllInIncremental => AllInMode(acheivements, scores, AcheivementType.AllInIncremental, GameMode.Incremental),
                AcheivementType.AllInMove => AllInMode(acheivements, scores, AcheivementType.AllInMove, GameMode.Move),
                AcheivementType.AllInBlink => AllInMode(acheivements, scores, AcheivementType.AllInBlink, GameMode.Blink),
                AcheivementType.AllInRotate => AllInMode(acheivements, scores, AcheivementType.AllInRotate, GameMode.Rotate),
                AcheivementType.AllInClassicPlus => AllInMode(acheivements, scores, AcheivementType.AllInClassicPlus, GameMode.ClassicPlus),
                AcheivementType.AllInEndlessPlus => AllInMode(acheivements, scores, AcheivementType.AllInEndlessPlus, GameMode.EndlessPlus),
                AcheivementType.AllInMemoryPlus => AllInMode(acheivements, scores, AcheivementType.AllInMemoryPlus, GameMode.MemoryPlus),
                AcheivementType.AllInTimeTrialPlus => AllInMode(acheivements, scores, AcheivementType.AllInTimeTrialPlus, GameMode.TimeTrialPlus),
                AcheivementType.AllInIncrementalPlus => AllInMode(acheivements, scores, AcheivementType.AllInIncrementalPlus, GameMode.IncrementalPlus),
                AcheivementType.AllInMovePlus => AllInMode(acheivements, scores, AcheivementType.AllInMovePlus, GameMode.MovePlus),
                AcheivementType.AllInBlinkPlus => AllInMode(acheivements, scores, AcheivementType.AllInBlinkPlus, GameMode.BlinkPlus),
                AcheivementType.AllInRotatePlus => AllInMode(acheivements, scores, AcheivementType.AllInRotatePlus, GameMode.RotatePlus),
                AcheivementType.AllInNormal => AllInNormal(acheivements, scores),
                AcheivementType.AllInPlus => AllInPlus(acheivements, scores),
                AcheivementType.AllInAll => AllInAll(acheivements, scores),
                AcheivementType.FirstLevel => FirstLevel(acheivements, scores),
                AcheivementType.FirstInClassic => FirstInMode(acheivements, scores, AcheivementType.FirstInClassic, GameMode.Classic),
                AcheivementType.FirstInEndless => FirstInMode(acheivements, scores, AcheivementType.FirstInEndless, GameMode.Endless),
                AcheivementType.FirstInMemory => FirstInMode(acheivements, scores, AcheivementType.FirstInMemory, GameMode.Memory),
                AcheivementType.FirstInTimeTrial => FirstInMode(acheivements, scores, AcheivementType.FirstInTimeTrial, GameMode.TimeTrial),
                AcheivementType.FirstInIncremental => FirstInMode(acheivements, scores, AcheivementType.FirstInIncremental, GameMode.Incremental),
                AcheivementType.FirstInMove => FirstInMode(acheivements, scores, AcheivementType.FirstInMove, GameMode.Move),
                AcheivementType.FirstInBlink => FirstInMode(acheivements, scores, AcheivementType.FirstInBlink, GameMode.Blink),
                AcheivementType.FirstInRotate => FirstInMode(acheivements, scores, AcheivementType.FirstInRotate, GameMode.Rotate),
                AcheivementType.FirstInClassicPlus => FirstInMode(acheivements, scores, AcheivementType.FirstInClassicPlus, GameMode.ClassicPlus),
                AcheivementType.FirstInEndlessPlus => FirstInMode(acheivements, scores, AcheivementType.FirstInEndlessPlus, GameMode.EndlessPlus),
                AcheivementType.FirstInMemoryPlus => FirstInMode(acheivements, scores, AcheivementType.FirstInMemoryPlus, GameMode.MemoryPlus),
                AcheivementType.FirstInTimeTrialPlus => FirstInMode(acheivements, scores, AcheivementType.FirstInTimeTrialPlus, GameMode.TimeTrialPlus),
                AcheivementType.FirstInIncrementalPlus => FirstInMode(acheivements, scores, AcheivementType.FirstInIncrementalPlus, GameMode.IncrementalPlus),
                AcheivementType.FirstInMovePlus => FirstInMode(acheivements, scores, AcheivementType.FirstInMovePlus, GameMode.MovePlus),
                AcheivementType.FirstInBlinkPlus => FirstInMode(acheivements, scores, AcheivementType.FirstInBlinkPlus, GameMode.BlinkPlus),
                AcheivementType.FirstInRotatePlus => FirstInMode(acheivements, scores, AcheivementType.FirstInRotatePlus, GameMode.RotatePlus),
                AcheivementType.FirstInNormal => FirstInNormal(acheivements, scores),
                AcheivementType.FirstInPlus => FirstInPlus(acheivements, scores),
                AcheivementType.FirstInAll => FirstInAll(acheivements, scores),
                AcheivementType.FirstChallenge => FirstChallenge(acheivements),
                AcheivementType.Challenger => Challenger(acheivements),
                _ => null
            };
        }

        /// <summary>
        /// Cuando se consigue la primera estrella.
        /// </summary>
        static AcheivementResume FirstStar(List<Acheivement> acheivements, List<Score> scores)
        {
            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.FirstStar), RewardType.RandomPerk)
            {
                Max = 1,
                Acheived = scores.Sum(t => t.Stars) > 0 ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Cada 5 estrellas, hasta conseguir el máximo del juego, LEVELS * STAGES * MODES * 3
        /// </summary>
        static AcheivementResume StarCollector(List<Acheivement> acheivements, List<Score> scores)
        {
            int maxStars = GameData.LEVELS * GameData.STAGES * GameData.STARS_PER_LEVEL * GameData.MODES;
            int acheivedStars = scores.Sum(t => t.Stars);
            int acheived = default;
            int max = 0;

            if (acheivedStars >= 5)
            {
                if (maxStars % 5 != 0)
                {
                    acheived = Math.Floor(acheivedStars / 5f).ToInt() + 1;
                    max = Math.Floor(maxStars / 5f).ToInt() + 1;
                }
                else
                {
                    acheived = acheivedStars / 5;
                    max = maxStars / 5;
                }
            }
            else
                max = Math.Floor(maxStars / 5f).ToInt() + 1;

            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.StarCollector), RewardType.RandomPerk)
            {
                Max = max,
                Acheived = acheived
            };

            return acheivementResume;
        }

        /// <summary>
        /// Todas en cada modo de juego.
        /// </summary>
        static AcheivementResume AllInMode(List<Acheivement> acheivements, List<Score> scores, AcheivementType acheivementType, GameMode gameMode)
        {
            int maxStars = GameData.LEVELS * GameData.STAGES * GameData.STARS_PER_LEVEL;

            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == acheivementType), RewardType.AllPerks)
            {
                Max = 1,
                Acheived = scores.Where(t => t.GameMode == gameMode).Sum(t => t.Stars) == maxStars ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Todas en todos los modos normal.
        /// </summary>
        static AcheivementResume AllInNormal(List<Acheivement> acheivements, List<Score> scores)
        {
            int maxStars = GameData.LEVELS * GameData.STAGES * GameData.STARS_PER_LEVEL * GameData.MODES_NORMAL;

            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.AllInNormal), RewardType.AllPerks)
            {
                Max = 1,
                Acheived = scores.Where(t => t.GameMode.IsIn(GameMode.Classic, GameMode.Rotate, GameMode.TimeTrial, GameMode.Blink, GameMode.Endless, GameMode.Incremental, GameMode.Memory, GameMode.Move)).Sum(t => t.Stars) == maxStars ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Todas en todos los modos plus.
        /// </summary>
        static AcheivementResume AllInPlus(List<Acheivement> acheivements, List<Score> scores)
        {
            int maxStars = GameData.LEVELS * GameData.STAGES * GameData.STARS_PER_LEVEL * GameData.MODES_NORMAL;

            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.AllInPlus), RewardType.AllPerks)
            {
                Max = 1,
                Acheived = scores.Where(t => t.GameMode.IsIn(GameMode.ClassicPlus, GameMode.RotatePlus, GameMode.TimeTrialPlus, GameMode.BlinkPlus, GameMode.EndlessPlus, GameMode.IncrementalPlus, GameMode.MemoryPlus, GameMode.MovePlus)).Sum(t => t.Stars) == maxStars ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Todas en todos los modos.
        /// </summary>
        static AcheivementResume AllInAll(List<Acheivement> acheivements, List<Score> scores)
        {
            int maxStars = GameData.LEVELS * GameData.STAGES * GameData.STARS_PER_LEVEL * GameData.MODES;

            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.AllInAll), RewardType.AllPerks)
            {
                Max = 1,
                Acheived = scores.Sum(t => t.Stars) == maxStars ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Al conseguir la primera estrella en un modo cualquiera.
        /// </summary>
        static AcheivementResume FirstLevel(List<Acheivement> acheivements, List<Score> scores)
        {
            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.FirstLevel), RewardType.RandomPerk)
            {
                Max = 1,
                Acheived = scores.Sum(t => t.Stars) > 0 ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Al conseguir una estrella en cualquier nivel de cada modo.
        /// </summary>
        static AcheivementResume FirstInMode(List<Acheivement> acheivements, List<Score> scores, AcheivementType acheivementType, GameMode gameMode)
        {
            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == acheivementType), RewardType.RandomPerk)
            {
                Max = 1,
                Acheived = scores.Where(t => t.GameMode == gameMode).Sum(t => t.Stars) > 0 ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Por conseguir una estrella en cada nivel de todos los modos nornal.
        /// </summary>
        static AcheivementResume FirstInNormal(List<Acheivement> acheivements, List<Score> scores)
        {
            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.FirstInNormal), RewardType.AllPerks)
            {
                Max = 1,
                Acheived =
                (scores.Where(t => t.GameMode == GameMode.Classic).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Rotate).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.TimeTrial).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Blink).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Endless).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Incremental).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Memory).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Move).Sum(t => t.Stars) > 0 ? 1 : 0) >= GameData.MODES_NORMAL ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Por conseguir una estrella en cada nivel de todos los modos plus.
        /// </summary>
        static AcheivementResume FirstInPlus(List<Acheivement> acheivements, List<Score> scores)
        {
            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.FirstInPlus), RewardType.AllPerks)
            {
                Max = 1,
                Acheived =
                (scores.Where(t => t.GameMode == GameMode.ClassicPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.RotatePlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.TimeTrialPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.BlinkPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.EndlessPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.IncrementalPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.MemoryPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.MovePlus).Sum(t => t.Stars) > 0 ? 1 : 0) >= GameData.MODES_PLUS ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Por conseguir una estrella en cada nivel de todos los modos.
        /// </summary>
        static AcheivementResume FirstInAll(List<Acheivement> acheivements, List<Score> scores)
        {
            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.FirstInAll), RewardType.AllPerks)
            {
                Max = 1,
                Acheived =
                (scores.Where(t => t.GameMode == GameMode.Classic).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Rotate).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.TimeTrial).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Blink).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Endless).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Incremental).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Memory).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.Move).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.ClassicPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.RotatePlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.TimeTrialPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.BlinkPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.EndlessPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.IncrementalPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.MemoryPlus).Sum(t => t.Stars) > 0 ? 1 : 0) +
                (scores.Where(t => t.GameMode == GameMode.MovePlus).Sum(t => t.Stars) > 0 ? 1 : 0) >= GameData.MODES ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Por superar el primer reto.
        /// </summary>
        static AcheivementResume FirstChallenge(List<Acheivement> acheivements)
        {
            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.FirstChallenge), RewardType.RandomPerk)
            {
                Max = 1,
                Acheived = ControllerRanking.GetUserRanking().Sum(t => t.Win) > 0 ? 1 : 0
            };

            return acheivementResume;
        }

        /// <summary>
        /// Por cada 5 retos superados sin fin.
        /// </summary>
        static AcheivementResume Challenger(List<Acheivement> acheivements)
        {
            AcheivementResume acheivementResume = new(acheivements.Single(t => t.Type == AcheivementType.Challenger), RewardType.RandomPerk)
            {
                Max = int.MaxValue,
                Acheived = (int)Math.Floor(ControllerRanking.GetUserRanking().Sum(t => t.Win) / 5f)
            };

            return acheivementResume;
        }

        /// <summary>
        /// Actualiza los potenciadores con los obtenidos por uno o varios logros.
        /// También actualiza el logro.
        /// </summary>
        /// <param name="acheivementResume"></param>
        /// <returns></returns>
        internal static WindowRewardParams RewardAllPerks(AcheivementResume acheivementResume)
        {
            int change = acheivementResume.Pending;
            int reveal = acheivementResume.Pending;
            int timeStop = acheivementResume.Pending;

            Update(acheivementResume);

            return new(PerkType.Change, reveal, PerkType.Reveal, change, PerkType.TimeStop, timeStop);
        }

        /// <summary>
        /// Actualiza los potenciadores con los obtenidos por uno o varios logros.
        /// También actualiza el logro.
        /// </summary>
        /// <param name="acheivementResume"></param>
        /// <returns></returns>
        internal static WindowRewardParams RewardRandomPerks(AcheivementResume acheivementResume)
        {
            int change = 0, reveal = 0, timeStop = 0;

            for (int i = 0; i < acheivementResume.Pending; i++)
            {
                int type = Statics.GetRandom(1, 3);

                switch (type)
                {
                    case 1:
                        change++;
                        break;
                    case 2:
                        reveal++;
                        break;
                    case 3:
                        timeStop++;
                        break;
                }
            }

            Update(acheivementResume);
            WindowRewardParams windowRewardParams = new();

            if (reveal.NotIsZero())
            {
                windowRewardParams.Amount1 = reveal;
                windowRewardParams.Reward1 = PerkType.Reveal;

                if (change.NotIsZero())
                {
                    windowRewardParams.Amount2 = change;
                    windowRewardParams.Reward2 = PerkType.Change;
                    windowRewardParams.Amount3 = timeStop;
                    windowRewardParams.Reward3 = PerkType.TimeStop;
                }
                else
                {
                    windowRewardParams.Amount2 = timeStop;
                    windowRewardParams.Reward2 = PerkType.TimeStop;
                    windowRewardParams.Amount3 = change;
                    windowRewardParams.Reward3 = PerkType.Change;
                }

                return windowRewardParams;
            }

            if (change.NotIsZero())
                return new(PerkType.Change, change, PerkType.TimeStop, timeStop, PerkType.Reveal, reveal);
            else
                return new(PerkType.TimeStop, timeStop, PerkType.Change, change, PerkType.Reveal, reveal);
        }

        static void Update(AcheivementResume acheivementResume)
        {
            Acheivement acheivement = ControllerAcheivement.Get(acheivementResume.AcheivementType);
            acheivement.Claimed += acheivementResume.Pending;
            ControllerAcheivement.Update(acheivement);

            Perk perkReveal = ControllerPerk.Get(PerkType.Reveal);
            perkReveal.Amount += acheivementResume.Pending;
            ControllerPerk.Update(perkReveal);

            Perk perkChange = ControllerPerk.Get(PerkType.Change);
            perkChange.Amount += acheivementResume.Pending;
            ControllerPerk.Update(perkChange);

            Perk perkTimeStop = ControllerPerk.Get(PerkType.TimeStop);
            perkTimeStop.Amount += acheivementResume.Pending;
            ControllerPerk.Update(perkTimeStop);
        }

        internal static string GetAcheivementName(AcheivementResume acheivementResume)
        {
            return acheivementResume.AcheivementType switch
            {
                AcheivementType.FirstStar => Resource.String.ACHEIVEMENT_FirstStar.GetString(),
                AcheivementType.StarCollector => Resource.String.ACHEIVEMENT_StarCollector.GetString(),
                AcheivementType.AllInClassic => Resource.String.ACHEIVEMENT_AllInClassic.GetString(),
                AcheivementType.AllInEndless => Resource.String.ACHEIVEMENT_AllInEndless.GetString(),
                AcheivementType.AllInMemory => Resource.String.ACHEIVEMENT_AllInMemory.GetString(),
                AcheivementType.AllInTimeTrial => Resource.String.ACHEIVEMENT_AllInTimeTrial.GetString(),
                AcheivementType.AllInIncremental => Resource.String.ACHEIVEMENT_AllInIncremental.GetString(),
                AcheivementType.AllInMove => Resource.String.ACHEIVEMENT_AllInMove.GetString(),
                AcheivementType.AllInBlink => Resource.String.ACHEIVEMENT_AllInBlink.GetString(),
                AcheivementType.AllInRotate => Resource.String.ACHEIVEMENT_AllInRotate.GetString(),
                AcheivementType.AllInClassicPlus => Resource.String.ACHEIVEMENT_AllInClassicPlus.GetString(),
                AcheivementType.AllInEndlessPlus => Resource.String.ACHEIVEMENT_AllInEndlessPlus.GetString(),
                AcheivementType.AllInMemoryPlus => Resource.String.ACHEIVEMENT_AllInMemoryPlus.GetString(),
                AcheivementType.AllInTimeTrialPlus => Resource.String.ACHEIVEMENT_AllInTimeTrialPlus.GetString(),
                AcheivementType.AllInIncrementalPlus => Resource.String.ACHEIVEMENT_AllInIncrementalPlus.GetString(),
                AcheivementType.AllInMovePlus => Resource.String.ACHEIVEMENT_AllInMovePlus.GetString(),
                AcheivementType.AllInBlinkPlus => Resource.String.ACHEIVEMENT_AllInBlinkPlus.GetString(),
                AcheivementType.AllInRotatePlus => Resource.String.ACHEIVEMENT_AllInRotatePlus.GetString(),
                AcheivementType.AllInNormal => Resource.String.ACHEIVEMENT_AllInNormal.GetString(),
                AcheivementType.AllInPlus => Resource.String.ACHEIVEMENT_AllInPlus.GetString(),
                AcheivementType.AllInAll => Resource.String.ACHEIVEMENT_AllInAll.GetString(),
                AcheivementType.FirstLevel => Resource.String.ACHEIVEMENT_FirstLevel.GetString(),
                AcheivementType.FirstInClassic => Resource.String.ACHEIVEMENT_FirstInClassic.GetString(),
                AcheivementType.FirstInEndless => Resource.String.ACHEIVEMENT_FirstInEndless.GetString(),
                AcheivementType.FirstInMemory => Resource.String.ACHEIVEMENT_FirstInMemory.GetString(),
                AcheivementType.FirstInTimeTrial => Resource.String.ACHEIVEMENT_FirstInTimeTrial.GetString(),
                AcheivementType.FirstInIncremental => Resource.String.ACHEIVEMENT_FirstInIncremental.GetString(),
                AcheivementType.FirstInMove => Resource.String.ACHEIVEMENT_FirstInMove.GetString(),
                AcheivementType.FirstInBlink => Resource.String.ACHEIVEMENT_FirstInBlink.GetString(),
                AcheivementType.FirstInRotate => Resource.String.ACHEIVEMENT_FirstInRotate.GetString(),
                AcheivementType.FirstInClassicPlus => Resource.String.ACHEIVEMENT_FirstInClassicPlus.GetString(),
                AcheivementType.FirstInEndlessPlus => Resource.String.ACHEIVEMENT_FirstInEndlessPlus.GetString(),
                AcheivementType.FirstInMemoryPlus => Resource.String.ACHEIVEMENT_FirstInMemoryPlus.GetString(),
                AcheivementType.FirstInTimeTrialPlus => Resource.String.ACHEIVEMENT_FirstInTimeTrialPlus.GetString(),
                AcheivementType.FirstInIncrementalPlus => Resource.String.ACHEIVEMENT_FirstInIncrementalPlus.GetString(),
                AcheivementType.FirstInMovePlus => Resource.String.ACHEIVEMENT_FirstInMovePlus.GetString(),
                AcheivementType.FirstInBlinkPlus => Resource.String.ACHEIVEMENT_FirstInBlinkPlus.GetString(),
                AcheivementType.FirstInRotatePlus => Resource.String.ACHEIVEMENT_FirstInRotatePlus.GetString(),
                AcheivementType.FirstInNormal => Resource.String.ACHEIVEMENT_FirstInNormal.GetString(),
                AcheivementType.FirstInPlus => Resource.String.ACHEIVEMENT_FirstInPlus.GetString(),
                AcheivementType.FirstInAll => Resource.String.ACHEIVEMENT_FirstInAll.GetString(),
                AcheivementType.FirstChallenge => Resource.String.ACHEIVEMENT_FirstChallenge.GetString(),
                AcheivementType.Challenger => Resource.String.ACHEIVEMENT_Challenger.GetString(),
                _ => string.Empty,
            };
        }

        internal static string GetAcheivementDescription(AcheivementResume acheivementResume)
        {
            return acheivementResume.AcheivementType switch
            {
                AcheivementType.FirstStar => Resource.String.ACHEIVEMENT_FirstStar_DES.GetString(),
                AcheivementType.StarCollector => Resource.String.ACHEIVEMENT_StarCollector_DES.GetString(),
                AcheivementType.AllInClassic => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.CLASSIC_MODE.GetString()),
                AcheivementType.AllInEndless => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.ENDLESS_MODE.GetString()),
                AcheivementType.AllInMemory => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.MEMORY_MODE.GetString()),
                AcheivementType.AllInTimeTrial => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.TIMETRIAL_MODE.GetString()),
                AcheivementType.AllInIncremental => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.INCREMENTAL_MODE.GetString()),
                AcheivementType.AllInMove => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.MOVE_MODE.GetString()),
                AcheivementType.AllInBlink => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.BLINK_MODE.GetString()),
                AcheivementType.AllInRotate => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.ROTATE_MODE.GetString()),
                AcheivementType.AllInClassicPlus => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.CLASSIC_MODE_PLUS.GetString()),
                AcheivementType.AllInEndlessPlus => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.ENDLESS_MODE_PLUS.GetString()),
                AcheivementType.AllInMemoryPlus => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.MEMORY_MODE_PLUS.GetString()),
                AcheivementType.AllInTimeTrialPlus => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.TIMETRIAL_MODE_PLUS.GetString()),
                AcheivementType.AllInIncrementalPlus => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.INCREMENTAL_MODE_PLUS.GetString()),
                AcheivementType.AllInMovePlus => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.MOVE_MODE_PLUS.GetString()),
                AcheivementType.AllInBlinkPlus => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.BLINK_MODE_PLUS.GetString()),
                AcheivementType.AllInRotatePlus => Resource.String.ACHEIVEMENT_AllInMode_DES.GetString(Resource.String.ROTATE_MODE_PLUS.GetString()),
                AcheivementType.AllInNormal => Resource.String.ACHEIVEMENT_AllInNormal_DES.GetString(),
                AcheivementType.AllInPlus => Resource.String.ACHEIVEMENT_AllInPlus_DES.GetString(),
                AcheivementType.AllInAll => Resource.String.ACHEIVEMENT_AllInAll_DES.GetString(),
                AcheivementType.FirstLevel => Resource.String.ACHEIVEMENT_FirstLevel_DES.GetString(),
                AcheivementType.FirstInClassic => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.CLASSIC_MODE.GetString()),
                AcheivementType.FirstInEndless => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.ENDLESS_MODE.GetString()),
                AcheivementType.FirstInMemory => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.MEMORY_MODE.GetString()),
                AcheivementType.FirstInTimeTrial => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.TIMETRIAL_MODE.GetString()),
                AcheivementType.FirstInIncremental => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.INCREMENTAL_MODE.GetString()),
                AcheivementType.FirstInMove => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.MOVE_MODE.GetString()),
                AcheivementType.FirstInBlink => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.BLINK_MODE.GetString()),
                AcheivementType.FirstInRotate => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.ROTATE_MODE.GetString()),
                AcheivementType.FirstInClassicPlus => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.CLASSIC_MODE_PLUS.GetString()),
                AcheivementType.FirstInEndlessPlus => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.ENDLESS_MODE_PLUS.GetString()),
                AcheivementType.FirstInMemoryPlus => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.MEMORY_MODE_PLUS.GetString()),
                AcheivementType.FirstInTimeTrialPlus => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.TIMETRIAL_MODE_PLUS.GetString()),
                AcheivementType.FirstInIncrementalPlus => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.INCREMENTAL_MODE_PLUS.GetString()),
                AcheivementType.FirstInMovePlus => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.MOVE_MODE_PLUS.GetString()),
                AcheivementType.FirstInBlinkPlus => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.BLINK_MODE_PLUS.GetString()),
                AcheivementType.FirstInRotatePlus => Resource.String.ACHEIVEMENT_FirstInMode_DES.GetString(Resource.String.ROTATE_MODE_PLUS.GetString()),
                AcheivementType.FirstInNormal => Resource.String.ACHEIVEMENT_FirstInNormal_DES.GetString(),
                AcheivementType.FirstInPlus => Resource.String.ACHEIVEMENT_FirstInPlus_DES.GetString(),
                AcheivementType.FirstInAll => Resource.String.ACHEIVEMENT_FirstInAll_DES.GetString(),
                AcheivementType.FirstChallenge => Resource.String.ACHEIVEMENT_FirstChallenge_DES.GetString(),
                AcheivementType.Challenger => Resource.String.ACHEIVEMENT_Challenger_DES.GetString(),
                _ => string.Empty,
            };
        }

        #endregion
    }
}
