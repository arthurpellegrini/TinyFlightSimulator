using System;
using System.Collections.Generic;

namespace EventManager
{
  /// Event Manager manages publishing raised events to subscribing/listening classes.
  /// 
  /// @example subscribe
  /// EventManager.Instance.AddListener
  /// <SomethingHappenedEvent>
  ///     (OnSomethingHappened);
  ///     @example unsubscribe
  ///     EventManager.Instance.RemoveListener
  ///     <SomethingHappenedEvent>
  ///         (OnSomethingHappened);
  ///         @example publish an event
  ///         EventManager.Instance.Raise(new SomethingHappenedEvent());
  ///         This class is a minor variation on http://www.willrmiller.com/?p=87
  public class EventManager
    {
        public delegate void EventDelegate<T>(T e) where T : Event;

        private static EventManager instance;

        /// Lookups only, there is one delegate lookup per listener
        private readonly Dictionary<Delegate, EventDelegate> delegateLookup = new();

        /// The actual delegate, there is one delegate per unique event. Each
        /// delegate has multiple invocation list items.
        private readonly Dictionary<Type, EventDelegate> delegates = new();

        public static EventManager Instance
        {
            get
            {
                if (instance == null) instance = new EventManager();

                return instance;
            }
        }

        /// The count of delegate lookups. The delegate lookups will increase by
        /// one for each unique AddListener. Useful for debugging and not much else.
        public int DelegateLookupCount => delegateLookup.Count;

        /// Add the delegate.
        public void AddListener<T>(EventDelegate<T> del) where T : Event
        {
            if (delegateLookup.ContainsKey(del)) return;

            // Create a new non-generic delegate which calls our generic one.  This
            // is the delegate we actually invoke.
            EventDelegate internalDelegate = e => del((T)e);
            delegateLookup[del] = internalDelegate;

            EventDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
                delegates[typeof(T)] = tempDel += internalDelegate;
            else
                delegates[typeof(T)] = internalDelegate;
        }

        /// Remove the delegate. Can be called multiple times on same delegate.
        public void RemoveListener<T>(EventDelegate<T> del) where T : Event
        {
            EventDelegate internalDelegate;
            if (delegateLookup.TryGetValue(del, out internalDelegate))
            {
                EventDelegate tempDel;
                if (delegates.TryGetValue(typeof(T), out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                        delegates.Remove(typeof(T));
                    else
                        delegates[typeof(T)] = tempDel;
                }

                delegateLookup.Remove(del);
            }
        }

        /// Raise the event to all the listeners
        public void Raise(Event e)
        {
            EventDelegate del;
            if (delegates.TryGetValue(e.GetType(), out del)) del.Invoke(e);
        }

        private delegate void EventDelegate(Event e);
    }
}