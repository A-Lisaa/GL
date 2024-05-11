using Engine;
using Engine.Events;

namespace GirlLife {
    public record Location : Engine.Location {
        protected internal new static Registration<Location> Registration { get; } = new(Engine.Location.Registration);

        protected internal new static Location Current {
            get {
                return (Location)CurrentInner;
            }
            set {
                CurrentInner = value;
                OnChange.Invoke();
            }
        }

        public bool IsOutdoor { get; set; }
    }
}
