using Engine.Events;
using Engine.Exceptions;

namespace Engine {
    public partial record Act {
        public Act(params EngineEvent[] onUseEvents) {
            OnUse = new(onUseEvents);
        }

        public string Text { get; set; } = "";
        public int UsesLeft { get; set; } = int.MaxValue;
        public bool IsActive { get; set; } = true;
        public bool IsForDestruction { get; set; }

        public EngineEventHandler OnUse { get; } = new();
        public EngineEventHandler OnUsesSpent { get; } = new();

        public virtual void Use() {
            OnUse.Invoke();
            if (--UsesLeft <= 0)
                OnUsesSpent.Invoke();
        }
    }

    public record Passage : Act {
        public Passage(Location next, params EngineEvent[] onUseEvents) : base(onUseEvents) {
            Next = next;
            if (Text.Length == 0)
                Text = $"To {Next.Name}";
        }

        // Next is set on Next registration so that Location can be created after Passage
#pragma warning disable CS8618
        public Passage(string next, params EngineEvent[] onUseEvents) : base(onUseEvents) {
#pragma warning restore CS8618
            Location.Registration.OnRegistration.Add(new EngineEvent() {
                FireCondition = () => Location.Registration.IsRegistered(next),
                Action = EngineEvent.Actions.Chain(
                    () => Next = Location.Registration.GetInstance(next),
                    () => {
                        if (Text.Length == 0)
                            Text = $"To {Next!.Name}";
                    }
                )
            });
            // if Next hasn't been registered
            OnUse.Add(new EngineEvent() {
                FireCondition = () => Next == null,
                Action = () => throw new ValueIsNullException(nameof(Next))
            });
        }

        public Location Next { get; set; }

        public override void Use() {
            base.Use();
            Location.Current = Next;
        }
    }

    public record SceneStarter : Act {
        public SceneStarter(Scene scene, params EngineEvent[] onUseEvents) : base(onUseEvents) {
            Scene = scene;
        }

        // Next is set on first use so that Location can be created after Act
#pragma warning disable CS8618
        public SceneStarter(string next, params EngineEvent[] onUseEvents) : base(onUseEvents) {
#pragma warning restore CS8618
            // will Location.GetInstance work or should it be Derived.GetInstance
            OnUse.Add(() => Scene = Scene.Registration.GetInstance(next));
        }

        public Scene Scene { get; set; }
    }
}
