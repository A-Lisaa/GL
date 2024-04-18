using Engine.Events;

namespace Engine {
    public class Observable<T>(T value) {
        public T Value { get; set; } = value;
        public EngineEventHandler OnChange { get; } = new();
    }
}
