using Engine;

namespace GirlLife {
    public static class Scenes {
        public static void CreateScenes() {
            Scene.Registration.Register(new Scene() {
                Body = "Scene A go brrr"
            }, "SceneA");

            Scene.Registration.Register(new Scene() {
                Body = "Scene B lol"
            }, "SceneB");
        }
    }
}
