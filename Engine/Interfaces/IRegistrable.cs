using Engine.Events;
using Engine.Exceptions;

using Serilog;

namespace Engine.Interfaces {
    public partial interface IRegistrable<T> {
        protected static Dictionary<string, T> Instances { get; set; } = [];

        public static EngineEventHandler OnRegistration { get; } = new();

#pragma warning disable IDE1006
        protected static List<T> allInstances => [.. Instances.Values];

        protected static T getInstance(string id) {
            if (!Instances.TryGetValue(id, out var instance)) {
                throw new NotRegisteredException($"Location with id = {id} isn't registered");
            }
            return instance;
        }

        public bool register(string id) {
            if (!IRegistrable<T>.Instances.TryAdd(id, (T)this)) {
                Log.Warning($"Location with id = {id} is already registered");
                return false;
            }
            OnRegistration.Invoke();
            return true;
        }
#pragma warning restore IDE1006

        public abstract static List<T> AllInstances { get; }

        public abstract static T GetInstance(string id);

        public abstract bool Register(string id);
    }
}
