using Engine.Events;

using Serilog;

using System.Collections.Generic;

namespace Engine.Interfaces {
    public class NotRegisteredException : Exception {
        public NotRegisteredException() { }
        public NotRegisteredException(string? message) : base(message) { }
        public NotRegisteredException(string? message, Exception? innerException) : base(message, innerException) { }
    }

    public interface IRegistrable<T> where T : IRegistrable<T> {
        protected static Dictionary<string, T> Instances { get; set; } = [];

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

        public abstract static Observable<T> Current { get; }

        public abstract static List<T> AllInstances { get; }

        public abstract static T GetInstance(string id);

        public abstract bool Register(string id);

        public abstract EngineEventHandler OnRegistration { get; }
    }
}
