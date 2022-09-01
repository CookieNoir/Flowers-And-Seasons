using System;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Color[] _backgroundColors;
    private FlowerInventoryItem[] _items;

    public void CreateInventoryHUD(FlowerModel[] flowers, FlowerData[] flowerDatas, int[] currentNumberOfFlowers, Action<int> plantFlowerAction)
    {
        _items = new FlowerInventoryItem[flowers.Length];
        for (int i = 0; i < flowers.Length; ++i)
        {
            GameObject newItem = Instantiate(_itemPrefab, transform);
            _items[i] = newItem.GetComponent<FlowerInventoryItem>();
            _items[i].SetValues(i, plantFlowerAction, flowers[i].icon, flowerDatas[i].requiredNumberOfFlowers, currentNumberOfFlowers[i], flowerDatas[i].startNumberOfSeeds, (i + 1).ToString(),
                flowers[i].growsInSpring, flowers[i].growsInSummer, flowers[i].growsInAutumn, flowers[i].growsInWinter, _backgroundColors[i % _backgroundColors.Length]);
        }
    }

    public void SetFlowersAmount(int[] requiredNumberOfFlowers, int[] currentNumberOfFlowers)
    {
        for (int i = 0; i < _items.Length; ++i)
        {
            _items[i].SetCurrentNumberOfFlowers(requiredNumberOfFlowers[i], currentNumberOfFlowers[i]);
        }
    }

    public void SetSeedsAmount(int[] numberOfSeeds)
    {
        for (int i = 0; i < numberOfSeeds.Length; ++i)
        {
            _items[i].SetNumberOfSeeds(numberOfSeeds[i]);
        }
    }

    public void Clear()
    {
        Destroyer.DestroyChildren(transform);
    }
}