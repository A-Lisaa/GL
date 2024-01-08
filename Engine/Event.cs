using System.Collections;

namespace Engine.Events {
    public record EngineEvent : IComparable<EngineEvent> {
        // returns true when the event is to be invoked
        public required Func<bool> FireCondition { protected get; init; }

        public required Action Action { protected get; init; }

        // 1) returns true when the event is to be destructed
        // 2) default set to OneTime seems like a good idea to avoid stupids setting it to Never
        // maybe should discourage and warn about using Never and similar conditions
        public virtual Func<bool> DestructionCondition { protected get; init; } = DestructionConditions.OneTime();

        // do we need this or should we use DestructionCondition directly?
        // probably not as IsForDestruction could be set by smth else
        public bool IsForDestruction { get; protected set; }

        // priority in which the event will be invoked be event handler, the lower the earlier
        public int Priority { get; init; }

        // Action will be invoked this amount of times on each event invocation,
        // it's this way so that we don't need to create multiple identical events
        public int TimesToInvokeAction { get; set; } = 1;

        public int CompareTo(EngineEvent? other) {
            if (other == null) return 1;
            return Priority.CompareTo(other.Priority);
        }

        public virtual void Invoke() {
            if (FireCondition.Invoke()) {
                for (int i = 0; i < TimesToInvokeAction; i++) {
                    Action.Invoke();
                }
            }
            IsForDestruction = DestructionCondition.Invoke();
        }
    }

    public static class FireConditions {
        public static Func<bool> All(params Func<bool>[] conditions) {
            return () => Array.TrueForAll(conditions, (condition) => condition.Invoke());
        }

        public static Func<bool> Any(params Func<bool>[] conditions) {
            return () => Array.Exists(conditions, (condition) => condition.Invoke());
        }

        public static Func<bool> Always() {
            return () => true;
        }

        // move to Game when works
        // it should be global otherwise we'll get a bunch of FireNTimes for each scene creation
        private static int lastNTimesIndex;
        public static Func<bool> NTimes(int times) {
            string currentNTimesName = $"FireNTimes{lastNTimesIndex++}";
            Game.Counters.TryAdd(currentNTimesName, 0);
            return () => {
                if (++Game.Counters[currentNTimesName] == times) {
                    Game.Counters.Remove(currentNTimesName);
                    return true;
                }
                return false;
            };
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
    }

    // some funcs in this and FireConditions are repeated, maybe make a common class?
    // can't inherit though, is smth like CommonConditions with common funcs a good idea?
    public static class DestructionConditions {
        public static Func<bool> All(params Func<bool>[] conditions) {
            return () => Array.TrueForAll(conditions, (condition) => condition.Invoke());
        }

        public static Func<bool> Any(params Func<bool>[] conditions) {
            return () => Array.Exists(conditions, (condition) => condition.Invoke());
        }

        // should it be a case of NTimes? probably not to not read/write to Game.Counters
        public static Func<bool> OneTime() {
            return () => true;
        }

        // shouldn't be common as Destruiction and Fire shall have different hames in Counters
        private static int lastNTimesIndex;
        public static Func<bool> NTimes(int times) {
            string currentNTimesName = $"DestructionNTimes{lastNTimesIndex++}";
            Game.Counters.TryAdd(currentNTimesName, 0);
            return () => {
                if (++Game.Counters[currentNTimesName] == times) {
                    Game.Counters.Remove(currentNTimesName);
                    return true;
                }
                return false;
            };
        }

        public static Func<bool> Never() {
            return () => false;
        }
    }
}