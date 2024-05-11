using Engine.Events;
using Engine.Exceptions;

using Serilog;

namespace Engine {
    public abstract class Registration {
        protected Registration() { }

        protected Registration(Registration registration) {
            _instances = registration._instances;
            OnRegistration = registration.OnRegistration;
        }

        protected Dictionary<string, object> _instances = [];

        public EngineEventHandler OnRegistration { get; protected set; } = new();
    }

    public class Registration<T> : Registration {
        public Registration() { }

        public Registration(Registration registration) : base(registration) { }

        public List<T> AllInstances => [..from instance in _instances.Values select (T)instance];

        public bool IsRegistered(string id) {
            return _instances.TryGetValue(id, out var _);
        }

        public T GetInstance(string id) {
            if (!_instances.TryGetValue(id, out var instance)) {
                throw new NotRegisteredException($"Location with id = {id} isn't registered");
            }
            return (T)instance;
        }

        public bool Register(T instance, string id) {
            if (!_instances.TryAdd(id, instance!)) {
                Log.Warning($"Location with id = {id} is already registered");
                return false;
            }
            OnRegistration.Invoke();
            return true;
        }
    }
}
