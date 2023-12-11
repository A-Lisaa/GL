using Serilog;

namespace Engine.UI {
#pragma warning disable S101
    public class CLI : BaseUI {
#pragma warning restore S101
        private static void PrintScene(Scene scene) {
            Console.WriteLine($"\nName = {scene.Name}");
            Console.WriteLine($"Body = {scene.Body}");
            Console.WriteLine("Acts: ");
            for (int i = 0; i < scene.Acts.Count; i++) {
                Act act = scene.Acts[i];
                Console.WriteLine($"{i}) {act.Text}");
            }
        }

        // should it just return input though?
        public override int? GetInput() {
            string? input = Console.ReadLine();
            if (!int.TryParse(input, out int actNumber)) {
                Log.Information("Invalid input");
                return null;
            }
            return actNumber;
        }

        public override void Run() {
            while (Game.State.IsRunning) {
                PrintScene(Game.State.CurrentScene);
                int? input = GetInput();
                if (input is null) {
                    Log.Debug("input is null, can't use");
                    continue;
                }
                Game.State.CurrentScene.UseAct((int)input);
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
