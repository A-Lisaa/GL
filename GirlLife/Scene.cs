using Engine;

namespace GirlLife {
    public record Scene : Engine.Scene {
        protected internal new static Registration<Scene> Registration { get; } = new(Engine.Scene.Registration);
    }
}
