using Utils;

namespace Engine.UI {
    // specific UIs internals shouldn't be visible to other assemblies
#pragma warning disable S101
    public class CLI : UI {
#pragma warning restore S101
        private static void PrintScene(Scene scene) {
            Console.WriteLine($"Name = {scene.Name}");
            Console.WriteLine($"Body = {scene.Body}");
            Console.WriteLine("Acts: ");
            foreach (var (i, act) in scene.Acts.Enumerate()) {
                Console.WriteLine($"{i}) {act.Text}");
            }
        }

        private static int GetAct() {
            string? input = Console.ReadLine();
            Console.WriteLine();
            if (input is null) {
                Console.WriteLine("input is null, can't use");
                return -1;
            }
            if (!int.TryParse(input, out int act)) {
                Console.WriteLine("Input is not an integer");
                return -1;
            }
            return act;
        }

        public override void Notify(string str) {
            Console.WriteLine(str);
        }

        // rewrite this method and its use
        public override void Run() {
            while (Game.State.IsRunning) {
                Console.WriteLine("--------------------------------------------------------------------------------------");
                PrintScene(Scene.Current.Value);
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.Write("Enter the act number: ");
                int act = GetAct();
                if (act == -1)
                    continue;
                Scene.Current.Value.UseAct(act);
                Scene.Current.Value.OnStart.Invoke();
                // should the global events be called in there? we'll have to change things like this in every UI child
                Game.Events.Invoke();
            }
        }
    }
}
