using System.Linq;
using UnityEngine;

public class FallTile : IFallTile
{
    
    public void FallDownTile(Tile[,] grid, int wight, int height, float cellSize)
    {

            for (int x = 0; x < wight; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (grid[x, y] == null)
                    {
                        for (int a = y + 1; a < height; a++)
                        {
                            if (grid[x, a] != null)
                            {
                                Tile t = grid[x, a];
                                grid[x, y] = t;
                                grid[x, a] = null;

                                t.MoveTo(new Vector3(x * cellSize, y * cellSize, 0));
                    
                                break;
                            }
                        }
                    }
                }
            }

    }


    public bool HasEmptyTileLinq(Tile[,] grid)
    {
        return grid.Cast<Tile>().Any(tile => tile == null);
    }
}
