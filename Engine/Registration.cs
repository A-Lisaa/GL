using Engine.Events;
using Engine.Exceptions;

using Serilog;

namespace Engine {
    public class Registration<T> {
        private readonly Dictionary<string, T> _instances = [];

        public class OnRegistrationEventArgs : EventArgs {
            public required T Instance { get; init; }

            public required string Id { get; init; }
        }

        public EngineEventHandler<OnRegistrationEventArgs> OnRegistration { get; protected set; } = new();

        public List<T> AllInstances => [..from instance in _instances.Values select instance];

        public T this[string index] {
            get => GetInstance(index);
            set => Register(value, index, true);
        }

        public bool IsRegistered(string id) {
            return _instances.TryGetValue(id, out var _);
        }

        public TInstance GetInstance<TInstance>() where TInstance : T {
            return (TInstance)GetInstance(typeof(TInstance).Name)!;
        }

        public T GetInstance(string id) {
            if (!_instances.TryGetValue(id, out var instance)) {
                throw new NotRegisteredException($"Location with id = {id} isn't registered");
            }
            return instance;
        }

        public bool Register<TRegister>(bool reassign = false) where TRegister : T, new() {
            return Register(new TRegister(), reassign);
        }

        public bool Register(T instance, bool reassign = false) {
            return Register(instance, instance!.GetType().Name, reassign);
        }

        public bool Register(T instance, string id, bool reassign = false) {
            ArgumentNullException.ThrowIfNull(instance);
            if (!_instances.TryAdd(id, instance)) {
                if (!reassign) {
                    Log.Warning($"Location with id = {id} is already registered");
                    return false;
                }
                _instances[id] = instance;
            }
            OnRegistration.Invoke(this, new() { Instance = instance, Id = id });
            return true;
        }
    }
}
