namespace GirlLife {
    public abstract record Location : Engine.Location {
        public virtual bool IsOutdoor { get; set; }
    }
}