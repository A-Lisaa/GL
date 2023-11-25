namespace Engine {
    public class EngineEventHandler {
        public List<EngineEvent> EngineEvents { private get; init; } = [];

        public void Add(EngineEvent engineEvent) {
            EngineEvents.Add(engineEvent);
        }

        public void Remove(EngineEvent engineEvent) {
            EngineEvents.Remove(engineEvent);
        }

        public void Invoke() {
            foreach (var engineEvent in EngineEvents) {
                engineEvent.Invoke();
                if (engineEvent.IsForDestruction) {
                    EngineEvents.Remove(engineEvent);
                }
            }
        }
    }
}
