namespace Engine {
    public abstract record EngineEvent {
        public required Action Action { private get; init; }

        public bool IsForDestruction { get; protected set; }

        public virtual void Invoke() {
            Action.Invoke();
        }
    }

    public record OneTimeEngineEvent : EngineEvent {
        public override void Invoke() {
            base.Invoke();
            IsForDestruction = true;
        }
    }

    // TODO: make condition classes for different built-in conditions and corresponding event handlers in Engine
    public record ConditionalEngineEvent : EngineEvent {
        public required Func<bool> Condition { private get; init; }

        public override void Invoke() {
            base.Invoke();
            IsForDestruction = Condition();
        }
    }
}
