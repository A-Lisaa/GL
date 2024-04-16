using Engine.Events;

using Serilog;

namespace Engine {
    public partial record Scene {
        public static Scene Chain(params Scene[] scenes) {
            for (int i = 0; i < scenes.Length - 1; i++) {
                scenes[i].Acts.Insert(0, new Act(scenes[i + 1]) { Text = "Continue" });
            }
            return scenes[0];
        }

        private static Scene current = new() {
            Name = "Techical Scene",
            Body = "Nothing to see here",
            Acts = [
                new Act(
                    new EngineEvent() {
                        FireCondition = EngineEvent.FireConditions.Always(),
                        Action = State.Actions.StopRunning()
                    }
                ) {
                    Text = "Exit"
                }
            ]
        };

        public static EngineEventHandler SceneChange { get; } = new();

        public static Scene Current {
            get => current;
            set {
                current = value;
                SceneChange.Invoke();
            }
        }

        private readonly static Dictionary<string, Scene> scenes = [];

        public bool Register(string id) {
            if (!scenes.TryAdd(id, this)) {
                Log.Error($"Scene with id {id} is already registered");
                return false;
            }
            return true;
        }

        public static Scene GetScene(string id) {
            return scenes[id];
        }

        public static List<Scene> AllScenes => [.. scenes.Values];

        public required string Body { get; set; }
        public string Name { get; set; } = "";
        public List<Act> Acts { get; init; } = [];

        public EngineEventHandler OnStart { get; } = new();

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
        }
    }
}
