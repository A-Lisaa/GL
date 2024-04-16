using Engine.Events;

using Serilog;

namespace Engine {
    public class State {
        public static class Actions {
            public static Action StopRunning() {
                return () => Game.State.IsRunning = false;
            }
        }

        public bool IsRunning { get; set; } = true;
    }
}
