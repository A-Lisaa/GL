namespace Engine.Events {
    public partial record EngineEvent {
        public class CommonConditions {
            public static Func<bool> All(params Func<bool>[] conditions) {
                return () => Array.TrueForAll(conditions, (condition) => condition.Invoke());
            }

            public static Func<bool> Any(params Func<bool>[] conditions) {
                return () => Array.Exists(conditions, (condition) => condition.Invoke());
            }

            // should it be common or separate?
            private readonly static Dictionary<int, int> NTimesCounters = [];
            private static int lastNTimesIndex;
            public static Func<bool> NTimes(int times) {
                int index = lastNTimesIndex++;
                NTimesCounters.Add(index, 0);
                return () => {
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
