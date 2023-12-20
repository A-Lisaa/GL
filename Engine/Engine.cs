﻿using Engine.Events;
using Engine.UI;

namespace Engine {
    public abstract class Game {
        public abstract BaseUI UI { get; }
        public static State State { get; } = new State();
        public static HashSet<string> Flags { get; } = [];
        public static Dictionary<string, int> Counters { get; } = [];
        public static EngineEventHandler Events { get; } = new();

        public virtual void Run() {
            UI.Run();
        }
    }
}
