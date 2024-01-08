using Serilog;

namespace Engine {
    public abstract record Scene {
        public abstract string Name { get; }
        public abstract string Body { get; }
        public abstract List<Act> Acts { get; }
        // size of empty EngineEventHandler? if 0, maybe make static EEH for Enter/Exit or smth, could do that in other parts

        protected virtual bool CanUseAct(int actNumber) {
            if (actNumber < 0 || actNumber >= Acts.Count) {
                Log.Error("actNumber out of bounds (< 0 or >= Acts.Count)");
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
