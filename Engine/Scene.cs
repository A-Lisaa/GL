using Serilog;

namespace Engine {
    public abstract record Scene {
        public abstract string Name { get; }
        public abstract string Body { get; }
        public abstract List<Act> Acts { get; }

        public virtual bool CanUseAct(int actNumber) {
            if (actNumber < 0) {
                Log.Error("actNumber can't be less than 0");
                return false;
            }
            if (actNumber >= Acts.Count) {
                Log.Error($"actNumber is bigger than Acts.Count - 1 ({Acts.Count-1})");
                return false;
            }
            return true;
        }

        public virtual void UseAct(int actNumber) {
            if (!CanUseAct(actNumber)) {
                return;
            }
            Log.Debug($"Using act {actNumber} in Scene {Name}");
            Acts[actNumber].Use();
        }
    }
}
