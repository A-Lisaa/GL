using Engine.Events;
using Engine.UI;

namespace Engine {
    public abstract class Game {
        public static class FireConditions {
            // these two are both in this and Destruction, not good
            public static Func<bool> HasFlag(string flag) {
                return () => Flags.Contains(flag);
            }

            public static Func<bool> HasNoFlag(string flag) {
                return () => !Flags.Contains(flag);
            }
        }

        public static class Actions {
            public static Action SetFlag(string flag) {
                return () => Flags.Add(flag);
            }

            public static Action DeleteFlag(string flag) {
                return () => Flags.Remove(flag);
            }

            public static Action AddEvent(EngineEvent @event) {
                return () => Events.Add(@event);
            }
        }

        public static class DestructionConditions {
            public static Func<bool> HasFlag(string flag) {
                return () => Flags.Contains(flag);
            }

            public static Func<bool> HasNoFlag(string flag) {
                return () => !Flags.Contains(flag);
            }
        }

        public abstract BaseUI UI { get; }
        public static State State { get; } = new State();
        public static HashSet<string> Flags { get; } = [];
        public static Dictionary<string, int> Counters { get; } = [];
        public static EngineEventHandler Events { get; } = new();

        public virtual void Run() {
            UI.Run();
        }
    }
}
