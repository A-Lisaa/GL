namespace Engine.UI {
    public abstract class UI {
        public static class Actions {
            public static Action Notify(string str) {
                return () => Game.UI.Notify(str);
            }
        }

        public abstract void Notify(string str);

        public abstract void Run();
    }
}
