namespace Engine.Events {
    public partial class EngineEventHandler {
        public static class Actions {
            public static Action AddEvent(EngineEventHandler handler, EngineEvent engineEvent) {
                return () => handler.Add(engineEvent);
            }
        }
    }
}
