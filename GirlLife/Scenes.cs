using Engine;

namespace GirlLife {
    public static class Scenes {
        public static void CreateScenes() {
            new Scene() {
                Body = "Scene A go brrr"
            }.Register("SceneA");

            new Scene() {
                Body = "Scene B lol"
            }.Register("SceneB");
        }
    }
}
