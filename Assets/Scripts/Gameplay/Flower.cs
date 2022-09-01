using System;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private Transform _scalableGroup;
    private int _numberOfSeeds;
    private bool _growsInSpring;
    private bool _growsInSummer;
    private bool _growsInAutumn;
    private bool _growsInWinter;
    private float _scale;
    [HideInInspector] public int Id;
    public bool CanBePicked { get; private set; }

    public int GetSeeds()
    {
        return _numberOfSeeds;
    }

    public void Initialize(int numberOfSeeds, bool growsInSpring, bool growsInSummer, bool growsInAutumn, bool growsInWinter)
    {
        CanBePicked = false;
        _scale = 0f;
        _ApplyScale();
        _numberOfSeeds = numberOfSeeds;
        _growsInSpring = growsInSpring;
        _growsInSummer = growsInSummer;
        _growsInAutumn = growsInAutumn;
        _growsInWinter = growsInWinter;
    }

    public void SetSeason(int value)
    {
        switch (value)
        {
            case 0:
                {
                    CanBePicked = _growsInSpring;
                    break;
                }
            case 1:
                {
                    CanBePicked = _growsInSummer;
                    break;
                }
            case 2:
                {
                    CanBePicked = _growsInAutumn;
                    break;
                }
            case 3:
            default:
                {
                    CanBePicked = _growsInWinter;
                    break;
                }
        }
    }

    private void _ApplyScale()
    {
        Vector3 scale = new Vector3(_scale, _scale, _scale);
        for (int i = 0; i < _scalableGroup.childCount; ++i)
        {
            _scalableGroup.GetChild(i).localScale = scale;
        }
    }

    void Update()
    {
        if (CanBePicked)
        {
            if (_scale < 1f)
            {
                _scale += Time.deltaTime;
                if (_scale > 1f) _scale = 1f;
                _ApplyScale();
            }
        }
        else
        {
            if (_scale > 0f)
            {
                _scale -= Time.deltaTime;
                if (_scale < 0f) _scale = 0f;
                _ApplyScale();
            }
        }
    }
}