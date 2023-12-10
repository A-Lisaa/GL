namespace Engine.Events {
    public record EngineEvent {
        // return true, when the event is to be invoked
        public required Func<bool> FireCondition { private get; init; }

        public required Action Action { private get; init; }

        // returns true, when the event is to be destructed
        public virtual Func<bool> DestructionCondition { protected get; init; } = DestructionConditions.OneTime();

        public bool IsForDestruction { get; protected set; }

        public virtual void Invoke() {
            if (FireCondition.Invoke()) {
                Action.Invoke();
            }
            IsForDestruction = DestructionCondition.Invoke();
        }
    }

    public static class FireConditions {
        public static Func<bool> All(params Func<bool>[] conditions) {
            return () => Array.TrueForAll(conditions, (condition) => condition.Invoke());
        }

        public static Func<bool> Always() {
            return () => true;
        }

        public static Func<bool> HasFlag(string flag) {
            return () => Game.Flags.Contains(flag);
        }

        public static Func<bool> HasNoFlag(string flag) {
            return () => !Game.Flags.Contains(flag);
        }
    }

    public static class Actions {
        public static Action Chain(params Action[] actions) {
            return () => {
                foreach (var action in actions) {
                    action.Invoke();
                }
            };
        }

        public static Action SetFlag(string flag) {
            return () => Game.Flags.Add(flag);
        }
    }

    public static class DestructionConditions {
        public static Func<bool> All(params Func<bool>[] conditions) {
            return () => Array.TrueForAll(conditions, (condition) => condition.Invoke());
        }

        public static Func<bool> OneTime() {
            return () => true;
        }

        public static Func<bool> Never() {
            return () => false;
        }

        public static Func<bool> HasFlag(string flag) {
            return () => Game.Flags.Contains(flag);
        }

        public static Func<bool> HasNoFlag(string flag) {
            return () => !Game.Flags.Contains(flag);
        }
    }
}