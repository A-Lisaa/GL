namespace Engine {
    public class EngineEventHandler {
        public List<EngineEvent> Events { private get; init; } = [];

        public void Add(EngineEvent engineEvent) {
            Events.Add(engineEvent);
        }

        public void Remove(EngineEvent engineEvent) {
            Events.Remove(engineEvent);
        }

        public void Invoke() {
            foreach (var engineEvent in Events) {
                engineEvent.Invoke();
                if (engineEvent.IsForDestruction) {
                    Events.Remove(engineEvent);
                }
            }
        }
    }
}
