using Engine.Events;

using Serilog;

namespace Engine {
    public record Act {
        private Scene? nextScene;
        private Location? nextLocation;

        public Act(params EngineEvent[] onUseEvents) {
            OnUse = new(onUseEvents);
        }

        public Act(Scene nextScene, params EngineEvent[] onUseEvents) : this(onUseEvents) {
            NextScene = nextScene;
        }

        public Act(string nextScene, params EngineEvent[] onUseEvents) : this(onUseEvents) {
            OnUse.Add(new EngineEvent() {
                Action = () => NextScene = Scene.GetScene(nextScene)
            });
        }

        public required string Text { get; set; }
        public Scene? NextScene {
            get => nextScene;
            set {
                if (nextLocation != null) {
                    Log.Error("Can't set NextScene and NextLocation");
                    return;
                }
                nextScene = value;
            }
        }
        public Location? NextLocation {
            get => nextLocation;
            set {
                if (nextScene != null) {
                    Log.Error("Can't set NextScene and NextLocation");
                    return;
                }
                nextLocation = value;
            }
        }
        public bool IsActive { get; set; } = true;

        public bool HasNextScene() {
            return NextScene != null;
        }

        public EngineEventHandler OnUse { get; }

        public virtual void Use() {
            OnUse.Invoke();
            if (NextScene != null) {
                Scene.Current = NextScene;
            }
        }
    }
}
