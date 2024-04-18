namespace Engine.Events {
    // make an excel table of different events with descriptions and tags for finding appropriate one
    public partial class EngineEventHandler {
        public EngineEventHandler(params EngineEvent[] engineEvents) {
            events = [.. engineEvents];
            events.Sort();
        }

        private readonly List<EngineEvent> events;

        public event Action? Invoked;

        public void Add(EngineEvent engineEvent) {
            events.Add(engineEvent);
            events.Sort();
        }

        public void Add(Action action) {
            Add(new EngineEvent() { Action = action });
        }

        public void AddRange(IEnumerable<EngineEvent> engineEvents) {
            events.AddRange(engineEvents);
            events.Sort();
        }

        public void Invoke() {
            foreach (var engineEvent in events) {
                engineEvent.Invoke();
            }
            events.RemoveAll((engineEvent) => engineEvent.IsForDestruction);
            Invoked?.Invoke();
        }
    }
}
