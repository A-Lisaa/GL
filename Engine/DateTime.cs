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
            int oldMinute = Minute;
            if (dateTime.Minute + minutes >= 60) {
                AddHours(minutes / 60);
                dateTime = dateTime.AddMinutes(minutes % 60);
                return;
            }
            dateTime = dateTime.AddMinutes(minutes);
            OnMinuteChange.Invoke(this, new() { OldMinute = oldMinute, NewMinute = Minute });
        }

        public void AddHours(int hours) {
            int oldHour = Hour;
            if (dateTime.Hour + hours >= 24) {
                AddDays(hours / 24);
                dateTime = dateTime.AddHours(hours % 24);
                return;
            }
            dateTime = dateTime.AddHours(hours);
            OnHourChange.Invoke(this, new() { OldHour = oldHour, NewHour = Hour });
        }

        public void AddDays(int days) {
            // DayOfWeek.Sunday is 0, make it 7
            int dayOfWeek = dateTime.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)dateTime.DayOfWeek;
            int oldDay = Day;
            dateTime = dateTime.AddDays(days);
            OnDayChange.Invoke(this, new() { OldDay = oldDay, NewDay = Day });
            if (dayOfWeek + days >= 8) {
                OnWeekChange.Invoke(this, new());
            }
        }

        public class OnMinuteChangeEventArgs : EventArgs {
            public required int OldMinute { get; init; }
            public required int NewMinute { get; init; }
        }

        public class OnHourChangeEventArgs : EventArgs {
            public required int OldHour { get; init; }
            public required int NewHour { get; init; }
        }

        public class OnDayChangeEventArgs : EventArgs {
            public required int OldDay { get; init; }
            public required int NewDay { get; init; }
        }

        public EngineEventHandler<OnMinuteChangeEventArgs> OnMinuteChange { get; } = new();
        public EngineEventHandler<OnHourChangeEventArgs> OnHourChange { get; } = new();
        public EngineEventHandler<OnDayChangeEventArgs> OnDayChange { get; } = new();
        public EngineEventHandler OnWeekChange { get; } = new();
    }
}
