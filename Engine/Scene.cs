using Engine.Events;
using Engine.Interfaces;

using Serilog;

namespace Engine {
    public partial record Scene : IRegistrable<Scene> {
        public static Scene Chain(params Scene[] scenes) {
            for (int i = 0; i < scenes.Length - 1; i++) {
                scenes[i].Acts.Insert(0, new SceneStarter(scenes[i + 1]) { Text = "Continue" });
            }
            return scenes[0];
        }

        public static Dictionary<string, Scene> Instances { get; set; } = [];

        public static Observable<Scene> Current => new(
            new Scene() {
                Name = "Technical Scene",
                Body = "Nothing to see here",
                Acts = [
                    new Act(new EngineEvent() { Action = Game.Actions.StopRunning() }) { Text = "Stop" }
                ]
            }
        );

        public static List<Scene> AllInstances => IRegistrable<Scene>.allInstances;

        public bool Register(string id) {
            return ((IRegistrable<Scene>)this).register(id);
        }

        public static Scene GetInstance(string id) {
            return IRegistrable<Scene>.getInstance(id);
        }

        public required string Body { get; set; }
        public string Name { get; set; } = "";
        public List<Act> Acts { get; init; } = [];

        public EngineEventHandler OnStart { get; } = new();
        public EngineEventHandler OnRegistration { get; } = new();

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
