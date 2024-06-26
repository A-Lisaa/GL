using Utils;
using Serilog;
using Engine;
using Engine.Events;
using Engine.UI;
using WpfUI;

namespace GirlLife {
    internal static class Program {
#pragma warning disable S101
        public class CLI : Engine.UI.CLI {
#pragma warning restore S101
            public override void Notify(string str) {
                Console.Write("Notify from GL: ");
                Console.WriteLine(str);
            }
        }

        [STAThread]
        public static void Main(string[] args) {
            Log.Logger = Logger.GetLogger(args.Contains("--debug"), args.Contains("--consoleLog"));

            Location.Registration.OnRegistration.AddEvent(new() {
                Action = UI.Actions.Notify("Registered a Location"),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            Scene.Registration.OnRegistration.AddEvent(new() {
                Action = UI.Actions.Notify("Registered a Scene"),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            Game.UI = new Wpf();

            Locations.RegisterLocations();
            Location.Current = Location.Registration.GetInstance<Hallway>();

            Location.OnChange.AddEvent(new() {
                Action = (object? _, Location.OnChangeEventArgs args) => Game.UI.Notify($"{args.NewLocation.GetType().Name}.IsOutdoor = {((Location)args.NewLocation).IsOutdoor}"),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            Location.Registration.GetInstance<MyRoom>().OnEnter.AddEvent(new() {
                Action = (object? _, Location.OnEnterEventArgs args) => Game.UI.Notify($"Parrot status is {((MyRoom)args.EnteredLocation).HasParrot}"),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            Scenes.RegisterScenes();

            Game game = new();
            game.Run();
        }
    }
}