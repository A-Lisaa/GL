using Engine.Events;

using Serilog;

namespace Engine {
    public abstract record Location {
        protected Location() {
            OnBeforeShown.AddEvent(new EngineEvent() {
                Action = (object? _, EventArgs _) => Acts.ForEach((Act act) => act.OnBeforeShown.Invoke(this, new())),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });
        }

        public static Registration<Location> Registration { get; } = new();

        protected record TechnicalLocation : Location {
            public override string Name { get; set; } = "Technical Location";
            public override string Body { get; set; } = "Nothing to see here";

            public override List<Act> Acts => [
                new Act(new EngineEvent<EventArgs>() { Action = Game.Actions.StopRunning() }) { Text = "Stop" }
            ];
        }

        protected static Location CurrentInner { get; set; } = new TechnicalLocation();

        public static Location Current {
            get {
                return CurrentInner;
            }
            set {
                OnChangeEventArgs args = new() { OldLocation = Current, NewLocation = value };
                CurrentInner = value;
                OnChange.Invoke(null, args);
            }
        }

        public class OnChangeEventArgs : EventArgs {
            public required Location OldLocation { get; set; }
            public required Location NewLocation { get; set; }
        }

        public static EngineEventHandler<OnChangeEventArgs> OnChange { get; } = new();

        public abstract string Name { get; set; }
        public abstract string Body { get; set; }
        public abstract List<Act> Acts { get; }

        public class OnEnterEventArgs : EventArgs {
            public required Location EnteredLocation { get; set; }
        }

        public class OnExitEventArgs : EventArgs {
            public required Location ExitedLocation { get; set; }
        }

        public EngineEventHandler OnBeforeShown { get; } = new();
        public EngineEventHandler<OnEnterEventArgs> OnEnter { get; } = new();
        public EngineEventHandler<OnExitEventArgs> OnExit { get; } = new();

        public static void Move(Location next) {
            Current.OnExit.Invoke(null, new() { ExitedLocation = Current });
            Current = next;
            Current.OnEnter.Invoke(null, new() { EnteredLocation = Current });
        }

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
