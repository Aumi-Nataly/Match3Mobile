using System.Collections.Generic;

public class MatchFinder : IMatchFinder
{
    public List<Tile> FindMatches(Tile[,] grid, int wight, int height)
    { 
        List<Tile> matches = new List<Tile>();

        //горизонтальные
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < wight - 2; x++)
            { 
                Tile a = grid[x, y];
                Tile b = grid[x + 1 , y];
                Tile c = grid[x + 2, y];

                if (a.Type == b.Type && b.Type == c.Type)
                {
                    matches.Add(a);
                    matches.Add(b);
                    matches.Add(c);
                }
            }
        }


        //вертикальные
        for (int x = 0; x < wight; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                Tile a = grid[x, y];
                Tile b = grid[x, y+ 1];
                Tile c = grid[x, y+ 2];

                if (a.Type == b.Type && b.Type == c.Type)
                {
                    matches.Add(a);
                    matches.Add(b);
                    matches.Add(c);
                }
            }
        }


        return matches;
    }
}
