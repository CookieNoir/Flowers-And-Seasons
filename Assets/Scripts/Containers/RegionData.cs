using System;
using UnityEngine;
[Serializable]
public struct RegionData
{
    public Vector2Int startPosition;
    public Vector2Int endPosition;
    public Seasons startSeason;

    public void Validate(Vector2Int worldSize)
    {
        startPosition.x = Math.Clamp(startPosition.x, 0, worldSize.x - 1);
        startPosition.y = Math.Clamp(startPosition.y, 0, worldSize.y - 1);
        endPosition.x = Math.Clamp(endPosition.x, startPosition.x, worldSize.x - 1);
        endPosition.y = Math.Clamp(endPosition.y, startPosition.y, worldSize.y - 1);
    }
}