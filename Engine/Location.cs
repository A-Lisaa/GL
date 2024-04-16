using Engine.Events;

using Serilog;

namespace Engine {
    public record Location {
        private void AddPassagesToActs() {
            Acts.InsertRange(0,
                from passage in Passages
                select new Act() {
                    Text = $"To {passage.Name}",
                    NextLocation = passage
                }
            );
        }

        public Location() {
            OnRegistration.Add(AddPassagesToActs);
        }

        // this is similar to Scene

        private static Location current = new() {
            Name = "Technical Location",
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

        public static EngineEventHandler LocationChange { get; } = new();

        public static Location Current {
            get => current;
            set {
                current = value;
                LocationChange.Invoke();
            }
        }

        private readonly static Dictionary<string, Location> locations = [];

        public EngineEventHandler OnRegistration { get; } = new();

        public bool Register(string id) {
            if (!locations.TryAdd(id, this)) {
                Log.Error($"Location with id {id} is already registered");
                return false;
            }
            OnRegistration.Invoke();
            return true;
        }

        public static Location GetLocation(string id) {
            return locations[id];
        }

        public static List<Location> AllLocations => [.. locations.Values];

        public required string Name { get; set; }
        public required string Body { get; set; }
        public List<Act> Acts { get; init; } = [];
        public List<Location> Passages { get; init; } = [];

        public EngineEventHandler OnEnter { get; } = new();
        public EngineEventHandler OnExit { get; } = new();

        public static void Move(Location destination) {
            Current.OnExit.Invoke();
            Current = destination;
            Current.OnEnter.Invoke();
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
