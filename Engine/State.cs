using Serilog;

namespace Engine {
    public record State {
        private Scene? currentScene;

        public bool IsRunning { get; set; } = true;
        public Scene? CurrentScene {
            get => currentScene;
            set {
                if (value is null) {
                    Log.Warning("You shouldn't change CurrentScene to null");
                }
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
