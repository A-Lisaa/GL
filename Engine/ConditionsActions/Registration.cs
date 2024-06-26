namespace Engine {
    public partial class Registration<T> {
        public class CommonConditions {
            public static Func<object?, OnRegistrationEventArgs, bool> WhenRegistered<TInstance>() where TInstance : T {
                return WhenRegistered(typeof(TInstance).Name);
            }

            public static Func<object?, OnRegistrationEventArgs, bool> WhenRegistered(string id) {
                return (object? _, OnRegistrationEventArgs args) => args.Id == id;
            }
        }

        public class FireConditions : CommonConditions;

        public class DestructionConditions : CommonConditions;
    }
}
