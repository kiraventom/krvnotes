using System.Windows.Input;

namespace GUI.ViewModels
{
    internal class ContextAction
    {
        public string Text { get; }
        public ICommand Command { get; }
    }
}
