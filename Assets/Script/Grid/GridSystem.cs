using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] 
    private int Width = 8;
    [SerializeField] 
    private int Height = 8;

    [SerializeField] 
    private Tile TilePrefab;

    private Tile[,] Grid;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Grid = new Tile[Width, Height];

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
        Tile tile = Instantiate(TilePrefab, transform);

        tile.transform.localPosition = new Vector3(x, y, 0);
        tile.GridPos = new Vector2Int(x, y);
        Grid[x, y] = tile;
    }
}

