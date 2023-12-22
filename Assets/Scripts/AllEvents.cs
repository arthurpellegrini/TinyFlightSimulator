using EventManager;

#region GameManager Events

public class GameMainMenuEvent : Event
{
}

public class GameCreditsEvent : Event
{
}

public class GameSettingsEvent : Event
{
}

public class GameChoosePlaneEvent : Event
{
}

public class GameResumeEvent : Event
{
}

public class GamePausedEvent : Event
{
}

public class GameErrorEvent : Event
{
    public string eErrorTitle { get; set; }
    public string eErrorDescription { get; set; }
}

public class GameStatisticsChangedEvent : Event
{
    public float eTimer { get; set; }
    public int eFps { get; set; }
    public int eSpeed { get; set; }
    public int eAltitude { get; set; }
}

#endregion

#region MenuManager Events

public class MainMenuButtonClickedEvent : Event
{
}

public class CreditsButtonClickedEvent : Event
{
}

public class SettingsButtonClickedEvent : Event
{
}

public class ChoosePlaneButtonClickedEvent : Event
{
}

public class PreviousPlaneButtonClickedEvent : Event
{
}

public class NextPlaneButtonClickedEvent : Event
{
}

public class ResumeButtonClickedEvent : Event
{
}

public class EscapeButtonClickedEvent : Event
{
}

public class SwitchCameraButtonClickedEvent : Event
{
}

public class QuitButtonClickedEvent : Event
{
}

#endregion


#region GameEvent

// public class EnemyHasBeenHitEvent : Event
// {
// 	public GameObject eEnemyGO;
// }

#endregion