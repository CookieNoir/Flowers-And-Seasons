using System;
using UnityEngine;

[Serializable]
public struct ObstacleStartPosition
{
    public int obstaclePrefabId;
    public Vector2Int position;
    public float rotation;
    
    public void Validate(Vector2Int worldSize)
    {
        position.x = Math.Clamp(position.x, 0, worldSize.x - 1);
        position.y = Math.Clamp(position.y, 0, worldSize.y - 1);
    }
}