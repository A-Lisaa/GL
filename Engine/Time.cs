//namespace Engine {
//    public record Time {
//        private readonly DateTime dateTime = new();

//        public EngineEventHandler MinutesChanged { get; } = new();

//        public void AddMinutes(double value) {
//            dateTime.AddMinutes(value);
//            MinutesChanged.Invoke();
//        }
//    }
//}
