public class Inventory
{
    private int _totalSeedsQuantity;
    private int[] _seedsQuantity;
    private InventoryUI _inventoryUI;

    public Inventory(int[] seedsQuantity, InventoryUI inventoryUI)
    {
        _totalSeedsQuantity = 0;
        _seedsQuantity = new int[seedsQuantity.Length];
        _inventoryUI = inventoryUI;
        for (int i = 0; i < _seedsQuantity.Length; ++i)
        {
            _seedsQuantity[i] = seedsQuantity[i];
            _totalSeedsQuantity += seedsQuantity[i];
        }
    }

    public bool PlantSeed(int id)
    {
        if (id < 0 || _seedsQuantity == null || id >= _seedsQuantity.Length) return false;
        if (_seedsQuantity[id] > 0)
        {
            _seedsQuantity[id]--;
            _totalSeedsQuantity--;
            _inventoryUI.SetSeedsAmount(_seedsQuantity);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool WinIsPossible(int[] requiredFlowersAmount, int[] currentFlowersAmount)
    {
        bool result = true;
        for (int i = 0; i < _seedsQuantity.Length; ++i)
        {
            if (currentFlowersAmount[i] + _seedsQuantity[i] < requiredFlowersAmount[i])
            {
                result = false;
                break;
            }
        }
        return result;
    }

    public bool IsEmpty()
    {
        return _totalSeedsQuantity <= 0;
    }

    public void AddSeeds(int id, int quantity)
    {
        if (id < 0 || _seedsQuantity == null || id >= _seedsQuantity.Length || quantity < 0) return;
        _seedsQuantity[id] += quantity;
        _totalSeedsQuantity += quantity;
        _inventoryUI.SetSeedsAmount(_seedsQuantity);
    }
}