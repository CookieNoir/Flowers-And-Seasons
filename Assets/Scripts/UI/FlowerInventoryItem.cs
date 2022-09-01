using System;
using UnityEngine;
using UnityEngine.UI;

public class FlowerInventoryItem : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Text _requiredNumberOfFlowers;
    [SerializeField] private Text _currentNumberOfFlowers;
    [SerializeField] private Text _numberOfSeeds;
    [SerializeField] private Text _keyCode;
    [Space(10)]
    [SerializeField] private MaskableGraphic _textBackground;
    [SerializeField] private MaskableGraphic _keyBackground;
    [SerializeField] private MaskableGraphic _iconBackground;
    [SerializeField] private MaskableGraphic _requiredBar;
    [SerializeField] private MaskableGraphic _requiredWord;
    [Space(10)]
    [SerializeField] private GameObject _springFlag;
    [SerializeField] private GameObject _summerFlag;
    [SerializeField] private GameObject _autumnFlag;
    [SerializeField] private GameObject _winterFlag;
    [Space(10)]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _successColor;
    private Color _requiredBarColor;
    private Action<int> _plantFlowerAction;
    private int _usedFlowerId;
    public void PlantFlower()
    {
        _plantFlowerAction(_usedFlowerId);
    }

    public void SetValues(int usedFlowerId, Action<int> plantFlowerAction, Sprite icon, int requiredNumberOfFlowers, int currentNumberOfFlowers, int numberOfSeeds, string key, bool growsInSpring, bool growsInSummer, bool growsInAutumn, bool growsInWinter, Color backgroundColor)
    {
        _usedFlowerId = usedFlowerId;
        _plantFlowerAction = plantFlowerAction;
        _icon.sprite = icon;
        _requiredNumberOfFlowers.text = requiredNumberOfFlowers.ToString();
        _currentNumberOfFlowers.text = currentNumberOfFlowers.ToString();
        _numberOfSeeds.text = numberOfSeeds.ToString();
        _keyCode.text = key;

        _springFlag.SetActive(growsInSpring);
        _summerFlag.SetActive(growsInSummer);
        _autumnFlag.SetActive(growsInAutumn);
        _winterFlag.SetActive(growsInWinter);

        _textBackground.color = backgroundColor;
        _keyBackground.color = backgroundColor;
        _iconBackground.color = backgroundColor;

        _requiredBarColor = _requiredBar.color;
        _requiredBarColor.a = Mathf.Clamp(currentNumberOfFlowers * 1f / requiredNumberOfFlowers, 0f, 1f);
        _requiredBar.color = _requiredBarColor;
        if (currentNumberOfFlowers >= requiredNumberOfFlowers)
        {
            _PaintRequiredFields(_successColor);
        }
        else
        {
            _PaintRequiredFields(_defaultColor);
        }
    }

    public void SetCurrentNumberOfFlowers(int requiredNumberOfFlowers, int currentNumberOfFlowers)
    {
        _currentNumberOfFlowers.text = currentNumberOfFlowers.ToString();
        _requiredBarColor.a = Mathf.Clamp(currentNumberOfFlowers * 1f / requiredNumberOfFlowers, 0f, 1f);
        _requiredBar.color = _requiredBarColor;
        if (currentNumberOfFlowers >= requiredNumberOfFlowers)
        {
            _PaintRequiredFields(_successColor);
        }
        else
        {
            _PaintRequiredFields(_defaultColor);
        }
    }

    public void SetNumberOfSeeds(int numberOfSeeds)
    {
        _numberOfSeeds.text = numberOfSeeds.ToString();
    }

    private void _PaintRequiredFields(Color color)
    {
        _requiredNumberOfFlowers.color = color;
        _requiredWord.color = color;
    }
}