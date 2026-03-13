using System.Collections.Generic;

public interface IMatchFinder
{
    List<Tile> FindMatches(Tile[,] grid, int wight, int height);
}
