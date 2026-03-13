using UnityEngine;

public interface IFallTile
{
    public Tile[,] FallDownTile(Tile[,] grid, int wight, int height);
}
