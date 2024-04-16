using Engine.Events;

namespace Engine {
    public partial class Game {
        public class CommonConditions {
            public static Func<bool> HasFlag(string flag) {
                return () => Flags.Contains(flag);
            }

            public static Func<bool> HasNoFlag(string flag) {
                return () => !Flags.Contains(flag);
            }
        }

        public class FireConditions : CommonConditions {

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

        public class DestructionConditions : CommonConditions {
        }
    }
}
