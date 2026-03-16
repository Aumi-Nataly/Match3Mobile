using System.Collections.Generic;

public class MatchFinder : IMatchFinder
{
    public List<MatchModel> FindMatches(Tile[,] grid, int wight, int height)
    { 
        List<MatchModel> matchModels = new List<MatchModel>();

        FindHorizontal(grid, wight, height, matchModels);
        FindVertical(grid, wight, height, matchModels);

        return matchModels;
    }


    private void FindHorizontal(Tile[,] grid, int width, int height, List<MatchModel> matchModels)
    {
        Stack<Tile> stack = new Stack<Tile>();

        for (int y = 0; y < height; y++)
        {
            stack.Clear();

            for (int x = 0; x < width - 1; x++)
            {
                Tile a = grid[x, y];
                Tile b = grid[x + 1, y];

                if (a.Type == b.Type)
                {
                    if (stack.Count == 0 || stack.Peek() != a)
                    {
                        stack.Push(a);
                    }

                    stack.Push(b);
                }
                else
                { 
                    if(stack.Count >= 3)
                    {
                        AddMatch(stack, matchModels);
                        stack.Clear();
                    }
                }
            }

            if (stack.Count >= 3)
            {
                AddMatch(stack, matchModels);
            }
        }
    }

    private void FindVertical(Tile[,] grid, int width, int height, List<MatchModel> matchModels)
    {
        Stack<Tile> stack = new Stack<Tile>();

        for (int x = 0; x < width; x++)
        {
            stack.Clear();

            for (int y = 0; y < height - 1; y++)
            {
                Tile a = grid[x, y];
                Tile b = grid[x, y + 1];

                if (a.Type == b.Type)
                {
                    if (stack.Count == 0 || stack.Peek() != a)
                    {
                        stack.Push(a);
                    }

                    stack.Push(b);
                }
                else
                {
                    if (stack.Count >= 3)
                    {
                        AddMatch(stack, matchModels);
                        stack.Clear();
                    }
                }
            }

            if (stack.Count >= 3)
            {
                AddMatch(stack, matchModels);
            }
        }
    }


    private void AddMatch(Stack<Tile> stack, List<MatchModel> matchModels)
    {
        List<Tile> list = new List<Tile>(stack.ToArray());

        switch (stack.Count) 
        {
            case 3:
                matchModels.Add(new MatchModel {ListTile = list, MatchType = MatchType.Three});
                break;
            case 4:
                matchModels.Add(new MatchModel { ListTile = list, MatchType = MatchType.Four });
                break;
            case 5:
                matchModels.Add(new MatchModel { ListTile = list, MatchType = MatchType.Five });
                break;
        }
    }
}
