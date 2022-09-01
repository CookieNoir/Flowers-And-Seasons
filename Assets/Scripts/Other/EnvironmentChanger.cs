using UnityEngine;

public class EnvironmentChanger : MonoBehaviour
{
    [SerializeField] private Light _light;
    [SerializeField] private Camera _camera;
    [SerializeField] private EnvironmentData _defaultEnvironmentData;

    public void ChangeEnvironment(EnvironmentData environmentData)
    {
        if (environmentData)
        {
            _ApplyEnvironmentData(environmentData);
        }
        else
        {
            _ApplyEnvironmentData(_defaultEnvironmentData);
        }
    }

    private void _ApplyEnvironmentData(EnvironmentData environmentData)
    {
        _light.transform.rotation = Quaternion.Euler(environmentData.sunEulerAngle);
        _light.color = environmentData.sunColor;
        _light.intensity = environmentData.intensity;
        _camera.backgroundColor = environmentData.skyColor;
        RenderSettings.ambientLight = environmentData.ambientColor;
    }
}