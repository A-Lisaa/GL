using Utils;

namespace Engine.UI {
    // specific UIs internals shouldn't be visible to other assemblies
#pragma warning disable S101
    public class CLI : UI {
#pragma warning restore S101
        private static void PrintLocation(Location location) {
            Console.WriteLine($"Name = {location.Name}");
            Console.WriteLine($"Body = {location.Body}");
            Console.WriteLine("Acts: ");
            foreach (var (i, act) in location.Acts.Enumerate()) {
                Console.Write($"{i}) {act.Text}");
                if (!act.IsActive) {
                    Console.Write(" (disabled)");
                }
                Console.WriteLine();
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
            while (Game.IsRunning) {
                Console.WriteLine("--------------------------------------------------------------------------------------");
                PrintLocation(Location.Current.Value);
                Console.WriteLine("--------------------------------------------------------------------------------------");
                Console.Write("Enter the act number: ");
                int act = GetAct();
                if (act == -1)
                    continue;
                Location.Current.Value.UseAct(act);
                Location.Current.Value.OnEnter.Invoke();
                // should the global events be called in there? we'll have to change things like this in every UI child
                Game.Events.Invoke();
            }
        }
    }
}
