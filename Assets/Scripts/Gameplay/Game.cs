using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private FollowingCamera _followingCamera;
    [SerializeField] private GroundCreator _groundCreator;
    [SerializeField] private Grid _grid;
    [Space(10)]
    [SerializeField] private DestinationPoint _destinationPoint;
    [SerializeField] private MouseDownAction _pickFlowerButton;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private HideableText _movesUI;
    [SerializeField] private Signs _signs;
    [SerializeField] private InteractiveWindowsHandler _interactiveWindowsHandler;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private EnvironmentChanger _environmentChanger;
    [Space(10)]
    [SerializeField] private GameObject[] _obstaclePrefabs;
    [SerializeField] private FlowerModel[] _flowerModels;
    [SerializeField] private LevelData[] _levelDatas;
    [SerializeField] private bool _usePlayerData;
    [SerializeField] private int _firstLevelId;
    private int _currentLevelId;
    private Inventory _inventory;
    private Vector2Int _worldSize;
    private Flower[,] _flowersMap;
    private FlowerModel[] _usedFlowerModels;
    private FlowerData[] _flowerDatas;
    private bool _isLimited;
    private int[] _requiredNumberOfFlowers;
    private int[] _currentNumberOfFlowers;
    private Vector2Int _currentPlayerPosition;
    private int _movesLeft;
    private bool _levelIsBuilt = false;
    private bool _canInteract = true;
    private bool _canMakeMove = true;
    private int _maxLevel;

    private void Start()
    {
        _maxLevel = PlayerPrefs.GetInt("Max Level", 0);
        _interactiveWindowsHandler.SetValues(EnableInteraction, DisableInteraction);
        _interactiveWindowsHandler.CreateLevelButtons(_levelDatas, _maxLevel, StartLevelWithId);
        _pickFlowerButton.SetAction(PickFlower);
        _destinationPoint.SetAction(Move);
        _playerMovement.OnMove += _OnPlayerMovement;
        _playerMovement.OnDestinationReached += _UpdateSignsVisibility;
        _loadingScreen.OnEndLevel += _LoadLevel;

        _LoadLevel(_usePlayerData? _maxLevel : _firstLevelId, true);
    }

    private void _BuildLevel(int levelId, bool newLevel)
    {
        _currentLevelId = levelId;
        LevelData levelData = _levelDatas[levelId];
        _worldSize = levelData.WorldSize;
        _groundCreator.CreateGround(_worldSize, levelData.Regions);
        bool[,] _obstaclesMap = new bool[_worldSize.x, _worldSize.y];
        if (levelData.ObstaclesStartPositions != null)
        {
            for (int i = 0; i < levelData.ObstaclesStartPositions.Length; ++i)
            {
                GameObject obstacle = Instantiate(_obstaclePrefabs[levelData.ObstaclesStartPositions[i].obstaclePrefabId], transform);
                int x = levelData.ObstaclesStartPositions[i].position.x;
                int y = levelData.ObstaclesStartPositions[i].position.y;
                obstacle.transform.localPosition = new Vector3(x + 0.5f, 0f, y + 0.5f);
                obstacle.transform.localRotation = Quaternion.Euler(0f, levelData.ObstaclesStartPositions[i].rotation, 0f);
                _obstaclesMap[x, y] = true;
            }
        }
        _currentPlayerPosition = _playerMovement.SetStartPositionAndNavigationMap(levelData.PlayerStartPosition, _obstaclesMap);
        _followingCamera.SetTarget(_playerMovement.transform);
        _usedFlowerModels = new FlowerModel[levelData.FlowerDatas.Length];
        for (int i = 0; i < _usedFlowerModels.Length; ++i)
        {
            _usedFlowerModels[i] = _flowerModels[levelData.FlowerDatas[i].flowerModelId];
        }
        _flowerDatas = new FlowerData[levelData.FlowerDatas.Length];
        for (int i = 0; i < _flowerDatas.Length; ++i)
        {
            _flowerDatas[i] = levelData.FlowerDatas[i];
        }
        _flowersMap = new Flower[_worldSize.x, _worldSize.y];
        _requiredNumberOfFlowers = new int[levelData.FlowerDatas.Length];
        _currentNumberOfFlowers = new int[levelData.FlowerDatas.Length];
        if (levelData.FlowerStartPositions != null)
        {
            for (int i = 0; i < levelData.FlowerStartPositions.Length; ++i)
            {
                _PlantFlower(levelData.FlowerStartPositions[i].flowerDataId, levelData.FlowerStartPositions[i].position, true);
            }
        }
        int[] numberOfSeeds = new int[levelData.FlowerDatas.Length];
        for (int i = 0; i < levelData.FlowerDatas.Length; ++i)
        {
            _requiredNumberOfFlowers[i] = levelData.FlowerDatas[i].requiredNumberOfFlowers;
            numberOfSeeds[i] = levelData.FlowerDatas[i].startNumberOfSeeds;
        }
        _inventoryUI.CreateInventoryHUD(_usedFlowerModels, levelData.FlowerDatas, _currentNumberOfFlowers, PlantFlower);
        _inventory = new Inventory(numberOfSeeds, _inventoryUI);
        _isLimited = levelData.IsLimited;
        _movesLeft = levelData.NumberOfMoves;
        if (_isLimited)
        {
            _movesUI.Show();
            _movesUI.SetText(_movesLeft.ToString());
        }
        else
        {
            _movesUI.Hide();
        }
        _grid.CreateGrid(levelData.WorldSize);
        _signs.SetDefaultValues();
        if (newLevel && levelData.EntryWindow)
        {
            _interactiveWindowsHandler.CreateEntryWindow(levelData.EntryWindow);
            DisableInteraction();
        }
        else
        {
            _interactiveWindowsHandler.HideAndShowMenuButton();
            EnableInteraction();
        }
        _environmentChanger.ChangeEnvironment(levelData.EnvironmentData);
        _canMakeMove = true;
        _levelIsBuilt = true;
        _loadingScreen.Fade();
    }

    private void _DropLevel()
    {
        if (_levelIsBuilt)
        {
            _groundCreator.Clear();
            _flowersMap = null;
            _usedFlowerModels = null;
            _flowerDatas = null;
            _requiredNumberOfFlowers = null;
            _currentNumberOfFlowers = null;
            _inventoryUI.Clear();
            _grid.Clear();
            Destroyer.DestroyChildren(transform);
            _interactiveWindowsHandler.ResetWindows();
            _levelIsBuilt = false;
        }
    }

    public void StartNextLevel()
    {
        _loadingScreen.LoadLevel((_currentLevelId + 1) % _levelDatas.Length, true);
    }

    public void StartLevelWithId(int levelId)
    {
        if (levelId > -1 && levelId < _levelDatas.Length)
        {
            _loadingScreen.LoadLevel(levelId, levelId != _currentLevelId);
        }
    }

    private void _LoadLevel(int levelId, bool newLevel)
    {
        _DropLevel();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        _BuildLevel(levelId, newLevel);
    }

    private int _flowersForWin()
    {
        int result = 0;
        for (int i = 0; i < _usedFlowerModels.Length; ++i)
        {
            if (_currentNumberOfFlowers[i] < _flowerDatas[i].requiredNumberOfFlowers)
            {
                result += _flowerDatas[i].requiredNumberOfFlowers - _currentNumberOfFlowers[i];
            }
        }
        return result;
    }

    public void RestartLevel()
    {
        if (_levelIsBuilt) _loadingScreen.LoadLevel(_currentLevelId, false);
    }

    public void Move(MovementDirections direction)
    {
        if (_levelIsBuilt && _playerMovement.IsDestinationReached && _canInteract && _canMakeMove)
        {
            if (_playerMovement.Move(direction))
            {
                if (_isLimited)
                {
                    _movesLeft--;
                    _movesUI.SetText(_movesLeft.ToString());
                    if (_movesLeft == 0)
                    {
                        _canMakeMove = false;
                        bool lost = _flowersForWin() > 1 || !_inventory.WinIsPossible(_requiredNumberOfFlowers, _currentNumberOfFlowers);
                        if (_flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y])
                        {
                            Flower flower = _flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y];
                            lost |= !(flower.CanBePicked && _currentNumberOfFlowers[flower.Id] - 1 >= _requiredNumberOfFlowers[flower.Id]);
                        }
                        if (lost) _OnLose();
                    }
                }
                _UpdateSignsVisibility();
            }
        }
    }

    public void PickFlower()
    {
        if (_levelIsBuilt && _playerMovement.IsDestinationReached && _canInteract && _flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y] &&
            _flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y].CanBePicked)
        {
            Flower flower = _flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y];
            _inventory.AddSeeds(flower.Id, flower.GetSeeds());
            _flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y] = null;
            _currentNumberOfFlowers[flower.Id]--;
            _inventoryUI.SetFlowersAmount(_requiredNumberOfFlowers, _currentNumberOfFlowers);
            Destroy(flower.gameObject);
            _UpdateSignsVisibility();
        }
    }

    public void PlantFlower(int usedFlowerId)
    {
        if (_levelIsBuilt && _playerMovement.IsDestinationReached && _canInteract && !_flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y] && _groundCreator.GetRegionIndex(_currentPlayerPosition.x, _currentPlayerPosition.y) > -1)
        {
            _PlantFlower(usedFlowerId, _currentPlayerPosition);
            if (_isLimited)
            {
                if (_flowersForWin() == 0) _OnWin();
                else
                {
                    if (!_canMakeMove) _OnLose();
                }
            }
            _UpdateSignsVisibility();
        }
    }

    private void _UpdateSignsVisibility()
    {
        if (_playerMovement.IsDestinationReached && _canInteract)
        {
            if (_flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y])
            {
                _signs.SetSigns(_flowersMap[_currentPlayerPosition.x, _currentPlayerPosition.y].CanBePicked, false);
            }
            else
            {
                _signs.SetSigns(false, _groundCreator.GetRegionIndex(_currentPlayerPosition.x, _currentPlayerPosition.y) > -1 && !_inventory.IsEmpty());
            }
        }
        else
        {
            _signs.SetSigns(false, false);
        }
    }

    private void Update()
    {
        if (_levelIsBuilt)
        {
            if (Input.GetKeyDown(KeyCode.U)) RestartLevel();
            else if (Input.GetKeyDown(KeyCode.W)) Move(MovementDirections.Up);
            else if (Input.GetKeyDown(KeyCode.S)) Move(MovementDirections.Down);
            else if (Input.GetKeyDown(KeyCode.A)) Move(MovementDirections.Left);
            else if (Input.GetKeyDown(KeyCode.D)) Move(MovementDirections.Right);
            else if (Input.GetKeyDown(KeyCode.F)) PickFlower();
            else
            {
                int i = 0;
                while (i < _usedFlowerModels.Length && !Input.GetKeyDown(KeyCode.Alpha1 + i)) ++i;
                if (i < _usedFlowerModels.Length) PlantFlower(i);
            }
        }      
    }

    public void EnableInteraction()
    {
        _canInteract = true;
        _playerMovement.SetMovementButtonsVisibility();
        _UpdateSignsVisibility();
    }

    public void DisableInteraction()
    {
        _canInteract = false;
        _destinationPoint.HideButtons();
        _signs.SetSigns(false, false);
    }

    private void _OnWin()
    {
        DisableInteraction();
        if (_currentLevelId == _maxLevel)
        {
            _maxLevel++;
            PlayerPrefs.SetInt("Max Level", _maxLevel);
            _interactiveWindowsHandler.UpdateLevelButtons(_maxLevel);
        }       
        _interactiveWindowsHandler.TurnOnEndWindow(true);
    }

    private void _OnLose()
    {
        DisableInteraction();
        _interactiveWindowsHandler.TurnOnEndWindow(false);
    }

    private void _OnPlayerMovement(int x, int y)
    {
        _currentPlayerPosition = new Vector2Int(x, y);
        _groundCreator.MakeStep(x, y, out Vector2Int startPosition, out Vector2Int endPosition, out int currentSeason);
        if (currentSeason > -1)
        {
            for (int i = startPosition.x; i <= endPosition.x; ++i)
            {
                for (int j = startPosition.y; j <= endPosition.y; ++j)
                {
                    if (_flowersMap[i, j]) _flowersMap[i, j].SetSeason(currentSeason);
                }
            }
        }
    }

    private void _PlantFlower(int usedFlowerId, Vector2Int position, bool fromStart = false)
    {
        bool hasSeed = fromStart || _inventory.PlantSeed(usedFlowerId);
        if (hasSeed)
        {
            int id = _flowerDatas[usedFlowerId].flowerModelId;
            GameObject newFlower = Instantiate(_flowerModels[id].flowerPrefab, transform);
            int x = position.x;
            int y = position.y;
            newFlower.transform.position = new Vector3(x + 0.5f, 0f, y + 0.5f);
            _flowersMap[x, y] = newFlower.GetComponent<Flower>();
            _flowersMap[x, y].Id = usedFlowerId;
            _flowersMap[x, y].Initialize(
                _flowerModels[id].numberOfSeeds,
                _flowerModels[id].growsInSpring,
                _flowerModels[id].growsInSummer,
                _flowerModels[id].growsInAutumn,
                _flowerModels[id].growsInWinter);
            _currentNumberOfFlowers[usedFlowerId]++;
            if (!fromStart) _inventoryUI.SetFlowersAmount(_requiredNumberOfFlowers, _currentNumberOfFlowers);
            else
            {
                int season = _groundCreator.GetSeason(x, y);
                if (season > -1) _flowersMap[x, y].SetSeason(season);
            }
        }
    }

    private void OnValidate()
    {
        if (_levelDatas != null && _levelDatas.Length > 0) _firstLevelId = Math.Clamp(_firstLevelId, 0, _levelDatas.Length - 1);
    }
}