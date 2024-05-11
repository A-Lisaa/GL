using Engine.Events;

using Serilog;

namespace Engine {
    public record Location {
        protected internal static Registration<Location> Registration { get; } = new();

        protected static object CurrentInner { get; set; } = new Location() {
            Name = "Technical Location",
            Body = "Nothing to see here",
            Acts = [
                new Act(new EngineEvent() { Action = Game.Actions.StopRunning() }) { Text = "Stop" }
            ]
        };

        protected internal static Location Current {
            get {
                return (Location)CurrentInner;
            }
            set {
                CurrentInner = value;
                OnChange.Invoke();
            }
        }

        public static EngineEventHandler OnChange { get; } = new();

        public string Name { get; set; } = "";
        public string Body { get; set; } = "";
        public List<Act> Acts { get; init; } = [];

        public EngineEventHandler OnEnter { get; } = new();
        public EngineEventHandler OnExit { get; } = new();

        public void UseAct(int actNumber) {
            if (actNumber < 0 || actNumber >= Acts.Count) {
                Log.Error($"actNumber is out of bounds (< 0 or >= Acts.Count ({Acts.Count}))");
                return;
            }
            var act = Acts[actNumber];
            if (!act.IsActive) {
                Log.Debug($"act {actNumber} is not active");
                return;
            }
            Log.Debug($"Using act {actNumber}");
            act.Use();
            Acts.RemoveAll(act => act.IsForDestruction);
        }
    }
}
