using Engine.Events;
using Engine.Interfaces;

namespace GirlLife {
    public record Location : Engine.Location {
        //public new static List<Location> AllInstances => IRegistrable<Location>.allInstances;

        //public override bool Register(string id) {
        //    return ((IRegistrable<Location>)this).register(id);
        //}

        //public new static Location GetInstance(string id) {
        //    return IRegistrable<Location>.getInstance(id);
        //}

        //public new static EngineEventHandler OnRegistration { get; } = new();

        public bool IsOutdoor { get; set; }
    }
}
