using UnityEngine;

public class GroundCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] _tiles;
    [SerializeField] private float _seasonsChangeSpeed = 4f;
    private Region[] _regions;
    private int[,] _regionsIndicesMap;
    private bool[] _regionChanged;
    private int _changedRegionsCount = 0;
    private float[] _regionsCurrentValues;
    private float[] _regionsTargetValues;
    private Color32[] _colors;
    private Texture2D _seasonsTexture;
    private bool _groundIsCreated = false;

    public void CreateGround(Vector2Int worldSize, RegionData[] regionDatas)
    {
        _regionsIndicesMap = new int[worldSize.x, worldSize.y];
        _regions = new Region[regionDatas.Length];
        for (int i = 0; i < _regions.Length; ++i)
        {
            _regions[i] = new Region(regionDatas[i]);
        }
        for (int i = 0; i < worldSize.x; ++i)
        {
            for (int j = 0; j < worldSize.y; ++j)
            {
                _regionsIndicesMap[i, j] = -1;
            }
        }
        _regionsCurrentValues = new float[_regions.Length];
        _regionsTargetValues = new float[_regions.Length];
        _regionChanged = new bool[_regions.Length];
        _colors = new Color32[GameConstraints.MaxWorldSizeX * GameConstraints.MaxWorldSizeY];
        _seasonsTexture = new Texture2D(GameConstraints.MaxWorldSizeX, GameConstraints.MaxWorldSizeY);
        _seasonsTexture.filterMode = FilterMode.Point;

        for (int i = 0; i < _regions.Length; ++i)
        {
            _regionsTargetValues[i] = _regions[i].CurrentSeason * 0.25f;
            _regionsCurrentValues[i] = _regionsTargetValues[i];
            Color regionColor = new Color(_regionsTargetValues[i], 0f, 0f, 1f);
            int xStart = _regions[i].StartPosition.x;
            int yStart = _regions[i].StartPosition.y;
            int xEnd = _regions[i].EndPosition.x;
            int yEnd = _regions[i].EndPosition.y;
            for (int j = xStart; j <= xEnd; ++j)
            {
                for (int k = yStart; k <= yEnd; ++k)
                {
                    int tileType = 0;
                    if (k < yEnd) tileType += 1;
                    if (j < xEnd) tileType += 2;
                    if (k > yStart) tileType += 4;
                    if (j > xStart) tileType += 8;

                    GameObject tile = Instantiate(_tiles[tileType], transform);
                    tile.transform.localPosition = new Vector3(j + 0.5f, 0f, k + 0.5f);
                    _colors[GameConstraints.MaxWorldSizeX * k + j] = regionColor;
                    _regionsIndicesMap[j, k] = i;
                }
            }
        }
        _RefreshSeasonsTexture();
        _groundIsCreated = true;
    }

    void Update()
    {
        if (_groundIsCreated) _UpdateRegionsValue();
    }

    public void Clear()
    {
        if (_groundIsCreated)
        {
            _regions = null;
            _regionsIndicesMap = null;
            _regionChanged = null;
            _regionsCurrentValues = null;
            _regionsTargetValues = null;
            _groundIsCreated = false;
            _changedRegionsCount = 0;
            Destroyer.DestroyChildren(transform);
        }
    }

    private void _RefreshSeasonsTexture()
    {
        _seasonsTexture.SetPixels32(_colors);
        _seasonsTexture.Apply();
        Shader.SetGlobalTexture("_Seasons", _seasonsTexture);
    }

    public int GetRegionIndex(int x, int y)
    {
        if (x > -1 && x < _regionsIndicesMap.GetLength(0) && y > -1 && y < _regionsIndicesMap.GetLength(1))
        {
            return _regionsIndicesMap[x, y];
        }
        else return -1;
    }

    public int GetSeason(int x, int y)
    {
        int region = GetRegionIndex(x, y);
        if (region > -1)
        {
            return _regions[region].CurrentSeason;
        }
        else return -1;
    }

    public void MakeStep(int x, int y, out Vector2Int startPosition, out Vector2Int endPosition, out int currentSeason)
    {
        int index = _regionsIndicesMap[x, y];
        if (index > -1)
        {
            _regions[index].ChangeSeason();
            float newTargetValue = _regions[index].CurrentSeason * 0.25f;
            if (newTargetValue < _regionsTargetValues[index]) newTargetValue += 1f;
            _regionsTargetValues[index] = newTargetValue;
            if (!_regionChanged[index])
            {
                _regionChanged[index] = true;
                _changedRegionsCount++;
            }
            currentSeason = _regions[index].CurrentSeason;
            startPosition = _regions[index].StartPosition;
            endPosition = _regions[index].EndPosition;
        }
        else
        {
            currentSeason = -1;
            startPosition = Vector2Int.zero;
            endPosition = Vector2Int.zero;
        }
    }

    private void _UpdateRegionsValue()
    {
        if (_changedRegionsCount > 0)
        {
            for (int i = 0; i < _regions.Length; ++i)
            {
                if (_regionChanged[i])
                {
                    _regionsCurrentValues[i] += _seasonsChangeSpeed * 0.25f * Time.deltaTime;
                    if (_regionsCurrentValues[i] >= _regionsTargetValues[i])
                    {
                        _regionsCurrentValues[i] = _regionsTargetValues[i];
                        _regionChanged[i] = false;
                        _changedRegionsCount--;
                    }
                    else
                    {
                        if (_regionsCurrentValues[i] >= 1f)
                        {
                            _regionsCurrentValues[i] -= 1f;
                            _regionsTargetValues[i] -= 1f;
                        }
                    }
                    Color regionColor = new Color(_regionsCurrentValues[i], 0f, 0f, 1f);
                    for (int j = _regions[i].StartPosition.x; j <= _regions[i].EndPosition.x; ++j)
                    {
                        for (int k = _regions[i].StartPosition.y; k <= _regions[i].EndPosition.y; ++k)
                        {
                            _colors[GameConstraints.MaxWorldSizeX * k + j] = regionColor;
                        }
                    }
                }
            }
            _RefreshSeasonsTexture();
        }
    }
}