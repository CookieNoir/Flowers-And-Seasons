using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Text _levelName;
    [SerializeField] private Image _levelIcon;
    [SerializeField] private MaskableGraphic _background;
    private Action<int> _loadLevelAction;
    private int _levelId;
    private bool _buttonIsSet = false;
    private bool _interactable = false;

    public void SetValues(int levelId, string levelName, Sprite levelIcon, Action<int> loadLevelAction, Color backgroundColor)
    {
        _levelId = levelId;
        _levelName.text = levelName;
        _levelIcon.sprite = levelIcon;
        _loadLevelAction = loadLevelAction;
        _background.color = backgroundColor;
        _buttonIsSet = true;
    }

    public void SetInteractable(bool interactable, Color color)
    {
        _interactable = interactable;
        _levelIcon.color = color;
    }

    public void LoadLevel()
    {
        if (_buttonIsSet && _interactable) _loadLevelAction(_levelId);
    }
}