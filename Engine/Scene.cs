namespace Engine {
    public abstract record Scene {
        public abstract string Name { get; }
        public abstract string Body { get; }
        public abstract List<Act> Acts { get; }

        public virtual void Show() {
            Console.WriteLine($"\nName = {Name}");
            Console.WriteLine($"Body = {Body}");
            Console.WriteLine("Acts: ");
            for (int i = 0; i < Acts.Count; i++) {
                Act act = Acts[i];
                Console.WriteLine($"{i}) {act.Text}");
            }
        }

        public virtual void WaitInput() {
            string? input = Console.ReadLine();
            if (input is null) {
                return;
            }
            UseAct(int.Parse(input));
        }

        public virtual void UseAct(int actNumber) {
            if (actNumber >= Acts.Count) {
                Console.WriteLine("actNumber is bigger than count of acts");
                return;
            }
            Console.WriteLine($"Using act {actNumber}");
            Acts[actNumber].Use();
        }
    }
}
