using Engine.Events;
using Engine.UI;

namespace Engine {
    public abstract class Game {
        public abstract BaseUI UI { get; }
        public static State State { get; } = new State();
        // make a separate record with only like Add and Remove methods exposed?
        // delete from here but make a namespace(?) with it, so that it can be used in derived Game, just like with Time and Counter
        public static HashSet<string> Flags { get; } = [];
        public static EngineEventHandler Events { get; } = new();

        public void Run() {
            UI.Run();
        }
    }
}
