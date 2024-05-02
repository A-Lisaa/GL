using Utils;
using Serilog;
using Engine;
using Engine.Interfaces;
using Engine.Events;
using Engine.UI;

namespace GirlLife {
    internal static class Program {
        public static void Main(string[] args) {
            Log.Logger = Logger.GetLogger(args.Contains("--debug"), args.Contains("--consoleLog"));

            IRegistrable<Engine.Location>.OnRegistration.Add(new EngineEvent() {
                Action = UI.Actions.Notify("Registered a Location"),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            IRegistrable<Scene>.OnRegistration.Add(new EngineEvent() {
                Action = UI.Actions.Notify("Registered a Scene"),
                DestructionCondition = EngineEvent.DestructionConditions.Never()
            });

            Locations.CreateLocations();
            Location.Current.Value = Location.GetInstance("myRoom");

            Scenes.CreateScenes();

            Game game = new();
            game.Run();
        }
    }
}