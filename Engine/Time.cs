namespace Engine {
    public record Time {
        public Time() {
        }

        private Dictionary<uint, uint> Months {
            get => new() {
                { 1, 31 },
                { 2, IsLeapYear() ? (uint)29 : 28 },
                { 3, 31 },
                { 4, 30 },
                { 5, 31 },
                { 6, 30 },
                { 7, 31 },
                { 8, 31 },
                { 9, 30 },
                { 10, 31 },
                { 11, 30 },
                { 12, 31 }
            };
        }

        private uint? year;
        private uint? month;
        private uint? day;
        private uint? hour;
        private uint? minute;

        public uint? Year {
            get { return year; }
            set {
                if (value is null) {
                    return;
                }
                if (value == 0) {
                    Console.WriteLine("Year can't be 0");
                    return;
                }
                year = value;
            }
        }
        public uint? Month {
            get => month;
            set {
                if (value is null) {
                    return;
                }
                if (value > 12) {
                    Year += Month / 12;
                }
                month = value % 12;
            }
        }
        public uint? Day {
            get => day;
            set {
                if (value is null || Month is null) {
                    return;
                }
                if (value > Months[(uint)Month]) {
                    Month += value / Months[(uint)Month];
                }
                day = value % Months[(uint)Month];
            }
        }
        public uint? Hour {
            get => hour;
            set {
                if (value is null) {
                    return;
                }
                if (value >= 24) {
                    Day += value / 24;
                }
                hour = value % 24;
            }
        }
        public uint? Minute {
            get => minute;
            set {
                if (value is null) {
                    return;
                }
                if (value >= 60) {
                    Hour += value / 60;
                }
                minute = value % 60;
            }
        }

        public static bool IsLeapYear(uint year) {
            return year % 4 == 0 && year % 100 != 0 && year % 400 == 0;
        }
        public bool IsLeapYear() {
            if (Year is null) {
                return false;
            }
            return IsLeapYear((uint)Year);
        }

        public static Time operator +(Time a, Time b) {
            return new Time() {
                Year = a.Year + b.Year,
                Month = a.Month + b.Month,
                Day = a.Day + b.Day,
                Hour = a.Hour + b.Hour,
                Minute = a.Minute + b.Minute,
            };
        }

        public override string? ToString() {
            return $"{Hour}:{Minute} {Day}.{Month}.{Year}";
        }
    }
}
