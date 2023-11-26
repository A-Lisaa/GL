namespace Engine {
    internal record SceneA : Scene {
        public override string Name => "Scene A";
        public override string Body => "Something's happening in Scene A";
        public override List<Act> Acts => [
            new Act<SceneB>(
                new OneTimeEngineEvent() {
                    Action = () => Console.WriteLine("Action 1 in SceneA used")
                }
            ) {
                Text = "Action 1"
            },
            new Act<SceneC>() {
                Text = "Action 2"
            }
        ];
    }

    internal record SceneB : Scene {
        public override string Name => "Scene B";
        public override string Body => "Something's happening in Scene B";
        public override List<Act> Acts => [
            new Act<SceneA>(
                new OneTimeEngineEvent() {
                    Action = () => Console.WriteLine("SceneA.Action1 used")
                }
            ) {
                Text = "Action 1"
            }
        ];
    }

    internal record SceneC : Scene {
        public override string Name => "Scene C";
        public override string Body => "Something's happening in Scene B";
        public override List<Act> Acts => [
            new(
                new OneTimeEngineEvent() {
                    Action = () => Engine.IsRunning = false
                }
            ) {
                Text = "Action 1"
            }
        ];
    }

    internal static class Program {
        public static void Main() {
            //Engine.CurrentScene = new SceneA();

            //Engine.Run();

            Time time1 = new() {
                Year = 2,
                Hour = 4,
                Minute = 15
            };

            Time time2 = new() {
                Hour = 2,
                Minute = 30
            };

            Time time3 = time1 + time2;

            Console.WriteLine(time3);
        }
    }
}
