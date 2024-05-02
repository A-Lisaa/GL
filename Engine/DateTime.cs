using Engine.Events;

namespace Engine {
    public partial record EngineDateTime(DateTime dateTime) {
        private DateTime dateTime = dateTime;

        public int Minute => dateTime.Minute;
        public int Hour => dateTime.Hour;
        public int Day => dateTime.Day;
        public DayOfWeek DayOfWeek => dateTime.DayOfWeek;
        public int Month => dateTime.Month;
        public int Year => dateTime.Year;

        public void AddMinutes(int minutes) {
            if (dateTime.Minute + minutes >= 60) {
                AddHours(minutes / 60);
                dateTime = dateTime.AddMinutes(minutes % 60);
                return;
            }
            dateTime = dateTime.AddMinutes(minutes);
            OnMinuteChange.Invoke();
        }

        public void AddHours(int hours) {
            if (dateTime.Hour + hours >= 24) {
                AddDays(hours / 24);
                dateTime = dateTime.AddHours(hours % 24);
                return;
            }
            dateTime = dateTime.AddHours(hours);
            OnHourChange.Invoke();
        }

        public void AddDays(int days) {
            // DayOfWeek.Sunday is 0, make it 7
            int dayOfWeek = dateTime.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)dateTime.DayOfWeek;
            dateTime = dateTime.AddDays(days);
            OnDayChange.Invoke();
            if (dayOfWeek + days >= 8) {
                OnWeekChange.Invoke();
            }
        }

        public EngineEventHandler OnMinuteChange { get; } = new();
        public EngineEventHandler OnHourChange { get; } = new();
        public EngineEventHandler OnDayChange { get; } = new();
        public EngineEventHandler OnWeekChange { get; } = new();
    }
}
