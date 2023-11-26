namespace Engine {
    public static class Engine {
        private static Scene? currentScene;

        public static bool IsRunning { get; set; } = true;
        public static Scene? CurrentScene {
            get => currentScene;
            set {
                currentScene = value;
                SceneChange?.Invoke();
            }
        }
        public static List<string> Flags { get; } = [];

        public static event Action? SceneChange;

        public static void Run() {
            while (IsRunning) {
                if (CurrentScene is null) {
                    Console.WriteLine("Engine.CurrentScene is null, can't run");
                    return;
                }
                CurrentScene.Show();
                CurrentScene.WaitInput();
            }
        }
    }
}
