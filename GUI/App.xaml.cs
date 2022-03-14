using System.Windows;
using BL;

namespace GUI
{
    public partial class App : Application
    {
        internal App()
        {

        }

        public static App Create(IController controller)
        {
            return new App {Controller = controller};
        }

        internal IController Controller { get; private init; }
    }
}