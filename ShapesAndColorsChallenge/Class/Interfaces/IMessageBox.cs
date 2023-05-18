using ShapesAndColorsChallenge.Class.Windows;

namespace ShapesAndColorsChallenge.Class.Interfaces
{
    public interface IMessage
    {
        public Window WindowMessage { get; set; }
        public void CloseMessageBox();
    }
}
