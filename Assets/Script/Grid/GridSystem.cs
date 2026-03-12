using UnityEngine;
using VContainer;

public class GridSystem : MonoBehaviour
{
    [SerializeField] 
    private int Width = 8;
    [SerializeField] 
    private int Height = 8;

    [SerializeField] 
    private Tile TilePrefab;
    [SerializeField] 
    private float CellSize = 100f;

    private TileSpriteManager _tileSpriteManager;
    private Pool  _pool;

    [Inject]
    public void Construct(TileSpriteManager tileSpriteManager, Pool pool)
    {
        _tileSpriteManager = tileSpriteManager;
        _pool = pool;

        Debug.Log($"GridSystem - TileSpriteManager = {tileSpriteManager!=null}");
        Debug.Log($"GridSystem - Pool = {pool != null}");
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                CreateTile((float)x * CellSize, (float)y * CellSize);
            }
        }
    }

    private void CreateTile(float x, float y)
    {
        Tile tile = _pool.GetFromPool();



        tile.transform.localPosition = new Vector3(x,y,0);
        tile.GridPos = new Vector2(x, y);
        TileType randomType = (TileType)Random.Range(0, System.Enum.GetValues(typeof(TileType)).Length);
        tile.SetType(randomType, _tileSpriteManager);
      



        //Tile tile = Instantiate(TilePrefab, new Vector3(x, y, 0), Quaternion.identity);
        //tile.transform.localPosition = new Vector3(x, y, 0);
        //tile.GridPos = new Vector2(x, y);

        //TileType randomType = (TileType)Random.Range(0, System.Enum.GetValues(typeof(TileType)).Length);
        //tile.SetType(randomType, _tileSpriteManager);

    }
}

