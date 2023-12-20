using Engine.Events;

namespace Engine.Time {
    public record Time {
        private readonly DateTime dateTime = new();

        public EngineEventHandler MinuteChanged { get; } = new();
        public EngineEventHandler HourChanged { get; } = new();
        public EngineEventHandler DayChanged { get; } = new();
        public EngineEventHandler WeekChanged { get; } = new();
        public EngineEventHandler MonthChanged { get; } = new();
        public EngineEventHandler YearChanged { get; } = new();

        public void AddMinutes(double value) {
            dateTime.AddMinutes(value);
            MinuteChanged.Invoke();
        }
    }
}
