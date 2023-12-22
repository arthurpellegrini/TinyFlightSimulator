namespace EventManager
{
  /// Interface for event handlers
  public interface IEventHandler
    {
      /// Subscribe to events
      /// 
      /// @example
      /// Events.AddListener
      /// <MoveResolvedEvent>
      ///     (OnMoveResolved);
      ///     or
      ///     EventManager.OnSetRule += OnSetRule;
      void SubscribeEvents();

      /// Unsubscribe from events
      /// 
      /// @example
      /// Events.RemoveListener
      /// <MoveResolvedEvent>
      ///     (OnMoveResolved);
      ///     or
      ///     EventManager.OnSetRule -= OnSetRule;
      void UnsubscribeEvents();
    }
}