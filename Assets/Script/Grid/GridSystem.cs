using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

public class GridSystem : MonoBehaviour
{
    [SerializeField]
    private int Width = 8;
    [SerializeField]
    private int Height = 8;
    [SerializeField]
    private float CellSize = 1f;
    [SerializeField]
    private int HeightSpawnNew = 2;


    private Tile[,] _grid;
    private TileSpriteManager _tileSpriteManager;
    private Pool _pool;
    private IMatchFinder _matchFinder;
    private IFallTile _fallTile;
    private SwipeDetection _swipeDetection;

    [Inject]
    public void Construct(TileSpriteManager tileSpriteManager, Pool pool,
        IMatchFinder matchFinder, IFallTile fallTile, SwipeDetection swipeDetection)
    {
        _tileSpriteManager = tileSpriteManager;
        _pool = pool;
        _matchFinder = matchFinder;
        _fallTile = fallTile;
        _swipeDetection = swipeDetection;

        Debug.Log($"GridSystem - TileSpriteManager = {tileSpriteManager != null}");
        Debug.Log($"GridSystem - Pool = {pool != null}");
        Debug.Log($"GridSystem - MatchFinder = {matchFinder != null}");
        Debug.Log($"GridSystem - FallTile = {fallTile != null}");
        Debug.Log($"GridSystem - SwipeDetection = {swipeDetection != null}");
    }

    private void OnDestroy()
    {
        _swipeDetection.OnSwipe -= HandleSwipe;
    }

    private void Start()
    {
        _swipeDetection.OnSwipe += HandleSwipe;
        StartCoroutine(SeeStartGrid());
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

    private void CreateTile(int x, int y)
    {
        Tile tile = _pool.GetFromPool();

        if (tile == null)
            return;

        tile.transform.SetParent(transform, worldPositionStays: true);
        tile.transform.localPosition = new Vector3(x * CellSize, y * CellSize, 0);

        TileType randomType = (TileType)Random.Range(0, System.Enum.GetValues(typeof(TileType)).Length);
        tile.SetType(randomType, _tileSpriteManager);

        _grid[x, y] = tile;

    }

    private void RemoveTile(Tile tile)
    {
        Vector2Int pos = FindTilePosition(tile);

        if (pos.x < 0)
            return;

        _grid[pos.x, pos.y] = null;
        _pool.ReturnToPool(tile);
    }

    private void ProcessMatches()
    {
        int step = 0;
        while (true && step < 20)
        {

            //1. Найти комбинации
            var foundList = _matchFinder.FindMatches(_grid, Width, Height);

            if (foundList.Count == 0)
                break;

            //2. Удалить комбинации
            foreach (var found in foundList)
            {
                RemoveTile(found);
            }

            //3. Сдвинуть вниз на пустые места
            _fallTile.FallDownTile(_grid, Width, Height, CellSize);

            //4. Создать новые ячейки на пустом месте
            SpawnNewTile();

            step++;
            Debug.Log("step = " + step);
        }
    }


    private void SpawnNewTile()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (_grid[x, y] == null)
                {
                    SpawnTileFromTop(x,y);
                }
            }
        }
    }

    private IEnumerator SeeStartGrid()
    {
        GenerateGrid();
        yield return new WaitForSeconds(1f);
        ProcessMatches();
    }

    private void HandleSwipe(SwipeModel swipeModel)
    {
        Vector2Int poscurrent = FindTilePosition(swipeModel.Tile);
        Vector2Int targetPos = poscurrent + new Vector2Int((int)swipeModel.Vect2.x, (int)swipeModel.Vect2.y);

        if (!IsInsideGrid(targetPos))
            return;

        Tile other = _grid[targetPos.x, targetPos.y];
        SwapTiles(swipeModel.Tile, other);

        var foundList = _matchFinder.FindMatches(_grid, Width, Height);

        if (foundList.Count > 0)
        {
            ProcessMatches();
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

        a.MoveTo(new Vector3(posB.x * CellSize, posB.y * CellSize, 0));
        b.MoveTo(new Vector3(posA.x * CellSize, posA.y * CellSize, 0));
    }

    private void SpawnTileFromTop(int x, int y)
    {
        Tile tile = _pool.GetFromPool();

        if (tile == null)
            return;

        tile.transform.SetParent(transform, worldPositionStays: true);
        tile.transform.localPosition = new Vector3(x * CellSize, (y * CellSize) + HeightSpawnNew, 0);

        TileType randomType = (TileType)Random.Range(0, System.Enum.GetValues(typeof(TileType)).Length);
        tile.SetType(randomType, _tileSpriteManager);

        _grid[x, y] = tile;
        tile.MoveTo(new Vector3(x * CellSize, y * CellSize, 0));
    }

    private void LogGridState(string message)
    {
        Debug.Log(message);
        for (int y = Height - 1; y >= 0; y--) // Сверху вниз для удобства чтения
        {
            string row = "";
            for (int x = 0; x < Width; x++)
            {
                row += (_grid[x, y] == null ? "□ " : "■ ");
            }
            Debug.Log($"Row {y}: {row}");
        }
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

}

