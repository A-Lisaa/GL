namespace Engine.UI {
    public partial class UI {
        public static class Actions {
            public static DefaultActionReturn Notify(string str) {
                return (object? _, EventArgs _) => Game.UI.Notify(str);
            }
        }
    }
}
