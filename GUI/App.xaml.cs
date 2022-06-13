using BL;

namespace GUI
{
    public partial class App
    {
        internal App()
        {

        }

        public static App Create(IEventManager eventManager)
        {
            return new App {EventManager = eventManager};
        }

        internal IEventManager EventManager { get; private init; }
    }
}