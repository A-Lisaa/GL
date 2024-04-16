namespace Engine {
    public partial record Scene {
        public static class FireConditions {
            public static Func<bool> IsScene(Scene scene) {
                return () => Current == scene;
            }

            public static Func<bool> IsScene(string scene) {
                return () => IsScene(GetScene(scene))();
            }
        }
    }
}
