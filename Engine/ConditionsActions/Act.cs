namespace Engine {
    public partial record Act {
        public class Actions {
            public static DefaultActionReturn SetActive(Act act) {
                return (object? _, EventArgs _) => act.IsActive = true;
            }

            public static DefaultActionReturn SetInactive(Act act) {
                return (object? _, EventArgs _) => act.IsActive = false;
            }

            public static DefaultActionReturn SetForDestruction(Act act) {
                return (object? _, EventArgs _) => act.IsForDestruction = true;
            }
        }
    }
}
