﻿namespace Engine {
    internal record SceneA : Scene {
        public override string Name => "Scene A";
        public override string Body => "Something's happening in Scene A";
        public override List<Act> Acts => [
            new Act<SceneB>(
                () => Console.WriteLine("Action 1 in SceneA used")
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
                () => Console.WriteLine("Action 1 in SceneB used")
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
                () => Engine.IsRunning = false
            ) {
                Text = "Action 1"
            }
        ];
    }

    internal static class Program {
        public static void Main() {
            Engine.CurrentScene = new SceneA();

            Engine.Run();
        }
    }
}