using Engine.Events;

namespace Engine {
    public record Act {
        public Act(params EngineEvent[] onUseEvents) {
            OnUse = new() { Events = [..onUseEvents] };
        }

        public required string Text { get; set; }

        public EngineEventHandler OnUse { get; }

        public virtual void Use() {
            OnUse.Invoke();
        }
    }

    public record Act<NextScene> : Act where NextScene : Scene, new() {
        public Act(params EngineEvent[] events) : base(events) {
        }

        public override void Use() {
            base.Use();
            Game.State.CurrentScene = new NextScene();
        }
    }
 }
