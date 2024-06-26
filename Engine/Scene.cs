using Engine.Events;

using Serilog;

namespace Engine {
    public abstract partial record Scene {
        public static Scene Chain(params Scene[] scenes) {
            for (int i = 0; i < scenes.Length - 1; i++) {
                scenes[i].Acts.Insert(0, new SceneStarter(scenes[i + 1]) { Text = "Continue" });
            }
            return scenes[0];
        }

        public static Registration<Scene> Registration { get; } = new();

        public abstract string Body { get; set; }
        public virtual List<Act> Acts { get; } = [];

        public EngineEventHandler OnStart { get; } = new();

        public void UseAct(int actNumber) {
            if (actNumber < 0 || actNumber >= Acts.Count) {
                Log.Error($"actNumber is out of bounds (< 0 or >= Acts.Count ({Acts.Count}))");
                return;
            }
            var act = Acts[actNumber];
            if (!act.IsActive) {
                Log.Debug($"act {actNumber} is not active");
                return;
            }
            Log.Debug($"Using act {actNumber}");
            act.Use();
        }
    }
}
