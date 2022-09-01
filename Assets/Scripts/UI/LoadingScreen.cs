using System;
using UnityEngine;

public class LoadingScreen : FadeableWindow
{
    public event Action<int, bool> OnEndLevel;
    private int _levelId;
    private bool _levelIsNew;
    private bool _loadingLevel = false;

    public void LoadLevel(int levelId, bool newLevel)
    {
        if (!_loadingLevel)
        {
            _levelId = levelId;
            _levelIsNew = newLevel;
            Appear();
            _loadingLevel = true;
        }
    }

    protected override void OnStopAppearing()
    {     
        OnEndLevel?.Invoke(_levelId, _levelIsNew);
        _loadingLevel = false;
    }
}