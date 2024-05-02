using Engine.Events;
using Engine.Interfaces;

using Serilog;

namespace Engine {
    // merge Location and Scene
    // Location is the boss of this gym and Scene is a fucking slave
    public record Location : IRegistrable<Location> {
        public static Observable<Location> Current { get; set; } = new(
            new Location() {
                Name = "Technical Location",
                Body = "Nothing to see here",
                Acts = [
                    new Act(new EngineEvent() { Action = Game.Actions.StopRunning() }) { Text = "Stop" }
                ]
            }
        );

        public static List<Location> AllInstances => IRegistrable<Location>.allInstances;

        public virtual bool Register(string id) {
            return ((IRegistrable<Location>)this).register(id);
        }

        public static Location GetInstance(string id) {
            return IRegistrable<Location>.getInstance(id);
        }

        public string Name { get; set; } = "";
        public string Body { get; set; } = "";
        public List<Act> Acts { get; init; } = [];

        public EngineEventHandler OnEnter { get; } = new();
        public EngineEventHandler OnExit { get; } = new();

        public static void Move(Location destination) {
            Location.Current.Value = destination;
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
