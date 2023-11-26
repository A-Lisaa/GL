namespace Engine {
    public record Act {
        public Act(params EngineEvent[] events) {
            Events = new() { Events = [..events] };
        }

        public required string Text { get; set; }

        public EngineEventHandler Events { get; }
        protected virtual void OnUsed() {
            Events.Invoke();
        }

        public virtual void Use() {
            OnUsed();
        }
    }

    public record Act<NextScene> : Act where NextScene : Scene, new() {
        public Act(params EngineEvent[] events) : base(events) {
        }

        public override void Use() {
            base.Use();
            Engine.CurrentScene = new NextScene();
        }
    }
 }
