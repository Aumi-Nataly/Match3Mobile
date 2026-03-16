using System.Collections.Generic;

public interface IMatchFinder
{
    List<MatchModel> FindMatches(Tile[,] grid, int wight, int height);
}
