namespace Engine.Events {
    public partial record EngineEvent {
        public class CommonConditions {
            public static Func<object?, TEventArgs, bool> All<TEventArgs>(params Func<object?, TEventArgs, bool>[] conditions) where TEventArgs : EventArgs {
                return (object? sender, TEventArgs eventArgs) => Array.TrueForAll(conditions, (condition) => condition.Invoke(sender, eventArgs));
            }

            public static Func<object?, TEventArgs, bool> Any<TEventArgs>(params Func<object?, TEventArgs, bool>[] conditions) where TEventArgs : EventArgs {
                return (object? sender, TEventArgs eventArgs) => Array.Exists(conditions, (condition) => condition.Invoke(sender, eventArgs));
            }

            // should it be common or separate?
            private readonly static Dictionary<int, int> NTimesCounters = [];
            private static int lastNTimesIndex;
            public static DefaultConditionReturn NTimes(int times) {
                int index = lastNTimesIndex++;
                NTimesCounters.Add(index, 0);
                return (object? _, EventArgs _) => {
                    if (++NTimesCounters[index] == times) {
                        NTimesCounters.Remove(index);
                        return true;
                    }
                    return false;
                };
            }
        }

        public class FireConditions : CommonConditions {
            public static DefaultConditionReturn Always() {
                return (object? _, EventArgs _) => true;
            }

            public static DefaultConditionReturn Chance(double chance) {
                return (object? _, EventArgs _) => Utility.Random.NextDouble() < chance;
            }
        }

        public static class Actions {
            public static Action<object?, TEventArgs> Chain<TEventArgs>(params Action<object?, TEventArgs>[] actions) where TEventArgs : EventArgs {
                return (object? sender, TEventArgs eventArgs) => {
                    foreach (var action in actions) {
                        action.Invoke(sender, eventArgs);
                    }
                };
            }
        }

        // some funcs in this and FireConditions are repeated, maybe make a common class?
        // can't inherit though, is smth like CommonConditions with common funcs a good idea?
        public class DestructionConditions : CommonConditions {
            // shouldn't be a case of NTimes to not read/write to Dictionary
            public static DefaultConditionReturn OneTime() {
                return (object? _, EventArgs _) => true;
            }

            public static DefaultConditionReturn Never() {
                return (object? _, EventArgs _) => false;
            }
        }
    }
}
