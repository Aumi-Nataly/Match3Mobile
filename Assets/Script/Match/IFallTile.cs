using UnityEngine;

public interface IFallTile
{
    public void FallDownTile(Tile[,] grid, int wight, int height, float cellSize);
    public bool HasEmptyTileLinq(Tile[,] grid);
}
