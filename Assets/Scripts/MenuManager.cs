using System.Collections;
using System.Collections.Generic;
using Common;
using TMPro;
using UnityEngine;

public class MenuManager : Manager<MenuManager>
{
    [Header("MenuManager")] [SerializeField]
    private GameObject mainMenuGo;

    [SerializeField] private GameObject creditsMenuGo;
    [SerializeField] private GameObject pausedMenuGo;
    [SerializeField] private GameObject settingsMenuGo;
    [SerializeField] private GameObject choosePlaneMenuGo;
    [SerializeField] private GameObject gameErrorMenuGo;
    [SerializeField] private GameObject hudGo;

    // [Header("MainMenu")]
    // [SerializeField] private Button _tmpPlayButton;
    // [SerializeField] private Button _tmpSettingsButton;


    [Header("ErrorMenu")] [SerializeField] private TMP_Text _tmpErrorTitle;

    [SerializeField] private TMP_Text _tmpErrorDescription;
    private List<GameObject> allPanels;

    #region Manager implementation

    protected override IEnumerator InitCoroutine()
    {
        yield break;
    }

    #endregion

    #region Monobehaviour lifecycle

    protected override void Awake()
    {
        base.Awake();
        RegisterPanels();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel")) EscapeButtonHasBeenClicked();
    }

    #endregion

    #region Panel Methods

    private void RegisterPanels()
    {
        allPanels = new List<GameObject>
        {
            mainMenuGo,
            choosePlaneMenuGo,
            settingsMenuGo,
            creditsMenuGo,
            pausedMenuGo,
            gameErrorMenuGo,
            hudGo
        };
    }

    private void OpenPanel(GameObject panel)
    {
        foreach (var item in allPanels)
            if (item)
                item.SetActive(item == panel);
    }

    #endregion

    #region UI OnClick Events

    public void MainMenuButtonHasBeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }

    public void ChoosePlaneButtonHasbeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new ChoosePlaneButtonClickedEvent());
    }
    
    public void PreviousPlaneButtonHasbeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new PreviousPlaneButtonClickedEvent());
    }   
    
    public void NextPlaneButtonHasbeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new NextPlaneButtonClickedEvent());
    }

    public void SettingsButtonHasBeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new SettingsButtonClickedEvent());
    }

    public void CreditsButtonHasBeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new CreditsButtonClickedEvent());
    }

    public void ResumeButtonHasBeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new ResumeButtonClickedEvent());
    }

    public void EscapeButtonHasBeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new EscapeButtonClickedEvent());
    }

    public void QuitButtonHasBeenClicked()
    {
        EventManager.EventManager.Instance.Raise(new QuitButtonClickedEvent());
    }

    #endregion

    #region Callbacks to GameManager events

    protected override void GameMainMenu(GameMainMenuEvent e)
    {
        OpenPanel(mainMenuGo);
    }

    protected override void GameChoosePlane(GameChoosePlaneEvent e)
    {
        OpenPanel(choosePlaneMenuGo);
    }

    protected override void GameSettings(GameSettingsEvent e)
    {
        OpenPanel(settingsMenuGo);
    }

    protected override void GameCredits(GameCreditsEvent e)
    {
        OpenPanel(creditsMenuGo);
    }

    protected override void GameResume(GameResumeEvent e)
    {
        OpenPanel(hudGo);
    }

    protected override void GamePaused(GamePausedEvent e)
    {
        OpenPanel(pausedMenuGo);
    }

    protected override void GameError(GameErrorEvent e)
    {
        _tmpErrorTitle.text = e.eErrorTitle;
        _tmpErrorDescription.text = e.eErrorDescription;
        OpenPanel(gameErrorMenuGo);
    }

    #endregion
}