using System.Collections;
using UnityEngine;
using VContainer;

public class GridSystem : MonoBehaviour
{
    [SerializeField] 
    private int Width = 8;
    [SerializeField] 
    private int Height = 8;
    [SerializeField] 
    private float CellSize = 1f;


    private Tile[,] _grid;
    private TileSpriteManager _tileSpriteManager;
    private Pool  _pool;
    private IMatchFinder _matchFinder;
    private IFallTile _fallTile;

    [Inject]
    public void Construct(TileSpriteManager tileSpriteManager, Pool pool, IMatchFinder matchFinder, IFallTile fallTile)
    {
        _tileSpriteManager = tileSpriteManager;
        _pool = pool;
        _matchFinder = matchFinder;
        _fallTile = fallTile;

        Debug.Log($"GridSystem - TileSpriteManager = {tileSpriteManager!=null}");
        Debug.Log($"GridSystem - Pool = {pool != null}");
        Debug.Log($"GridSystem - MatchFinder = {matchFinder != null}");
        Debug.Log($"GridSystem - FallTile = {fallTile != null}");
    }

    private void Start()
    {
        //GenerateGrid();
        //ProcessMatches();

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
        tile.GridPos = new Vector2(x * CellSize, y * CellSize);

        TileType randomType = (TileType)Random.Range(0, System.Enum.GetValues(typeof(TileType)).Length);
        tile.SetType(randomType, _tileSpriteManager);

        _grid[x, y] = tile;
    }

    private void RemoveTile(Tile tile)
    {
        Vector2 pos = tile.GridPos;
        _grid[(int)pos.x, (int)pos.y]=null;
        _pool.ReturnToPool(tile);
    }

    private void ProcessMatches()
    {
        //1. Найти комбинации
        var foundList = _matchFinder.FindMatches(_grid, Width, Height);
        Debug.Log("Matches found:" + foundList.Count);

        //2. Удалить комбинации
        foreach (var found in foundList)
        {
            RemoveTile(found);
        }

        //3. Сдвинуть вниз на пустые места
        _grid = _fallTile.FallDownTile(_grid, Width, Height);
    }


    private IEnumerator SeeStartGrid()
    {
        GenerateGrid();
        yield return new WaitForSeconds(3f);
        ProcessMatches();
    }
}

