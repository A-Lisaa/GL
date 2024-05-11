namespace Engine {
    public partial record Act {
        public class Actions {
            public static Action<EventArgs> SetActive(Act act) {
                return (EventArgs _) => act.IsActive = true;
            }

            public static Action<EventArgs> SetInactive(Act act) {
                return (EventArgs _) => act.IsActive = false;
            }

            public static Action<EventArgs> SetForDestruction(Act act) {
                return (EventArgs _) => act.IsForDestruction = true;
            }
        }
    }
}
