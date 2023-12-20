using Engine.Events;

namespace Engine {
    public record Act {
        public Act(params EngineEvent[] onUseEvents) {
            OnUse = new() { Events = [..onUseEvents] };
        }

        public required string Text { get; set; }

        // make some way for scenes to set events with life longer the scene itself, although that brings problems:
        // 1) if we just add all the events to global events HashSet, events with same callback will PROBABLY (investigate)
        // conflict with each other, on the other hand if we add events to List, a bunch of events might unwantedly outlive the scene
        // (though if we set DestructionCondition to something sensible, they probably shouldn't, need more thought on that)
        // also if we set DestructionCondition to not OneTime, they'll remain in the List and on each Scene creation there'll be more
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
