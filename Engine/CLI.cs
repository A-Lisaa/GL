using Serilog;

namespace Engine.UI {
#pragma warning disable S101
    public class CLI : BaseUI {
#pragma warning restore S101
        private static void PrintScene(Scene scene) {
            Console.WriteLine($"Name = {scene.Name}");
            Console.WriteLine($"Body = {scene.Body}");
            Console.WriteLine("Acts: ");
            for (int i = 0; i < scene.Acts.Count; i++) {
                Act act = scene.Acts[i];
                Console.WriteLine($"{i}) {act.Text}");
            }
        }

        // rewrite this method and its use
        public override void Run() {
            while (Game.State.IsRunning) {
                Console.WriteLine("--------------------------------------------------------------------------------------");
                PrintScene(Game.State.CurrentScene);
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.Write("Enter the act number: ");
                string? input = Console.ReadLine();
                Console.WriteLine();
                if (input is null) {
                    Log.Debug("input is null, can't use");
                    continue;
                }
                if (!int.TryParse(input, out int actNumber)) {
                    Log.Information("Invalid input");
                    continue;
                }
                Game.State.CurrentScene.UseAct(actNumber);
                // should the global events be called in there? we'll have to change things like this in every UI child
                Game.Events.Invoke();
            }
        }
    }

#pragma warning disable S101
    public static class CLIActions {
#pragma warning restore S101
        public static Action Print(string str) {
            return () => Console.WriteLine(str);
        }
    }
}
