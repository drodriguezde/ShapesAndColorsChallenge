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
                AcheivementType.FirstStar => LanguageManager.Get("ACHEIVEMENT_FirstStar"),
                AcheivementType.StarCollector => LanguageManager.Get("ACHEIVEMENT_StarCollector"),
                AcheivementType.AllInClassic => LanguageManager.Get("ACHEIVEMENT_AllInClassic"),
                AcheivementType.AllInEndless => LanguageManager.Get("ACHEIVEMENT_AllInEndless"),
                AcheivementType.AllInMemory => LanguageManager.Get("ACHEIVEMENT_AllInMemory"),
                AcheivementType.AllInTimeTrial => LanguageManager.Get("ACHEIVEMENT_AllInTimeTrial"),
                AcheivementType.AllInIncremental => LanguageManager.Get("ACHEIVEMENT_AllInIncremental"),
                AcheivementType.AllInMove => LanguageManager.Get("ACHEIVEMENT_AllInMove"),
                AcheivementType.AllInBlink => LanguageManager.Get("ACHEIVEMENT_AllInBlink"),
                AcheivementType.AllInRotate => LanguageManager.Get("ACHEIVEMENT_AllInRotate"),
                AcheivementType.AllInClassicPlus => LanguageManager.Get("ACHEIVEMENT_AllInClassicPlus"),
                AcheivementType.AllInEndlessPlus => LanguageManager.Get("ACHEIVEMENT_AllInEndlessPlus"),
                AcheivementType.AllInMemoryPlus => LanguageManager.Get("ACHEIVEMENT_AllInMemoryPlus"),
                AcheivementType.AllInTimeTrialPlus => LanguageManager.Get("ACHEIVEMENT_AllInTimeTrialPlus"),
                AcheivementType.AllInIncrementalPlus => LanguageManager.Get("ACHEIVEMENT_AllInIncrementalPlus"),
                AcheivementType.AllInMovePlus => LanguageManager.Get("ACHEIVEMENT_AllInMovePlus"),
                AcheivementType.AllInBlinkPlus => LanguageManager.Get("ACHEIVEMENT_AllInBlinkPlus"),
                AcheivementType.AllInRotatePlus => LanguageManager.Get("ACHEIVEMENT_AllInRotatePlus"),
                AcheivementType.AllInNormal => LanguageManager.Get("ACHEIVEMENT_AllInNormal"),
                AcheivementType.AllInPlus => LanguageManager.Get("ACHEIVEMENT_AllInPlus"),
                AcheivementType.AllInAll => LanguageManager.Get("ACHEIVEMENT_AllInAll"),
                AcheivementType.FirstLevel => LanguageManager.Get("ACHEIVEMENT_FirstLevel"),
                AcheivementType.FirstInClassic => LanguageManager.Get("ACHEIVEMENT_FirstInClassic"),
                AcheivementType.FirstInEndless => LanguageManager.Get("ACHEIVEMENT_FirstInEndless"),
                AcheivementType.FirstInMemory => LanguageManager.Get("ACHEIVEMENT_FirstInMemory"),
                AcheivementType.FirstInTimeTrial => LanguageManager.Get("ACHEIVEMENT_FirstInTimeTrial"),
                AcheivementType.FirstInIncremental => LanguageManager.Get("ACHEIVEMENT_FirstInIncremental"),
                AcheivementType.FirstInMove => LanguageManager.Get("ACHEIVEMENT_FirstInMove"),
                AcheivementType.FirstInBlink => LanguageManager.Get("ACHEIVEMENT_FirstInBlink"),
                AcheivementType.FirstInRotate => LanguageManager.Get("ACHEIVEMENT_FirstInRotate"),
                AcheivementType.FirstInClassicPlus => LanguageManager.Get("ACHEIVEMENT_FirstInClassicPlus"),
                AcheivementType.FirstInEndlessPlus => LanguageManager.Get("ACHEIVEMENT_FirstInEndlessPlus"),
                AcheivementType.FirstInMemoryPlus => LanguageManager.Get("ACHEIVEMENT_FirstInMemoryPlus"),
                AcheivementType.FirstInTimeTrialPlus => LanguageManager.Get("ACHEIVEMENT_FirstInTimeTrialPlus"),
                AcheivementType.FirstInIncrementalPlus => LanguageManager.Get("ACHEIVEMENT_FirstInIncrementalPlus"),
                AcheivementType.FirstInMovePlus => LanguageManager.Get("ACHEIVEMENT_FirstInMovePlus"),
                AcheivementType.FirstInBlinkPlus => LanguageManager.Get("ACHEIVEMENT_FirstInBlinkPlus"),
                AcheivementType.FirstInRotatePlus => LanguageManager.Get("ACHEIVEMENT_FirstInRotatePlus"),
                AcheivementType.FirstInNormal => LanguageManager.Get("ACHEIVEMENT_FirstInNormal"),
                AcheivementType.FirstInPlus => LanguageManager.Get("ACHEIVEMENT_FirstInPlus"),
                AcheivementType.FirstInAll => LanguageManager.Get("ACHEIVEMENT_FirstInAll"),
                AcheivementType.FirstChallenge => LanguageManager.Get("ACHEIVEMENT_FirstChallenge"),
                AcheivementType.Challenger => LanguageManager.Get("ACHEIVEMENT_Challenger"),
                _ => string.Empty,
            };
        }

        internal static string GetAcheivementDescription(AcheivementResume acheivementResume)
        {
            return acheivementResume.AcheivementType switch
            {
                AcheivementType.FirstStar => LanguageManager.Get("ACHEIVEMENT_FirstStar_DES"),
                AcheivementType.StarCollector => LanguageManager.Get("ACHEIVEMENT_StarCollector_DES"),
                AcheivementType.AllInClassic => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("CLASSIC_MODE")),
                AcheivementType.AllInEndless => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("ENDLESS_MODE")),
                AcheivementType.AllInMemory => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("MEMORY_MODE")),
                AcheivementType.AllInTimeTrial => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("TIMETRIAL_MODE")),
                AcheivementType.AllInIncremental => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("INCREMENTAL_MODE")),
                AcheivementType.AllInMove => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("MOVE_MODE")),
                AcheivementType.AllInBlink => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("BLINK_MODE")),
                AcheivementType.AllInRotate => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("ROTATE_MODE")),
                AcheivementType.AllInClassicPlus => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("CLASSIC_MODE_PLUS")),
                AcheivementType.AllInEndlessPlus => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("ENDLESS_MODE_PLUS")),
                AcheivementType.AllInMemoryPlus => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("MEMORY_MODE_PLUS")),
                AcheivementType.AllInTimeTrialPlus => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("TIMETRIAL_MODE_PLUS")),
                AcheivementType.AllInIncrementalPlus => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("INCREMENTAL_MODE_PLUS")),
                AcheivementType.AllInMovePlus => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("MOVE_MODE_PLUS")),
                AcheivementType.AllInBlinkPlus => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("BLINK_MODE_PLUS")),
                AcheivementType.AllInRotatePlus => LanguageManager.Get("ACHEIVEMENT_AllInMode_DES", LanguageManager.Get("ROTATE_MODE_PLUS")),
                AcheivementType.AllInNormal => LanguageManager.Get("ACHEIVEMENT_AllInNormal_DES"),
                AcheivementType.AllInPlus => LanguageManager.Get("ACHEIVEMENT_AllInPlus_DES"),
                AcheivementType.AllInAll => LanguageManager.Get("ACHEIVEMENT_AllInAll_DES"),
                AcheivementType.FirstLevel => LanguageManager.Get("ACHEIVEMENT_FirstLevel_DES"),
                AcheivementType.FirstInClassic => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("CLASSIC_MODE")),
                AcheivementType.FirstInEndless => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("ENDLESS_MODE")),
                AcheivementType.FirstInMemory => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("MEMORY_MODE")),
                AcheivementType.FirstInTimeTrial => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("TIMETRIAL_MODE")),
                AcheivementType.FirstInIncremental => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("INCREMENTAL_MODE")),
                AcheivementType.FirstInMove => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("MOVE_MODE")),
                AcheivementType.FirstInBlink => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("BLINK_MODE")),
                AcheivementType.FirstInRotate => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("ROTATE_MODE")),
                AcheivementType.FirstInClassicPlus => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("CLASSIC_MODE_PLUS")),
                AcheivementType.FirstInEndlessPlus => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("ENDLESS_MODE_PLUS")),
                AcheivementType.FirstInMemoryPlus => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("MEMORY_MODE_PLUS")),
                AcheivementType.FirstInTimeTrialPlus => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("TIMETRIAL_MODE_PLUS")),
                AcheivementType.FirstInIncrementalPlus => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("INCREMENTAL_MODE_PLUS")),
                AcheivementType.FirstInMovePlus => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("MOVE_MODE_PLUS")),
                AcheivementType.FirstInBlinkPlus => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("BLINK_MODE_PLUS")),
                AcheivementType.FirstInRotatePlus => LanguageManager.Get("ACHEIVEMENT_FirstInMode_DES", LanguageManager.Get("ROTATE_MODE_PLUS")),
                AcheivementType.FirstInNormal => LanguageManager.Get("ACHEIVEMENT_FirstInNormal_DES"),
                AcheivementType.FirstInPlus => LanguageManager.Get("ACHEIVEMENT_FirstInPlus_DES"),
                AcheivementType.FirstInAll => LanguageManager.Get("ACHEIVEMENT_FirstInAll_DES"),
                AcheivementType.FirstChallenge => LanguageManager.Get("ACHEIVEMENT_FirstChallenge_DES"),
                AcheivementType.Challenger => LanguageManager.Get("ACHEIVEMENT_Challenger_DES"),
                _ => string.Empty,
            };
        }

        #endregion
    }
}
