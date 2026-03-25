using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
public class GridSystem : MonoBehaviour
{

    [SerializeField]
    private int HeightSpawnNew = 2;

    private float CellSize = 1f;

    private int Width = 8;
    private int Height = 8;
    private Tile[,] _grid;

    private TileSpriteManager _tileSpriteManager;
    private Pool _pool;
    private IMatchFinder _matchFinder;
    private IFallTile _fallTile;
    private SwipeDetection _swipeDetection; 
    private AudioManager _audio;

    public event Action<MatchType> OnScoreAdded;
    public event Action<List<Tile>> OnDeletedTile;
    private bool StartingCombination = true;
    private bool _isProcessing;

    [Inject]
    public void Construct(TileSpriteManager tileSpriteManager, Pool pool,
        IMatchFinder matchFinder, IFallTile fallTile, SwipeDetection swipeDetection
        ,AudioManager audio)
    {
        _tileSpriteManager = tileSpriteManager;
        _pool = pool;
        _matchFinder = matchFinder;
        _fallTile = fallTile;
        _swipeDetection = swipeDetection;
        _audio = audio;

        Debug.Log($"GridSystem - TileSpriteManager = {tileSpriteManager != null}");
        Debug.Log($"GridSystem - Pool = {pool != null}");
        Debug.Log($"GridSystem - MatchFinder = {matchFinder != null}");
        Debug.Log($"GridSystem - FallTile = {fallTile != null}");
        Debug.Log($"GridSystem - SwipeDetection = {swipeDetection != null}");
        Debug.Log($"GridSystem - AudioManager = {audio != null}");
    }

    private void OnDestroy()
    {
        _swipeDetection.OnSwipe -= HandleSwipe;
    }

    private void Start()
    {
        Width = UnityEngine.Random.Range(4, 7);

        _pool.ReloadPool();

         SetupCamera();
        _swipeDetection.OnSwipe += HandleSwipe;
        SeeStartGrid();
    }

    public void Mix()
    {
        Debug.Log("Mix");

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                 _pool.ReturnToPool(_grid[x, y]);
                _grid[x, y] = null;
               
            }
        }

        SeeStartGrid();
    }

    private void GenerateGrid()
    {
        _grid = new Tile[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                CreateTile(x, y);
            }
        }
    }

    public Dictionary<TileType, Sprite> GetDictionarySprite()
    {
        return _tileSpriteManager.GetDictionaryType();
    }

    private void CreateTile(int x, int y)
    {
        Tile tile = _pool.GetFromPool();

        if (tile == null)
            return;

        tile.transform.SetParent(transform);
        tile.transform.localPosition = GetWorldPosition(x, y);

        TileType randomType = (TileType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(TileType)).Length);
        tile.SetType(randomType, _tileSpriteManager);

        _grid[x, y] = tile;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        float offsetX = (Width - 1) / 2f;
        float offsetY = (Height - 1) / 2f;

        return new Vector3((x - offsetX) * CellSize, (y - offsetY) * CellSize, 0);
    }

    private void RemoveTile(List<Tile> tiles)
    {
        foreach (Tile tile in tiles)
        {
            Vector2Int pos = FindTilePosition(tile);

            if (pos.x < 0)
                return;

            _grid[pos.x, pos.y] = null;
            _pool.ReturnToPool(tile);
        }
    }

    private IEnumerator ProcessMatches(bool startingCombination)
    {
        _isProcessing = true;
        int step = 0;

        while (step < 100) // Защита от бесконечного цикла
        {
            // 1. Найти комбинации
            var foundList = _matchFinder.FindMatches(_grid, Width, Height);

            // Если комбинаций нет — завершаем обработку
            if (foundList.Count == 0)
            {
                Debug.Log($"Обработка завершена на шаге {step}. Больше комбинаций нет.");
                break;
            }


            // 2. Удалить комбинации
            foreach (var found in foundList)
            {
                RemoveTile(found.ListTile);
                _audio.PlayRemoveSound();

                if (!startingCombination)
                { 
                    OnScoreAdded?.Invoke(found.MatchType);
                    OnDeletedTile?.Invoke(found.ListTile);
                }
            }

            yield return new WaitForSeconds(0.3f);

            // 3. Сдвинуть вниз на пустые места
            _fallTile.FallDownTile(_grid, Width, Height, CellSize);

            // Ждём завершения анимации падения
            yield return new WaitForSeconds(0.5f);

            // 4. Создать новые ячейки на пустом месте
            SpawnNewTile();

            // Ждём завершения анимации создания новых тайлов
            yield return new WaitForSeconds(2f);

            step++;

        }
        _isProcessing = false;
    }


   /// <summary>
   /// Запуск корутин генерации ячеек на все столбцы
   /// </summary>
    private void SpawnNewTile()
    {
        for (int x = 0; x < Width; x++)
        {
            StartCoroutine(SpawnColumnSequentially(x));
        }
    }

    /// <summary>
    /// Генерация ячеек по очереди и их перемещение вниз
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private IEnumerator SpawnColumnSequentially(int x)
    {
        for (int y = 0; y < Height; y++)
        {
            if (_grid[x, y] == null)
            {      
                yield return StartCoroutine(SpawnTileFromTop(x, y));
            }
        }
    }

    private void SeeStartGrid()
    {
        GenerateGrid();
        StartCoroutine(ProcessMatches(StartingCombination));
        StartingCombination = false;
    }

    private void HandleSwipe(SwipeModel swipeModel)
    {
        if (_isProcessing) //защита от пользовательского ввода
            return;

        Vector2Int poscurrent = FindTilePosition(swipeModel.Tile);
        Vector2Int targetPos = poscurrent + new Vector2Int((int)swipeModel.Vect2.x, (int)swipeModel.Vect2.y);

        if (!IsInsideGrid(targetPos))
            return;

        Tile other = _grid[targetPos.x, targetPos.y];
        SwapTiles(swipeModel.Tile, other);

        var foundList = _matchFinder.FindMatches(_grid, Width, Height);

        if (foundList.Count > 0)
        {
            StartCoroutine(ProcessMatches(StartingCombination));
        }
        else
        {
            // Нет совпадения — вернуть обратно
            SwapTiles(swipeModel.Tile, other);
        }
    }

    private bool IsInsideGrid(Vector2 pos)
    {
        return pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;
    }

    private void SwapTiles(Tile a, Tile b)
    {
        Vector2Int posA = FindTilePosition(a);
        Vector2Int posB = FindTilePosition(b);

        _grid[posA.x, posA.y] = b;
        _grid[posB.x, posB.y] = a;

        a.MoveTo(GetWorldPosition(posB.x, posB.y));
        b.MoveTo(GetWorldPosition(posA.x, posA.y));

    }

    private IEnumerator SpawnTileFromTop(int x, int y)
    {
        Tile tile = _pool.GetFromPool();

        if (tile == null)
            yield return new WaitForSeconds(0.1f); 

        tile.transform.SetParent(transform, worldPositionStays: true);
        tile.transform.localPosition = GetWorldPosition(x, Height);

        TileType randomType = (TileType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(TileType)).Length);
        tile.SetType(randomType, _tileSpriteManager);
    
        tile.MoveTo(GetWorldPosition(x, y));
         _grid[x, y] = tile;

        yield return new WaitForSeconds(0.8f);
    }

    private Vector2Int FindTilePosition(Tile tile)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (_grid[x, y] == tile)
                {
                    return new Vector2Int(x, y);
                }

            }
        }

        return new Vector2Int(-1, -1);
    }


    private void SetupCamera()
    {
        //1. От ширины столбцов расчитать размер камеры
        float gridWidth = Width * CellSize;
        float aspect = (float)Screen.width / Screen.height;

        Camera.main.orthographicSize = gridWidth / (2f * aspect);
        Canvas.ForceUpdateCanvases();

        //2. Расчитать колво рядов
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float UiPercent = 80f;
        float availableHeight = cameraHeight * UiPercent / 100;
       
        Height = Mathf.FloorToInt(availableHeight / CellSize);
        Debug.Log($"SetupCamera Height ={Height}");

    }

    public void GotToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

