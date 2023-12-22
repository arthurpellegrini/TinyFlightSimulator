using System.Collections;
using Common;
using TMPro;
using UnityEngine;

public class HudManager : Manager<HudManager>
{
    #region Manager implementation

    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }

    #endregion

    private void RefreshGameUI(int fps, int speed, int altitude)
    {
        _fps.text = "FPS:" + fps;
        _speed.text = speed + "kts";
        _altimeter.text = altitude + "ft";
    }

    #region Callbacks to GameManager events

    protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
    {
        RefreshGameUI(e.eFps, e.eSpeed, e.eAltitude);
    }

    #endregion

    [Header("HudManager")]

    #region Labels & Values

    [Header("Game")]
    [SerializeField] private TMP_Text _fps;

    [Space(10)] [Header("Plane")] 
    [SerializeField] private TMP_Text _speed;
    [SerializeField] private TMP_Text _altimeter;

    #endregion
}