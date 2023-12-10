﻿using Serilog;

namespace Engine {
    public abstract record Scene {
        public abstract string Name { get; }
        public abstract string Body { get; }
        public abstract List<Act> Acts { get; }

        public virtual void UseAct(int actNumber) {
            if (actNumber < 0) {
                Log.Information("actNumber can't be less than 0");
                return;
            }
            if (actNumber >= Acts.Count) {
                Log.Information("actNumber is bigger than Acts.Count");
                return;
            }
            Log.Debug($"Using act {actNumber}");
            Acts[actNumber].Use();
        }
    }
}
