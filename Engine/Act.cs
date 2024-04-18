using Engine.Events;
using Engine.Interfaces;

namespace Engine {
    public record Act {
        public Act(params EngineEvent[] onUseEvents) {
            OnUse = new(onUseEvents);
        }

        public required string Text { get; set; }
        public bool IsActive { get; set; } = true;

        public EngineEventHandler OnUse { get; } = new();

        public virtual void Use() {
            OnUse.Invoke();
        }
    }

    public record Act<T> : Act where T : IRegistrable<T> {
        public Act(params EngineEvent[] onUseEvents) : base(onUseEvents) { }

        public Act(T next, params EngineEvent[] onUseEvents) : base(onUseEvents) {
            Next = next;
        }

        public Act(string next, params EngineEvent[] onUseEvents) : base(onUseEvents) {
            // Next is set on first use so that T object can be created before
            OnUse.Add(() => Next = T.GetInstance(next));
        }

        public required T Next { get; set; }

        public override void Use() {
            base.Use();
            T.Current.Value = Next;
        }
    }
}
