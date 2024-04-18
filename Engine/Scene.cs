using Engine.Events;
using Engine.Interfaces;

using Serilog;

using System.Collections.Generic;

namespace Engine {
    public partial record Scene : IRegistrable<Scene> {
        public static Scene Chain(params Scene[] scenes) {
            for (int i = 0; i < scenes.Length - 1; i++) {
                scenes[i].Acts.Insert(0, new Act<Scene>() { Text = "Continue", Next = scenes[i+1] });
            }
            return scenes[0];
        }

        public static Dictionary<string, Scene> Instances { get; set; } = [];

        public static Observable<Scene> Current => new(
            new Scene() {
                Name = "Technical Scene",
                Body = "Nothing to see here",
                Acts = [
                    new Act(new EngineEvent() { Action = () => State.Actions.StopRunning() }) { Text = "Stop" }
                ]
            }
        );

        public static List<Scene> AllInstances => [.. Instances.Values];

        public bool Register(string id) {
            if (!Instances.TryAdd(id, this)) {
                Log.Error($"Scene with id = {id} is already registered");
                return false;
            }
            return true;
        }

        public static Scene GetInstance(string id) {
            if (!Instances.TryGetValue(id, out var instance)) {
                throw new NotRegisteredException($"Scene with id = {id} isn't registered");
            }
            return instance;
        }

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
