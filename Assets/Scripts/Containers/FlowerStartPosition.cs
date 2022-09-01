using System;
using UnityEngine;

[Serializable]
public struct FlowerStartPosition
{
    public int flowerDataId;
    public Vector2Int position;

    public void Validate(Vector2Int worldSize, int maxId)
    {
        flowerDataId = Math.Clamp(flowerDataId, 0, maxId);
        position.x = Math.Clamp(position.x, 0, worldSize.x - 1);
        position.y = Math.Clamp(position.y, 0, worldSize.y - 1);
    }
}