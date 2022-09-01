using UnityEngine;

public class LevelGizmosDrawer : MonoBehaviour
{
    [SerializeField] private LevelData _levelData;

    private void OnDrawGizmos()
    {
        if (_levelData)
        {
            Gizmos.color = Color.white;
            Vector3 position0 = Vector3.zero;
            Vector3 position1 = new Vector3(_levelData.WorldSize.x, 0f, 0f);
            Vector3 position2 = new Vector3(_levelData.WorldSize.x, 0f, _levelData.WorldSize.y);
            Vector3 position3 = new Vector3(0f, 0f, _levelData.WorldSize.y);
            Gizmos.DrawLine(position0, position1);
            Gizmos.DrawLine(position1, position2);
            Gizmos.DrawLine(position2, position3);
            Gizmos.DrawLine(position3, position0);
            Gizmos.DrawIcon(new Vector3(_levelData.PlayerStartPosition.x + 0.5f, 0f, _levelData.PlayerStartPosition.y + 0.5f), "Player Icon");
            if (_levelData.Regions != null)
            {
                for (int i = 0; i < _levelData.Regions.Length; ++i)
                {
                    position0 = new Vector3(_levelData.Regions[i].startPosition.x + GameConstraints.RegionGizmosOffset, 0f, _levelData.Regions[i].startPosition.y + GameConstraints.RegionGizmosOffset);
                    position1 = new Vector3(_levelData.Regions[i].endPosition.x + 1f - GameConstraints.RegionGizmosOffset, 0f, _levelData.Regions[i].startPosition.y + GameConstraints.RegionGizmosOffset);
                    position2 = new Vector3(_levelData.Regions[i].endPosition.x + 1f - GameConstraints.RegionGizmosOffset, 0f, _levelData.Regions[i].endPosition.y + 1f - GameConstraints.RegionGizmosOffset);
                    position3 = new Vector3(_levelData.Regions[i].startPosition.x + GameConstraints.RegionGizmosOffset, 0f, _levelData.Regions[i].endPosition.y + 1f - GameConstraints.RegionGizmosOffset);
                    switch (_levelData.Regions[i].startSeason)
                    {
                        case Seasons.Spring:
                            {
                                Gizmos.color = Color.green;
                                break;
                            }
                        case Seasons.Summer:
                            {
                                Gizmos.color = Color.yellow;
                                break;
                            }
                        case Seasons.Autumn:
                            {
                                Gizmos.color = Color.red;
                                break;
                            }
                        case Seasons.Winter:
                            {
                                Gizmos.color = Color.cyan;
                                break;
                            }
                    }
                    Gizmos.DrawLine(position0, position1);
                    Gizmos.DrawLine(position1, position2);
                    Gizmos.DrawLine(position2, position3);
                    Gizmos.DrawLine(position3, position0);
                }
            }
            if (_levelData.ObstaclesStartPositions != null)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < _levelData.ObstaclesStartPositions.Length; ++i)
                {
                    position0 = new Vector3(_levelData.ObstaclesStartPositions[i].position.x, 0f, _levelData.ObstaclesStartPositions[i].position.y);
                    position1 = position0 + Vector3.right;
                    position2 = position1 + Vector3.forward;
                    position3 = position0 + Vector3.forward;
                    Gizmos.DrawLine(position0, position1);
                    Gizmos.DrawLine(position1, position2);
                    Gizmos.DrawLine(position2, position3);
                    Gizmos.DrawLine(position3, position0);
                    Gizmos.DrawLine(position1, position3);
                    for (float j = 0.25f; j < 0.875f; j += 0.25f)
                    {
                        Vector3 offsetRight = new Vector3(j, 0f, 0f);
                        Vector3 offsetForward = new Vector3(0f, 0f, j);
                        Gizmos.DrawLine(position0 + offsetRight, position0 + offsetForward);
                        Gizmos.DrawLine(position3 + offsetRight, position1 + offsetForward);
                    }
                }
            }
            if (_levelData.FlowerDatas != null && _levelData.FlowerStartPositions != null)
            {
                for (int i = 0; i < _levelData.FlowerStartPositions.Length; ++i)
                {
                    Gizmos.DrawIcon(new Vector3(_levelData.FlowerStartPositions[i].position.x + 0.5f, 0f, _levelData.FlowerStartPositions[i].position.y + 0.5f), "Flower Icon");
                }
            }
        }
    }
}