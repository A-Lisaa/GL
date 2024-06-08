using Engine.Events;

namespace Engine {
    public partial class Game {
        public class CommonConditions {
            public static DefaultConditionReturn HasFlag(string flag) {
                return (object? _, EventArgs _) => Flags.Contains(flag);
            }

            public static DefaultConditionReturn HasNoFlag(string flag) {
                return (object? _, EventArgs _) => !Flags.Contains(flag);
            }
        }

        public class FireConditions : CommonConditions;

        public class DestructionConditions : CommonConditions;

        public class Actions {
            public static DefaultActionReturn SetFlag(string flag) {
                return (object? _, EventArgs _) => Flags.Add(flag);
            }

            public static DefaultActionReturn DeleteFlag(string flag) {
                return (object? _, EventArgs _) => Flags.Remove(flag);
            }

            public static DefaultActionReturn AddEvent(EngineEvent<EventArgs> @event) {
                return (object ? _, EventArgs _) => Events.AddEvent(@event);
            }

            public static DefaultActionReturn StopRunning() {
                return (object? _, EventArgs _) => IsRunning = false;
            }
        }
    }
}
