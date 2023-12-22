using System.Collections;
using Common;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Manager<GameManager>
{
    [Header("Plane Selection")]
    [SerializeField] private GameObject[] planes;
    [SerializeField] private int selectedPlane = 0;
    
    [FormerlySerializedAs("ThirdPersonCameraGo")]
    [FormerlySerializedAs("mainCameraGo")]
    [Header("Camera and Flight Instances")]
    [SerializeField] private GameObject thirdPersonCameraGo;
    [SerializeField] private GameObject[] firstPersonCameraGo;
    [SerializeField] private GameObject globalPlaneGo;
    [SerializeField] private GameObject mouseFlightRigGo;
    [SerializeField] private MFlight.Demo.Plane plane;
    private Rigidbody planeRb;
    
    
    private GameState _gameState = GameState.Playing;
    public bool IsPlaying => _gameState == GameState.Playing;
    
    private int _fps;
    private readonly float[] _frameDeltaTimeArray = new float[30];
    private int _lastFrameIndex;

    private int _speed;
    private int _attitude;
    
    #region Manager implementation

    private void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }
    
    private void DisablePlaneInput()
    {
        globalPlaneGo.GetComponent<MonoBehaviour>().enabled = false;
        mouseFlightRigGo.SetActive(false);
    }

    private void EnablePlaneInput()
    {
        globalPlaneGo.GetComponent<MonoBehaviour>().enabled = true;
        mouseFlightRigGo.SetActive(true);
    }

    private void ResetCamera()
    {
        Camera thirdPersonCamera = thirdPersonCameraGo.GetComponent<Camera>();
        AudioListener thirdPersonAudioListener = thirdPersonCameraGo.GetComponent<AudioListener>();
            
        Camera firstPersonCamera = firstPersonCameraGo[selectedPlane].GetComponent<Camera>();
        AudioListener firstPersonAudioListener = firstPersonCameraGo[selectedPlane].GetComponent<AudioListener>();
        
        firstPersonCamera.enabled = false;
        firstPersonAudioListener.enabled = false;
        thirdPersonCamera.enabled = true;
        thirdPersonAudioListener.enabled = true;
    }

    protected override IEnumerator InitCoroutine()
    {
        EventManager.EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
        yield break;
    }
    
    protected void Update()
    {
        if (IsPlaying) SetGameStats();
    }

    #endregion

    #region Events' subscription

    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
        EventManager.EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.EventManager.Instance.AddListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
        EventManager.EventManager.Instance.AddListener<SettingsButtonClickedEvent>(SettingsButtonClicked);
        EventManager.EventManager.Instance.AddListener<ChoosePlaneButtonClickedEvent>(ChoosePlaneButtonClicked);
        EventManager.EventManager.Instance.AddListener<PreviousPlaneButtonClickedEvent>(PreviousPlaneButtonClicked);
        EventManager.EventManager.Instance.AddListener<NextPlaneButtonClickedEvent>(NextPlaneButtonClicked);
        EventManager.EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.EventManager.Instance.AddListener<SwitchCameraButtonClickedEvent>(SwitchCameraButtonClicked);
        EventManager.EventManager.Instance.AddListener<QuitButtonClickedEvent>(QuitButtonClicked);
    }

    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
        EventManager.EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<CreditsButtonClickedEvent>(CreditsButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<SettingsButtonClickedEvent>(SettingsButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<ChoosePlaneButtonClickedEvent>(ChoosePlaneButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<PreviousPlaneButtonClickedEvent>(PreviousPlaneButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<NextPlaneButtonClickedEvent>(NextPlaneButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<SwitchCameraButtonClickedEvent>(SwitchCameraButtonClicked);
        EventManager.EventManager.Instance.RemoveListener<QuitButtonClickedEvent>(QuitButtonClicked);
    }

    #endregion

    #region GameStatistics

    private int SetFrameRate()
    {
        _frameDeltaTimeArray[_lastFrameIndex] = Time.deltaTime;
        _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;
        var total = 0f;
        foreach (var deltaTime in _frameDeltaTimeArray) total += deltaTime;
        return Mathf.RoundToInt(_frameDeltaTimeArray.Length / total);
    }

    private void SetGameStats()
    {
        EventManager.EventManager.Instance.Raise(new GameStatisticsChangedEvent
        {
            eFps = SetFrameRate(), 
            eSpeed = Mathf.RoundToInt(planeRb.velocity.magnitude * 1.9444f), 
            eAltitude = Mathf.RoundToInt(globalPlaneGo.transform.position.y * 3.281f)
        });
    }

    #endregion

    #region GameManager Functions

    private void GameMainMenu()
    {
        if (_gameState != GameState.Menu) MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
        _gameState = GameState.Menu;
        ResetCamera();
        DisablePlaneInput();
        SetTimeScale(0);
        EventManager.EventManager.Instance.Raise(new GameMainMenuEvent());
    }

    private void GameSettings()
    {
        if (_gameState != GameState.Menu) MusicLoopsManager.Instance.PlayMusic(Constants.MENU_MUSIC);
        _gameState = GameState.Menu;
        SetTimeScale(0);
        EventManager.EventManager.Instance.Raise(new GameSettingsEvent());
    }

    private void GameCredits()
    {
        _gameState = GameState.Menu;
        SetTimeScale(0);
        EventManager.EventManager.Instance.Raise(new GameCreditsEvent());
    }

    private void GameChoosePlane()
    {
        _gameState = GameState.Menu;
        
        // Reset Plane Movement
        plane = globalPlaneGo.GetComponent<MFlight.Demo.Plane>();
        plane.ResetPitchYawRoll();
        planeRb = globalPlaneGo.GetComponent<Rigidbody>();
        planeRb.isKinematic = true;
        SetTimeScale(0);
        
        // Set Camera Position
        ResetCamera();
        
        thirdPersonCameraGo.transform.position = new Vector3(-370, 124, 168.5f);
        thirdPersonCameraGo.transform.localEulerAngles = new Vector3(-4, -85, 0);
        
        // Set Plane Position
        globalPlaneGo.transform.position = new Vector3(-385, 125, 170);
        globalPlaneGo.transform.localEulerAngles = new Vector3(0, -225, 0);
        planes[selectedPlane].SetActive(true);

        SetTimeScale(1);
        EventManager.EventManager.Instance.Raise(new GameChoosePlaneEvent());
    }

    private void GameResume()
    {
        _gameState = GameState.Playing;
        EnablePlaneInput();
        planeRb.isKinematic = false;
        SetTimeScale(1);
        EventManager.EventManager.Instance.Raise(new GameResumeEvent());
    }

    private void GamePaused()
    {
        _gameState = GameState.Paused;
        DisablePlaneInput();
        SetTimeScale(0);
        EventManager.EventManager.Instance.Raise(new GamePausedEvent());
    }

    #endregion

    #region Callbacks to Events issued by MenuManager

    private void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
    {
        GameMainMenu();
    }

    private void SettingsButtonClicked(SettingsButtonClickedEvent e)
    {
        GameSettings();
    }

    private void CreditsButtonClicked(CreditsButtonClickedEvent e)
    {
        GameCredits();
    }

    private void ChoosePlaneButtonClicked(ChoosePlaneButtonClickedEvent e)
    {
        GameChoosePlane();
    }
    
    private void PreviousPlaneButtonClicked(PreviousPlaneButtonClickedEvent e)
    {
        planes[selectedPlane].SetActive(false);
        selectedPlane--;
        if (selectedPlane < 0)
        {
            selectedPlane += planes.Length;
        }
        planes[selectedPlane].SetActive(true);
    }
    
    private void NextPlaneButtonClicked(NextPlaneButtonClickedEvent e)
    {
        planes[selectedPlane].SetActive(false);
        selectedPlane = (selectedPlane + 1) % planes.Length;
        planes[selectedPlane].SetActive(true);
    }

    private void ResumeButtonClicked(ResumeButtonClickedEvent e)
    {
        GameResume();
    }

    private void SwitchCameraButtonClicked(SwitchCameraButtonClickedEvent e)
    {
        if (IsPlaying)
        {
            Camera thirdPersonCamera = thirdPersonCameraGo.GetComponent<Camera>();
            AudioListener thirdPersonAudioListener = thirdPersonCameraGo.GetComponent<AudioListener>();
            
            Camera firstPersonCamera = firstPersonCameraGo[selectedPlane].GetComponent<Camera>();
            AudioListener firstPersonAudioListener = firstPersonCameraGo[selectedPlane].GetComponent<AudioListener>();
            
            firstPersonCamera.enabled = !firstPersonCamera.enabled;
            thirdPersonCamera.enabled = !thirdPersonCamera.enabled;
            
            firstPersonAudioListener.enabled = !firstPersonAudioListener.enabled;
            thirdPersonAudioListener.enabled = !thirdPersonAudioListener.enabled;
        }
    }

    private void EscapeButtonClicked(EscapeButtonClickedEvent e)
    {
        if (IsPlaying) GamePaused();
        else GameResume();
    }

    private void QuitButtonClicked(QuitButtonClickedEvent e)
    {
        Application.Quit();
    }

    #endregion
}