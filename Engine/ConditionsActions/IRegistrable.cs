namespace Engine.Interfaces {
    public partial interface IRegistrable<T> {
        public class FireConditions {
            public static Func<bool> OnRegistered(string name) {
                return () => Instances.ContainsKey(name);
            }
        }
    }
}
