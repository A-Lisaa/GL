namespace Engine.Events {
    // make an excel table of different events with descriptions and tags for finding appropriate one
    public record EngineEventHandler {
        // should it be HashSet or List?
        // will HashSet even correctly handle EngineEvent? should investigate
        public HashSet<EngineEvent> Events { private get; init; } = [];

        public event Action? Invoked;

        // make a method for constructing and adding EngineEvent? a factory?
        public void Add(EngineEvent engineEvent) {
            Events.Add(engineEvent);
        }

        public void Invoke() {
            foreach (var engineEvent in Events) {
                engineEvent.Invoke();
            }
            Events.RemoveWhere((engineEvent) => engineEvent.IsForDestruction);
            Invoked?.Invoke();
        }
    }
}
