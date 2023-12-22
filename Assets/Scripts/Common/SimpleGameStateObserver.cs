using EventManager;
using UnityEngine;

namespace Common
{
    public abstract class SimpleGameStateObserver : MonoBehaviour, IEventHandler
    {
        protected virtual void Awake()
        {
            SubscribeEvents();
        }

        public virtual void SubscribeEvents()
        {
            EventManager.EventManager.Instance.AddListener<GameMainMenuEvent>(GameMainMenu);
            EventManager.EventManager.Instance.AddListener<GameCreditsEvent>(GameCredits);
            EventManager.EventManager.Instance.AddListener<GameSettingsEvent>(GameSettings);
            EventManager.EventManager.Instance.AddListener<GameChoosePlaneEvent>(GameChoosePlane);
            EventManager.EventManager.Instance.AddListener<GameResumeEvent>(GameResume);
            EventManager.EventManager.Instance.AddListener<GamePausedEvent>(GamePaused);
            EventManager.EventManager.Instance.AddListener<GameErrorEvent>(GameError);
            EventManager.EventManager.Instance.AddListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
        }

        public virtual void UnsubscribeEvents()
        {
            EventManager.EventManager.Instance.RemoveListener<GameMainMenuEvent>(GameMainMenu);
            EventManager.EventManager.Instance.RemoveListener<GameCreditsEvent>(GameCredits);
            EventManager.EventManager.Instance.RemoveListener<GameSettingsEvent>(GameSettings);
            EventManager.EventManager.Instance.RemoveListener<GameChoosePlaneEvent>(GameChoosePlane);
            EventManager.EventManager.Instance.RemoveListener<GameResumeEvent>(GameResume);
            EventManager.EventManager.Instance.RemoveListener<GamePausedEvent>(GamePaused);
            EventManager.EventManager.Instance.RemoveListener<GameErrorEvent>(GameError);
            EventManager.EventManager.Instance.RemoveListener<GameStatisticsChangedEvent>(GameStatisticsChanged);
        }

        protected virtual void GameMainMenu(GameMainMenuEvent e)
        {
        }

        protected virtual void GameCredits(GameCreditsEvent e)
        {
        }

        protected virtual void GameSettings(GameSettingsEvent e)
        {
        }

        protected virtual void GameChoosePlane(GameChoosePlaneEvent e)
        {
        }

        protected virtual void GameResume(GameResumeEvent e)
        {
        }

        protected virtual void GamePaused(GamePausedEvent e)
        {
        }

        protected virtual void GameError(GameErrorEvent e)
        {
        }

        protected virtual void GameStatisticsChanged(GameStatisticsChangedEvent e)
        {
        }
    }
}