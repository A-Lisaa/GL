using Engine;
using Engine.Events;
using Engine.UI;

using Serilog;

namespace Game {
    public record State : Engine.State {
    }

    public class Game : Engine.Game {
        public override BaseUI UI { get; } = new CLI();
        public static Dictionary<string, int> Counters { get; } = [];
    }

    internal record SceneA : Scene {
        public override string Name => "Scene A";
        public override string Body => "Something's happening in Scene A";
        public override List<Act> Acts => [
            new Act<SceneB>(
                new EngineEvent() {
                    FireCondition = FireConditions.Always(),
                    Action = CLIActions.Print("Went to SceneB from SceneA")
                },
                new EngineEvent() {
                    FireCondition = FireConditions.All(
                        FireConditions.HasFlag("SceneAAction2Run"),
                        FireConditions.HasNoFlag("SceneAAction2RunShowed")
                    ),
                    Action = Actions.Chain(
                        CLIActions.Print("Scene C was executed before"),
                        Actions.SetFlag("SceneAAction2RunShowed")
                    )
                },
                new EngineEvent() {
                    FireCondition = FireConditions.Always(),
                    Action = () => {
                        if (!Game.Counters.TryGetValue("ABTimes", out int _)) {
                            Game.Counters["ABTimes"] = 0;
                        }
                        Game.Counters["ABTimes"]++;
                        CLIActions.Print($"Went to SceneB from SceneA {Game.Counters["ABTimes"]} times")();
                    }
                }
            ) {
                Text = "To SceneB"
            },
            new Act<SceneC>(
                new EngineEvent() {
                    FireCondition = FireConditions.HasNoFlag("SceneAAction2Run"),
                    Action = Actions.SetFlag("SceneAAction2Run")
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
                    Action = CLIActions.Print("Went to SceneA from SceneB")
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
                    Action = StateActions.StopRunning()
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
        public static void Main() {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Game.State.CurrentScene = new SceneA();

            Game.Events.Add(
                new EngineEvent() {
                    FireCondition = FireConditions.HasNoFlag("SceneAAction2Run"),
                    Action = CLIActions.Print("SceneAAction2Run hasn't been set"),
                    DestructionCondition = DestructionConditions.HasFlag("SceneAAction2Run")
                }
            );

            Game game = new();
            game.Run();
        }
    }
}