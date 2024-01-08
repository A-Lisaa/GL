using Engine;
using Engine.Events;
using Engine.UI;

using Serilog;

namespace Game {
    public class Game : Engine.Game {
        public override BaseUI UI { get; } = new CLI();
    }

    internal record SceneA : Scene {
        public override string Name => "Scene A";
        public override string Body => "Something's happening in Scene A";
        public override List<Act> Acts => [
            new Act<SceneB>(
                new EngineEvent() {
                    FireCondition = FireConditions.Always(),
                    Action = CLI.Actions.Print("This should be after 'went to'"),
                    Priority = 1,
                    TimesToInvokeAction = 2
                },
                new EngineEvent() {
                    FireCondition = FireConditions.Always(),
                    Action = CLI.Actions.Print("Went to SceneB from SceneA")
                },
                new EngineEvent() {
                    FireCondition = FireConditions.Always(),
                    Action = CLI.Actions.Print("This should be before 'went to'"),
                    Priority = -1,
                    TimesToInvokeAction = 3
                },
                new EngineEvent() {
                    FireCondition = FireConditions.All(
                        Game.FireConditions.HasFlag("SceneAAction2Run"),
                        Game.FireConditions.HasNoFlag("SceneAAction2RunShowed")
                    ),
                    Action = Actions.Chain(
                        // using specific UI member is kinda stupid when we want to change them
                        CLI.Actions.Print("Scene C was executed before"),
                        Game.Actions.SetFlag("SceneAAction2RunShowed")
                    )
                },
                new EngineEvent() {
                    FireCondition = FireConditions.Always(),
                    Action = () => {
                        Game.Counters.TryAdd("ABTimes", 0);
                        CLI.Actions.Print($"Went to SceneB from SceneA {++Game.Counters["ABTimes"]} times")();
                    }
                }
            ) {
                Text = "To SceneB"
            },
            new Act<SceneC>(
                new EngineEvent() {
                    FireCondition = Game.FireConditions.HasNoFlag("SceneAAction2Run"),
                    Action = Game.Actions.SetFlag("SceneAAction2Run")
                }
            ) {
                Text = "To SceneC"
            }
        ];
    }

    internal record SceneB : Scene {
        public override string Name => "Scene B";
        public override string Body => "Something's happening in Scene B";
        public override List<Act> Acts => [
            new Act<SceneA>(
                new EngineEvent() {
                    FireCondition = FireConditions.Always(),
                    Action = CLI.Actions.Print("Went to SceneA from SceneB")
                }
            ) {
                Text = "To SceneA"
            }
        ];
    }

    internal record SceneC : Scene {
        public override string Name => "Scene C";
        public override string Body => "Something's happening in Scene C";
        public override List<Act> Acts => [
            new Act(
                new EngineEvent() {
                    FireCondition = FireConditions.Always(),
                    Action = State.Actions.StopRunning()
                }
            ) {
                Text = "Stop"
            },
            new Act<SceneA>() {
                Text = "To SceneA"
            }
        ];
    }

    internal static class Program {
        public static Serilog.Core.Logger GetLogger(bool debugEnabled = false, bool consoleLog = false) {
            var loggerConfiguration = new LoggerConfiguration();

            if (consoleLog) {
                loggerConfiguration.WriteTo.Console();
            }
            else {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                // I suspect that retainedFileCountLimit doesn't work, and we don't want like 1488 log files
                loggerConfiguration.WriteTo.File($"logs/{timestamp}.log", retainedFileCountLimit: 5);
            }

#if DEBUG
            loggerConfiguration.MinimumLevel.Debug();
#endif
            if (debugEnabled) {
                loggerConfiguration.MinimumLevel.Debug();
            }

            return loggerConfiguration.CreateLogger();
        }

        public static void Main(string[] args) {
            Log.Logger = GetLogger(args.Contains("--debug"), args.Contains("--consoleLog"));

            Game.State.CurrentScene = new SceneA();

            Game.Events.Add(
                new EngineEvent() {
                    FireCondition = Game.FireConditions.HasNoFlag("SceneAAction2Run"),
                    Action = CLI.Actions.Print("SceneAAction2Run hasn't been set"),
                    DestructionCondition = Game.DestructionConditions.HasFlag("SceneAAction2Run"),
                }
            );

            Game game = new();
            game.Run();
        }
    }
}