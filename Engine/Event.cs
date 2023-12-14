namespace Engine.Events {
    public record EngineEvent {
        // returns true when the event is to be invoked
        public required Func<bool> FireCondition { private get; init; }

        public required Action Action { private get; init; }

        // 1) returns true when the event is to be destructed
        // 2) default set to OneTime seems like a good idea to avoid stupids setting it to Never
        // maybe should discourage and warn about using Never and similar conditions
        public virtual Func<bool> DestructionCondition { protected get; init; } = DestructionConditions.OneTime();

        // do we need this or should we use DestructionCondition directly?
        // probably not as IsForDestruction could be set by smth else
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

        public static Func<bool> Any(params Func<bool>[] conditions) {
            return () => {
                return (
                    from condition in conditions
                    where condition.Invoke()
                    select condition
                ).Any();
            };
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

        public static Action DeleteFlag(string flag) {
            return () => Game.Flags.Remove(flag);
        }
    }

    // some funcs in this and FireConditions are repeated, maybe make a common class?
    // can't inherit though, is smth like CommonConditions with common funcs a good idea?
    public static class DestructionConditions {
        public static Func<bool> All(params Func<bool>[] conditions) {
            return () => Array.TrueForAll(conditions, (condition) => condition.Invoke());
        }

        public static Func<bool> Any(params Func<bool>[] conditions) {
            return () => {
                return (
                    from condition in conditions
                    where condition.Invoke()
                    select condition
                ).Any();
            };
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