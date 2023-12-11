using Serilog;

namespace Engine {
    public record State {
        private sealed record EmptyScene : Scene {
            public override string Name => "Empty Technical Scene";

            public override string Body => "Nothing to see here";

            public override List<Act> Acts => [];
        }
        private Scene currentScene = new EmptyScene();

        public bool IsRunning { get; set; } = true;
        // it's just a property with event fired on its change, there should be smth default doing this
        public Scene CurrentScene {
            get => currentScene;
            set {
                currentScene = value;
                SceneChange?.Invoke();
            }
        }

        public event Action? SceneChange;
    }

    public static class StateActions {
        public static Action StopRunning() {
            return () => Game.State.IsRunning = false;
        }

        public static Action ChangeScene(Scene scene) {
            return () => Game.State.CurrentScene = scene;
        }
    }
}
