using Serilog;

namespace Engine.Interfaces {
    public class NotRegisteredException : Exception {
        public NotRegisteredException() {}
        public NotRegisteredException(string? message) : base(message) {}
        public NotRegisteredException(string? message, Exception? innerException) : base(message, innerException) {}
    }

    public interface IRegistrable<T> where T : IRegistrable<T> {
        protected abstract static Dictionary<string, T> Instances { get; set; }

        public abstract bool Register(string id);

        public abstract static T GetInstance(string id);

        public abstract static Observable<T> Current { get; }

        public abstract static List<T> AllInstances { get; }
    }
}
