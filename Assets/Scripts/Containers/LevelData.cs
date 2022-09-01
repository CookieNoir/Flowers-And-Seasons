using System;
using UnityEngine;
[CreateAssetMenu(fileName = "Level Data", menuName = "ScriptableObjects/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private Sprite _levelIcon;
    public Sprite LevelIcon
    {
        get => _levelIcon;
        private set => _levelIcon = value;
    }
    [SerializeField] private bool _showOnlyWhenReached;
    public bool ShowOnlyWhenReached
    {
        get => _showOnlyWhenReached;
        private set => _showOnlyWhenReached = value;
    }
    [SerializeField] private Vector2Int _worldSize;
    public Vector2Int WorldSize
    {
        get => _worldSize;
        private set => _worldSize = value;
    }
    [SerializeField] private Vector2Int _playerStartPosition;
    public Vector2Int PlayerStartPosition
    {
        get => _playerStartPosition;
        private set => _playerStartPosition = value;
    }
    [SerializeField] private RegionData[] _regions;
    public RegionData[] Regions
    {
        get => _regions;
        private set => _regions = value;
    }
    [SerializeField] private ObstacleStartPosition[] _obstaclesStartPositions;
    public ObstacleStartPosition[] ObstaclesStartPositions
    {
        get => _obstaclesStartPositions;
        private set => _obstaclesStartPositions = value;
    }
    [SerializeField] private FlowerData[] _flowerDatas;
    public FlowerData[] FlowerDatas
    {
        get => _flowerDatas;
        private set => _flowerDatas = value;
    }
    [SerializeField] private FlowerStartPosition[] _flowerStartPositions;
    public FlowerStartPosition[] FlowerStartPositions
    {
        get => _flowerStartPositions;
        private set => _flowerStartPositions = value;
    }
    [SerializeField] private bool _isLimited;
    public bool IsLimited
    {
        get => _isLimited;
        private set => _isLimited = value;
    }
    [SerializeField] private int _numberOfMoves;
    public int NumberOfMoves
    {
        get => _numberOfMoves;
        private set => _numberOfMoves = value;
    }
    [SerializeField] private GameObject _entryWindow;
    public GameObject EntryWindow
    {
        get => _entryWindow;
        private set => _entryWindow = value;
    }
    [SerializeField] private EnvironmentData _environmentData;
    public EnvironmentData EnvironmentData
    {
        get => _environmentData;
        private set => _environmentData = value;
    }

    private void OnValidate()
    {
        _worldSize.x = Math.Clamp(_worldSize.x, 2, GameConstraints.MaxWorldSizeX);
        _worldSize.y = Math.Clamp(_worldSize.y, 2, GameConstraints.MaxWorldSizeY);
        _playerStartPosition.x = Math.Clamp(_playerStartPosition.x, 0, _worldSize.x - 1);
        _playerStartPosition.y = Math.Clamp(_playerStartPosition.y, 0, _worldSize.y - 1);
        int i = 0;
        if (_regions != null) for (; i < _regions.Length; ++i)
            {
                _regions[i].Validate(_worldSize);
            }
        if (_obstaclesStartPositions != null) for (i = 0; i < _obstaclesStartPositions.Length; ++i)
            {
                _obstaclesStartPositions[i].Validate(_worldSize);
            }
        if (_flowerDatas != null)
        {
            for (i = 0; i < _flowerDatas.Length; ++i)
            {
                _flowerDatas[i].Validate();
            }
            if (_flowerStartPositions != null) for (i = 0; i < _flowerStartPositions.Length; ++i)
                {
                    _flowerStartPositions[i].Validate(_worldSize, _flowerDatas.Length);
                }
        }
        if (_numberOfMoves < 0) _numberOfMoves = 0;
    }
}