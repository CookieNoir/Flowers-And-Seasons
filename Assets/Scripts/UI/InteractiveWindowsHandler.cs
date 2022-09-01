using System;
using UnityEngine;

public class InteractiveWindowsHandler : FadeableWindow
{
    [SerializeField] private GameObject _winWindow;
    [SerializeField] private GameObject _loseWindow;
    [SerializeField] private GameObject _selectLevelWindow;
    [SerializeField] private LevelButtonsHandler _levelButtonsHandler;
    [SerializeField] private FadeableWindow _menuButton;
    private GameObject _entryWindow;
    private GameObject _prevWindow;
    private Action _enableInteraction;
    private Action _disableInteraction;

    public void SetValues(Action enableInteraction, Action disableInteraction)
    {
        _enableInteraction = enableInteraction;
        _disableInteraction = disableInteraction;
        _menuButton.Hide();
        ResetWindows();
    }

    public void CreateLevelButtons(LevelData[] levelDatas, int maxLevel, Action<int> loadLevelAction)
    {
        _levelButtonsHandler.CreateLevelButtons(levelDatas, maxLevel, loadLevelAction);
    }

    public void UpdateLevelButtons(int maxLevel)
    {
        _levelButtonsHandler.UpdateLevelButtons(maxLevel);
    }

    public void TurnOnEndWindow(bool result)
    {
        _HideAllWindows();
        _prevWindow = result ? _winWindow : _loseWindow;
        _prevWindow.SetActive(true);
        Appear();
        _menuButton.Fade();
    }

    public void CreateEntryWindow(GameObject entryWindow)
    {
        _entryWindow = Instantiate(entryWindow, transform);
        _entryWindow.GetComponent<EntryWindow>().OnClose = Close;
        Show();
    }

    public void HideAndShowMenuButton()
    {
        Hide();
        _menuButton.Appear();
    }

    public void TurnOnSelectLevelWindow()
    {
        _disableInteraction();
        _HideAllWindows();
        _selectLevelWindow.SetActive(true);
        Appear();
        _menuButton.Fade();
    }

    public void ResetWindows()
    {
        _HideAllWindows();
        if (_entryWindow) Destroy(_entryWindow);
        _prevWindow = null;
    }

    public void Back()
    {
        if (_prevWindow)
        {
            _selectLevelWindow.SetActive(false);
            _prevWindow.SetActive(true);
        }
        else
        {
            _enableInteraction();
            Fade();
            _menuButton.Appear();
        }
    }

    public void Close()
    {
        _enableInteraction();
        Fade();
        _menuButton.Appear();
    }

    private void _HideAllWindows()
    {
        if (_entryWindow) _entryWindow.SetActive(false);
        _winWindow.SetActive(false);
        _loseWindow.SetActive(false);
        _selectLevelWindow.SetActive(false);
    }
}