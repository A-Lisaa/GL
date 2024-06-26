using Engine;
using Engine.Events;

namespace GirlLife {
    public record MyRoom : Location {
        private bool hasParrot;

        public record TalkWithSister : Scene {
            public override string Body { get; set; } = "Talking with my sister";
        }

        public override string Name { get; set; } = "My Room";
        public override string Body { get; set; } = "Just my room";
        public override List<Act> Acts { get; } = [
            new Passage("Hallway")
        ];

        public bool HasParrot {
            get => hasParrot;
            set {
                hasParrot = value;
                if (hasParrot) {
                    Body += "\n\nYour parrot is sitting in his cage";
                }
            }
        }
    }

    public record Hallway : Location {
        public override string Name { get; set; } = "Hallway";
        public override string Body { get; set; } = "Hallway of my apartment";
        public override List<Act> Acts { get; } = [
            new Passage("MyRoom"),
            new Passage("Street")
        ];
    }

    public record Street : Location {
        public override string Name { get; set; } = "Lenin Street";
        public override string Body { get; set; } = "The main street of the town";
        public override List<Act> Acts { get; } = [
            new Passage("Hallway"),
            new Passage("Mall")
        ];

        public override bool IsOutdoor { get; set; } = true;
    }

    public record Mall : Location {
        public override string Name { get; set; } = "Mall";
        public override string Body { get; set; } = "City mall";
        public override List<Act> Acts { get; } = [
            new Passage("Street"),
            new Passage("ZooShop")
        ];
    }

    public record ZooShop : Location {
        public override string Name { get; set; } = "Zoo Shop";
        public override string Body { get; set; } = "Animals are sold here";
        public override List<Act> Acts { get; } = [
            new Passage("Mall"),
            new Act(EngineEvent.FromAction((object? sender, EventArgs _) => { Location.Registration.GetInstance<MyRoom>().HasParrot = true; ((Act)sender).IsActive = false; } )) { Text = "Buy a parrot" }
        ];
    }

    public static class Locations {
        public static void RegisterLocations() {
            Location.Registration.Register<MyRoom>();
            Location.Registration.Register<Hallway>();
            Location.Registration.Register<Street>();
            Location.Registration.Register<Mall>();
            Location.Registration.Register<ZooShop>();
        }
    }
}
