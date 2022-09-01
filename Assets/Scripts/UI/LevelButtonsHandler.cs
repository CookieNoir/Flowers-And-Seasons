using System;
using UnityEngine;

public class LevelButtonsHandler: MonoBehaviour
{
    [SerializeField] private GameObject _levelButtonPrefab;
    [SerializeField] private Color _unlockedColor;
    [SerializeField] private Color _lockedColor;
    [SerializeField] private Color[] _backgroundColors;
    private LevelButton[] _levelButtons;
    private int _maxLevel = 0;
    private bool _buttonsCreated = false;

    public void CreateLevelButtons(LevelData[] levelDatas, int maxLevel, Action<int> loadLevelAction)
    {
        _levelButtons = new LevelButton[levelDatas.Length];
        if (maxLevel >= _levelButtons.Length) maxLevel = _levelButtons.Length - 1;
        for (int i = 0; i <= maxLevel; ++i)
        {
            GameObject newButton = Instantiate(_levelButtonPrefab, transform);
            _levelButtons[i] = newButton.GetComponent<LevelButton>();
            _levelButtons[i].SetValues(i, levelDatas[i].name, levelDatas[i].LevelIcon, loadLevelAction, _backgroundColors[i % _backgroundColors.Length]);
            _levelButtons[i].SetInteractable(true, _unlockedColor);
        }
        for (int i = maxLevel + 1; i < _levelButtons.Length; ++i)
        {
            GameObject newButton = Instantiate(_levelButtonPrefab, transform);
            _levelButtons[i] = newButton.GetComponent<LevelButton>();
            _levelButtons[i].SetValues(i, levelDatas[i].name, levelDatas[i].LevelIcon, loadLevelAction, _backgroundColors[i % _backgroundColors.Length]);
            _levelButtons[i].SetInteractable(false, _lockedColor);
            newButton.SetActive(!levelDatas[i].ShowOnlyWhenReached);
        }
        _maxLevel = maxLevel;
        _buttonsCreated = true;
    }

    public void UpdateLevelButtons(int maxLevel)
    {
        if (_buttonsCreated)
        {
            if (maxLevel >= _levelButtons.Length) maxLevel = _levelButtons.Length - 1;
            for (int i = _maxLevel + 1; i <= maxLevel; ++i)
            {
                _levelButtons[i].gameObject.SetActive(true);
                _levelButtons[i].SetInteractable(true, _unlockedColor);
            }
            _maxLevel = maxLevel;
        }
    }
}