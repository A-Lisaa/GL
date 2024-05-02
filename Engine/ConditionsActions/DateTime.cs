namespace Engine {
    public partial record EngineDateTime {
        public class CommonConditions {
            public static Func<bool> IsMinute(int minute) {
                return () => Game.DateTime.Minute == minute;
            }

            public static Func<bool> IsHour(int hour) {
                return () => Game.DateTime.Hour == hour;
            }

            public static Func<bool> IsDay(int day) {
                return () => Game.DateTime.Day == day;
            }

            public static Func<bool> IsDayOfWeek(DayOfWeek dayOfWeek) {
                return () => Game.DateTime.DayOfWeek == dayOfWeek;
            }

            public static Func<bool> IsMonth(int month) {
                return () => Game.DateTime.Month == month;
            }

            public static Func<bool> IsYear(int year) {
                return () => Game.DateTime.Year == year;
            }
        }

        public class FireConditions : CommonConditions;

        public class DestructionConditions : CommonConditions;
    }
}
