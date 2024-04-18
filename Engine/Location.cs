using Engine.Events;
using Engine.Interfaces;

using Serilog;

namespace Engine {
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

        public static Dictionary<string, Location> Instances { get; set; } = [];

        public static Observable<Location> Current => new(
            new Location() {
                Name = "Technical Scene",
                Body = "Nothing to see here",
                Acts = [
                    new Act(new EngineEvent() { Action = () => State.Actions.StopRunning() }) { Text = "Stop" }
                ]
            }
        );

        public static List<Location> AllInstances => [.. Instances.Values];

        public bool Register(string id) {
            if (!Instances.TryAdd(id, this)) {
                Log.Error($"Location with id = {id} is already registered");
                return false;
            }
            return true;
        }

        public static Location GetInstance(string id) {
            if (!Instances.TryGetValue(id, out var instance)) {
                throw new NotRegisteredException($"Location with id = {id} isn't registered");
            }
            return instance;
        }

        public required string Name { get; set; }
        public required string Body { get; set; }
        public List<Act> Acts { get; init; } = [];
        public List<Location> Passages { get; init; } = [];

        public EngineEventHandler OnRegistration { get; } = new();
        public EngineEventHandler OnEnter { get; } = new();
        public EngineEventHandler OnExit { get; } = new();

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
