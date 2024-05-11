using Engine;
using Engine.Events;
using Engine.UI;

namespace GirlLife {
    public static class Locations {
        public static void CreateLocations() {
            var sceneAStarter = new SceneStarter("SceneA") {
                Text = "Scene A",
                UsesLeft = 1,
            };
            sceneAStarter.OnUsesSpent.Add(Act.Actions.SetForDestruction(sceneAStarter));
            var myRoom = new Location() {
                Name = "My Room",
                Body = "Just my room",
                Acts = [
                    new Passage("hallway"),
                    new Act(
                        new EngineEvent() {
                            Action = UI.Actions.Notify($"Current time is: {Game.DateTime}"),
                            DestructionCondition = EngineEvent.DestructionConditions.Never()
                        }
                    ) {
                        Text = "Look at the watch"
                    },
                    sceneAStarter,
                    new SceneStarter("SceneB") {
                        Text = "Scene B"
                    }
                ]
            };
            Location.Registration.Register(myRoom, "myRoom");

            Location.Registration.Register(new Location() {
                Name = "Hallway",
                Body = "Hallway is fine",
                Acts = [
                    new Passage("street"),
                    new Act(
                        EngineEvent.FromAction(Game.Actions.StopRunning())
                    ) {
                        Text = "Exit"
                    }
                ]
            }, "hallway");

            Location.Registration.Register(new Location() {
                Name = "Street",
                Body = "Lenin Street",
                Acts = [
                    new Passage("hallway")
                ],
                IsOutdoor = true
            }, "street");
        }
    }
}
