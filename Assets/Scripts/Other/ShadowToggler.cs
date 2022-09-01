using UnityEngine;

public class ShadowToggler : MonoBehaviour
{
    [SerializeField] private Light _lightSource;
    private bool value;

    private void Awake()
    {
        value = (_lightSource.shadows == LightShadows.None) ? false : true;
    }

    public void ToggleShadows()
    {
        value = !value;
        _lightSource.shadows = value ? LightShadows.Soft : LightShadows.None;
    }
}