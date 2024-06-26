namespace Engine.UI {
    public abstract partial class UI {
        public abstract void Notify(string str);

        public abstract void ShowLocation(Location location);

        public abstract void ShowScene(Scene scene);

        public abstract void Update();

        public abstract void Run();
    }
}
