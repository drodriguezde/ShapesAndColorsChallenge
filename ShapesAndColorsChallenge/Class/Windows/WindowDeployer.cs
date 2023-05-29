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
* DISPOSE CONTROL : STATIC
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
using ShapesAndColorsChallenge.Enum;

namespace ShapesAndColorsChallenge.Class.Windows
{
    internal static class WindowDeployer
    {
        #region METHODS

        internal static Window Deploy(WindowType windowType, object parameters = null)
        {
            return windowType switch
            {
                WindowType.DanStudios => DeployDanStudios(),
                WindowType.Title => DeployTitle(),
                WindowType.GameMode => DeployGameMode(),
                WindowType.Stage => DeployStage(),
                WindowType.Level => DeployLevel(),
                WindowType.Settings => DeploySettings(),
                WindowType.Language => DeployLanguage(),
                WindowType.Acheivements => DeployAcheivements(),
                WindowType.HowToPlay => DeployHowToPlay(parameters),
                WindowType.Rankings => DeployRankings(parameters),
                WindowType.Challenges => DeployChallenges(),
                WindowType.Nationality => DeployNationality(),
                WindowType.Game => DeployGame(),
                WindowType.MessageBox => DeployMessageBox(parameters),
                WindowType.Reward => DeployMessageReward(parameters),
                WindowType.Result => DeployMessageResult(parameters),
                WindowType.Pause => DeployPause(),
                WindowType.SelectRanking => DeploySelectRanking(),
                _ => DeployDanStudios(),
            };
        }

        static WindowResult DeployMessageResult(object parameters)
        {
            WindowResult window = new((WindowResultParams)parameters) { AddBackButton = false };
            return window;
        }

        static WindowReward DeployMessageReward(object parameters)
        {
            WindowReward window = new((WindowRewardParams)parameters) { AddBackButton = false };
            return window;
        }

        static WindowPause DeployPause()
        {
            WindowPause window = new() { AddBackButton = false, BacklayerTransparency = 0.9f/*Se oscurece un poco más para que no se vean las fichas*/ };
            return window;
        }

        static WindowSelectRanking DeploySelectRanking()
        {
            WindowSelectRanking window = new() { AddBackButton = false };
            return window;
        }

        static WindowMessageBox DeployMessageBox(object parameters)
        {
            WindowMessageBox window = new(
                ((WindowMessageBoxParams)parameters).MessageBoxButton,
                ((WindowMessageBoxParams)parameters).Message,
                ((WindowMessageBoxParams)parameters).LinesNumber)
            { AddBackButton = false };
            return window;
        }

        static Window DeployDanStudios()
        {
            WindowDanStudios window = new() { AddBackButton = false };
            return window;
        }

        static Window DeployTitle()
        {
            WindowTitle window = new();
            return window;
        }

        static Window DeployGameMode()
        {
            WindowGameMode window = new();
            return window;
        }

        static Window DeployStage()
        {
            WindowStage window = new();
            return window;
        }

        static Window DeployLevel()
        {
            WindowLevel window = new();
            return window;
        }

        static Window DeploySettings()
        {
            Window window = new WindowSettings();
            return window;
        }

        static Window DeployLanguage()
        {
            Window window = new WindowLanguage();
            return window;
        }

        static Window DeployAcheivements()
        {
            Window window = new WindowAcheivements() { AddBottomBackGround = true };
            return window;
        }

        static Window DeployHowToPlay(object parameters)
        {
            Window window = new WindowHowToPlay((WindowHowToPlayParams)parameters);
            return window;
        }

        static Window DeployRankings(object parameters)
        {
            Window window = new WindowRankings((bool)parameters) { AddBottomBackGround = true };
            return window;
        }

        static Window DeployChallenges()
        {
            Window window = new WindowChallenges();
            return window;
        }

        static Window DeployNationality()
        {
            Window window = new WindowNationality();
            return window;
        }

        static Window DeployGame()
        {
            Window window = new WindowGame();
            return window;
        }

        #endregion
    }
}