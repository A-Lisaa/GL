namespace Engine {
    public partial record Act {
        public class Actions {
            public static Action SetActive(Act act) {
                return () => act.IsActive = true;
            }

            public static Action SetInactive(Act act) {
                return () => act.IsActive = false;
            }

            public static Action SetForDestruction(Act act) {
                return () => act.IsForDestruction = true;
            }
        }
    }
}
