using Engine.Events;
using Engine.UI;

namespace Engine {
    public partial class Game {
        public static bool IsRunning { get; set; } = true;
        public static UI.UI UI { get; set; } = new CLI();
        public static HashSet<string> Flags { get; } = [];
        public static Dictionary<string, int> Counters { get; } = [];
        public static EngineEventHandler Events { get; } = new();
        public static EngineDateTime DateTime { get; } = new(System.DateTime.Now);

        public virtual void Update() {

        }

        public virtual void Run() {
            UI.Run();
        }
    }
}
