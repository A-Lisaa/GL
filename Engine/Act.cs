using Engine.Events;
using Engine.Exceptions;

namespace Engine {
    public partial record Act {
        public Act(params EngineEvent<EventArgs>[] onUseEvents) {
            OnUse = new(onUseEvents);
        }

        public string Text { get; set; } = "";
        public int UsesLeft { get; set; } = int.MaxValue;
        public bool IsActive { get; set; } = true;
        public bool IsForDestruction { get; set; }

        public EngineEventHandler OnUse { get; } = new();
        public EngineEventHandler OnBeforeShown { get; } = new();
        public EngineEventHandler OnUsesSpent { get; } = new();

        public virtual void Use() {
            OnUse.Invoke(this, new());
            if (--UsesLeft <= 0)
                OnUsesSpent.Invoke(this, new());
        }
    }

    public record SceneStarter : Act {
        public SceneStarter(Scene scene, params EngineEvent<EventArgs>[] onUseEvents) : base(onUseEvents) {
            Scene = scene;
        }

        // Next is set on Next registration so that Scene can be created after SceneStarter
#pragma warning disable CS8618
        public SceneStarter(string next, params EngineEvent<EventArgs>[] onUseEvents) : base(onUseEvents) {
#pragma warning restore CS8618
            Scene.Registration.OnRegistration.AddEvent(new() {
                FireCondition = (object? _, Registration<Scene>.OnRegistrationEventArgs args) => args.Id == next,
                Action = (object? _, Registration<Scene>.OnRegistrationEventArgs args) => Scene = args.Instance
            });
        }

        public Scene Scene { get; set; }
    }

    public record Passage : Act {
        public Passage(Location next, params EngineEvent<EventArgs>[] onUseEvents) : base(onUseEvents) {
            Next = next;
            if (Text.Length == 0)
                Text = $"To {Next.Name}";
        }

        //Next is set on Next registration so that Location can be created after Passage
#pragma warning disable CS8618
        public Passage(string next, params EngineEvent<EventArgs>[] onUseEvents) : base(onUseEvents) {
#pragma warning restore CS8618
            if (Location.Registration.IsRegistered(next)) {
                Next = Location.Registration[next];
                if (Text.Length == 0)
                    Text = $"To {Next.Name}";
            }
            else {
                Location.Registration.OnRegistration.AddEvent(new() {
                    FireCondition = (object? _, Registration<Location>.OnRegistrationEventArgs args) => args.Id == next,
                    Action = (object? _, Registration<Location>.OnRegistrationEventArgs args) => {
                        Next = args.Instance;
                        if (Text.Length == 0)
                            Text = $"To {Next.Name}";
                    }
                });
            }

            // if Next hasn't been registered
            OnBeforeShown.AddEvent(new() {
                FireCondition = (object? _, EventArgs _) => Next == null,
                Action = (object? _, EventArgs _) => throw new ValueNullException(nameof(Next))
            });
        }

        public Location Next { get; set; }

        public override void Use() {
            base.Use();
            Location.Move(Next);
        }
    }
}
