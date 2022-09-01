using UnityEngine;

public class Signs : MonoBehaviour
{
    [SerializeField] private FadeableWindow _pressToPickFlower;
    [SerializeField] private FadeableWindow _pressToPlantFlower;

    public void SetSigns(bool canPickFlower, bool canPlantFlower)
    {
        if (canPickFlower)
        {
            _pressToPickFlower.Appear();
        }
        else
        {
            _pressToPickFlower.Fade();
        }
        if (canPlantFlower)
        {
            _pressToPlantFlower.Appear();
        }
        else
        {
            _pressToPlantFlower.Fade();
        }
    }

    public void SetDefaultValues()
    {
        _pressToPickFlower.Hide();
        _pressToPlantFlower.Hide();
    }
}