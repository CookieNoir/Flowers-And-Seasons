using UnityEngine;

public class Region
{
    public Vector2Int StartPosition { get; private set; }
    public Vector2Int EndPosition { get; private set; }
    public int CurrentSeason { get; private set; }

    public void ChangeSeason()
    {
        CurrentSeason = (CurrentSeason + 1) % 4;
    }

    public Region(RegionData data)
    {
        StartPosition = data.startPosition;
        EndPosition = data.endPosition;
        CurrentSeason = (int)data.startSeason;
    }
}