using Utils;
using Serilog;
using Engine;
using Engine.Events;
using Engine.UI;

namespace GirlLife {
    internal static class Program {
        public class CLI : Engine.UI.CLI {
            public override void Notify(string str) {
                Console.Write("Notify from GL: ");
                Console.WriteLine(str);
            }
        }

        public static void Main(string[] args) {
            Log.Logger = Logger.GetLogger(args.Contains("--debug"), args.Contains("--consoleLog"));

            Location.Registration.OnRegistration.Add(new EngineEvent() {
                Action = UI.Actions.Notify("Registered a Location"),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            Scene.Registration.OnRegistration.Add(new EngineEvent() {
                Action = UI.Actions.Notify("Registered a Scene"),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            Game.UI = new CLI();

            Locations.CreateLocations();
            Location.Current = Location.Registration.GetInstance("myRoom");
            Location.OnChange.Add(new EngineEvent() {
                Action = () => Console.WriteLine(Location.Current.IsOutdoor),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            Scenes.CreateScenes();

            Game game = new();
            game.Run();
        }
    }
}