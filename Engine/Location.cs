using Engine.Events;
using Engine.Interfaces;

using Serilog;

namespace Engine {
    // merge Location and Scene
    public record Location : IRegistrable<Location> {
        private void AddPassagesToActs() {
            Acts.InsertRange(0,
                from passage in Passages
                select new Act<Location>() {
                    Text = $"To {passage.Name}",
                    Next = passage
                }
            );
        }

        public Location() {
            OnRegistration.Add(AddPassagesToActs);
        }

        public static Observable<Location> Current => new(
            new Location() {
                Name = "Technical Scene",
                Body = "Nothing to see here",
                Acts = [
                    new Act(new EngineEvent() { Action = () => Game.Actions.StopRunning() }) { Text = "Stop" }
                ]
            }
        );

        public static List<Location> AllInstances => IRegistrable<Location>.allInstances;

        public bool Register(string id) {
            return ((IRegistrable<Location>)this).register(id);
        }

        public static Location GetInstance(string id) {
            return IRegistrable<Location>.getInstance(id);
        }

        public required string Name { get; set; }
        public required string Body { get; set; }
        public List<Act> Acts { get; init; } = [];
        public List<Location> Passages { get; init; } = [];

        public EngineEventHandler OnEnter { get; } = new();
        public EngineEventHandler OnExit { get; } = new();
        public EngineEventHandler OnRegistration { get; } = new();

        public static void Move(Location destination) {
            Current.Value = destination;
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
        }
    }
}
