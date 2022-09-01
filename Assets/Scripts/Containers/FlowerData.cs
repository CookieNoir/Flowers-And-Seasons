using System;

[Serializable]
public struct FlowerData
{
    public int flowerModelId;
    public int startNumberOfSeeds;
    public int requiredNumberOfFlowers;

    public void Validate()
    {
        if (flowerModelId < 0) flowerModelId = 0;
        if (startNumberOfSeeds < 0) startNumberOfSeeds = 0;
        if (requiredNumberOfFlowers < 0) requiredNumberOfFlowers = 0;
    }
}