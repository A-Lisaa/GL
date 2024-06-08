global using DefaultConditionReturn = System.Func<object?, System.EventArgs, bool>;
global using DefaultActionReturn = System.Action<object?, System.EventArgs>;

namespace Engine.Events {
    public record EngineEvent<TEventArgs> : IComparable<EngineEvent<TEventArgs>> where TEventArgs : EventArgs  {
        public required Action<object?, TEventArgs> Action { private get; init; }

        /// <summary>
        /// returns <c>true</c> when the event is to be invoked
        /// </summary>
        public Func<object?, TEventArgs, bool> FireCondition { private get; init; } = EngineEvent.FireConditions.Always();

        // default set to OneTime seems like a good idea to avoid stupids setting it to Never
        // maybe should discourage and warn about using Never and similar conditions
        /// <summary>
        /// returns <c>true</c> when the event is to be destructed
        /// </summary>
        public Func<object?, TEventArgs, bool> DestructionCondition { private get; init; } = EngineEvent.DestructionConditions.OneTime();

        // do we need this or should we use DestructionCondition directly?
        // probably not as IsForDestruction could be set by smth else
        public bool IsForDestruction { get; private set; }

        public static class Priorities {
            public static int Minimum => int.MaxValue;
            public static int Maximum => int.MinValue;
        }

        /// <summary>
        /// priority in which the event will be invoked by event handler, the lower the earlier
        /// </summary>
        public int Priority { private get; init; }

        /// <summary>
        /// Action will be invoked this amount of times on each event invocation,
        /// it's this way so that we don't need to create multiple identical events
        /// </summary>
        public int TimesToInvokeAction { private get; init; } = 1;

        public int CompareTo(EngineEvent<TEventArgs>? other) {
            if (other == null)
                return 1;
            return Priority.CompareTo(other.Priority);
        }

        public virtual void Invoke(object? sender, TEventArgs eventArgs) {
            if (FireCondition.Invoke(sender, eventArgs)) {
                for (int _ = 0; _ < TimesToInvokeAction; _++) {
                    Action.Invoke(sender, eventArgs);
                }
                IsForDestruction = DestructionCondition.Invoke(sender, eventArgs);
            }
        }

        public static EngineEvent<TEventArgs> FromAction(Action<object?, TEventArgs> action) {
            return new() { Action = action };
        }

        public static IEnumerable<EngineEvent<TEventArgs>> FromActions(IEnumerable<Action<object?, TEventArgs>> actions) {
            return
                from action in actions
                select FromAction(action);
        }
    }

    public partial record EngineEvent : EngineEvent<EventArgs>;
}