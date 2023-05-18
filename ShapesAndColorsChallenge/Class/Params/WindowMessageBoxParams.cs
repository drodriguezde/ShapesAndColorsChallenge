using ShapesAndColorsChallenge.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesAndColorsChallenge.Class.Params
{
    internal class WindowMessageBoxParams
    {
        #region PROPERTIES

        internal string Message { get; private set; } = string.Empty;
        internal MessageBoxButton MessageBoxButton { get; private set; } = MessageBoxButton.AcceptCancel;
        internal int LinesNumber { get; private set; } = 1;

        #endregion

        #region CONSTRUCTOR

        internal WindowMessageBoxParams(string message, MessageBoxButton messageBoxButton, int linesNumber = 1)
        {
            Message = message;
            MessageBoxButton = messageBoxButton;
            LinesNumber = linesNumber;
        }

        #endregion
    }
}
