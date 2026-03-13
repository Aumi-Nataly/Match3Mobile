using UnityEngine;

public class FallTile : IFallTile
{
    public Tile[,] FallDownTile(Tile[,] grid, int wight, int height)
    {
        Tile[,] res = new Tile[wight, height];

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

                            t.transform.localPosition = new Vector3(x, y, 0);
                            t.GridPos = new Vector2(x, y);
                            break;
                        }
                    }
                }

                res[x, y] = grid[x, y];
            }
        }

        return res;
    }
}
