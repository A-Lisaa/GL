namespace GirlLife {
    internal class Character {
        public string Name { get; set; } = "Unknown";

        public double Weight { get; set; }
        public double Height { get; set; }

        public double BMI { get => Weight / (Height * Height); }
    }
}
