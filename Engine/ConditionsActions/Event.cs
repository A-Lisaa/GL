namespace Engine.Events {
    public record EngineEvent {
        public class CommonConditions {
            public static Func<TEventArgs, bool> All<TEventArgs>(params Func<TEventArgs, bool>[] conditions) where TEventArgs : EventArgs {
                return (TEventArgs eventArgs) => Array.TrueForAll(conditions, (condition) => condition.Invoke(eventArgs));
            }

            public static Func<TEventArgs, bool> Any<TEventArgs>(params Func<TEventArgs, bool>[] conditions) where TEventArgs : EventArgs {
                return (TEventArgs eventArgs) => Array.Exists(conditions, (condition) => condition.Invoke(eventArgs));
            }

            // should it be common or separate?
            private readonly static Dictionary<int, int> NTimesCounters = [];
            private static int lastNTimesIndex;
            public static Func<EventArgs, bool> NTimes(int times) {
                int index = lastNTimesIndex++;
                NTimesCounters.Add(index, 0);
                return (EventArgs _) => {
                    if (++NTimesCounters[index] == times) {
                        NTimesCounters.Remove(index);
                        return true;
                    }
                    return false;
                };
            }
        }

        public class FireConditions : CommonConditions {
            public static Func<bool> Always() {
                return () => true;
            }

            public static Func<bool> Chance(double chance) {
                return () => Utility.Random.NextDouble() < chance;
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
        public class DestructionConditions : CommonConditions {
            // shouldn't be a case of NTimes to not read/write to Dictionary
            public static Func<bool> OneTime() {
                return () => true;
            }

            public static Func<bool> Never() {
                return () => false;
            }
        }
    }
}
