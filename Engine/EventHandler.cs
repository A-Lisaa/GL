namespace Engine.Events {
    public class EngineEventHandler<TEventArgs> where TEventArgs : EventArgs {
        public EngineEventHandler() { }

        public EngineEventHandler(EngineEvent<TEventArgs>[] events) : this() {
            _events = [..events];
        }

        public EngineEventHandler(EngineEventHandler<TEventArgs> handler) : this() {
            _events = handler._events;
        }

        private readonly List<EngineEvent<TEventArgs>> _events = [];

        public class EventAddedEventArgs : EventArgs {
            public required EngineEvent<TEventArgs> EngineEvent { get; init; }
        }

        public class InvokedEventArgs : EventArgs {
            public required List<EngineEvent<TEventArgs>> RemovedEvents { get; init; }
        }

        public event EventHandler<EventAddedEventArgs>? OnEventAdded;
        public event EventHandler<InvokedEventArgs>? OnInvoked;

        public void AddEvent(EngineEvent<TEventArgs> engineEvent) {
            _events.Add(engineEvent);
            OnEventAdded?.Invoke(this, new() { EngineEvent = engineEvent });
        }

        public void AddAction(Action<object?, TEventArgs> action) {
            AddEvent(new EngineEvent<TEventArgs>() { Action = action });
        }

        public void AddRange(IEnumerable<EngineEvent<TEventArgs>> engineEvents) {
            foreach (var elem in engineEvents)
                AddEvent(elem);
        }

        public void Invoke(object? sender, TEventArgs eventArgs) {
            _events.Sort();
            foreach (var engineEvent in _events) {
                engineEvent.Invoke(sender, eventArgs);
            }
            // can't use RemoveAll as removed events are needed for InvokedEventArgs
            List<EngineEvent<TEventArgs>> newEvents = [];
            List<EngineEvent<TEventArgs>> removedEvents = [];
            foreach (var engineEvent in _events) {
                if (engineEvent.IsForDestruction)
                    removedEvents.Add(engineEvent);
                else
                    newEvents.Add(engineEvent);
            }
            _events.Clear();
            _events.AddRange(newEvents);
            OnInvoked?.Invoke(this, new() { RemovedEvents = removedEvents });
        }
    }

    public class EngineEventHandler(params EngineEvent<EventArgs>[] engineEvents) : EngineEventHandler<EventArgs>(engineEvents);
}