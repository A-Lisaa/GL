namespace Engine {
    public record Act {
        public Act(params Action[] actions) {
            foreach (Action action in actions) {
                Used += action;
            }
        }

        public required string Text { get; set; }

        // event actions should be automatically removed after condition (one use, time, if), make like a class hierarchy or smth
        public event Action? Used;
        protected virtual void OnUsed() {
            Used?.Invoke();
        }

        public virtual void Use() {
            OnUsed();
        }
    }

    public record Act<NextScene> : Act where NextScene : Scene, new() {
        public Act(params Action[] actions) : base(actions) {
        }

        public override void Use() {
            base.Use();
            Engine.CurrentScene = new NextScene();
        }
    }
 }
