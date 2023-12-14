namespace Engine.Events {
    public record EngineEventHandler {
        // should it be HashSet or List?
        // will HashSet even correctly handle EngineEvent? should investigate
        public HashSet<EngineEvent> Events { private get; init; } = [];

        public event Action? Invoked;

        public void Add(EngineEvent engineEvent) {
            Events.Add(engineEvent);
        }

        public void Invoke() {
            foreach (var engineEvent in Events) {
                engineEvent.Invoke();
            }
            Events.RemoveWhere(engineEvent => engineEvent.IsForDestruction);
            Invoked?.Invoke();
        }
    }
}
