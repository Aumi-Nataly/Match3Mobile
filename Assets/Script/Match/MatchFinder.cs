using System.Collections.Generic;
using System.Linq;

public class MatchFinder : IMatchFinder
{
    public List<MatchModel> FindMatches(Tile[,] grid, int width, int height)
    {
        List<MatchModel> matchModels = new List<MatchModel>();

        // 1. Ищем стандартные горизонтальные и вертикальные совпадения
        FindHorizontal(grid, width, height, matchModels);
        FindVertical(grid, width, height, matchModels);

        // 2. Ищем L‑образные и T‑образные формы
        FindLShape(grid, width, height, matchModels);

        // 3. Удаляем дубликаты (один тайл — только в одном совпадении)
        RemoveDuplicateMatches(matchModels);

        return matchModels;
    }

    private void RemoveDuplicateMatches(List<MatchModel> matches)
    {
        HashSet<Tile> usedTiles = new HashSet<Tile>();
        matches.RemoveAll(match =>
        {
            bool hasDuplicate = match.ListTile.Any(tile => usedTiles.Contains(tile));
            if (!hasDuplicate)
            {
                foreach (var tile in match.ListTile)
                {
                    usedTiles.Add(tile);
                }
            }
            return hasDuplicate;
        });
    }


    private void FindLShape(Tile[,] grid, int width, int height, List<MatchModel> matchModels)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile current = grid[x, y];
                if (current == null) continue;

                // направо + вверх
                if (x + 2 < width && y + 2 < height)
                {
                    if (IsSame(grid, x, y, x + 1, y, current) &&
                        IsSame(grid, x, y, x + 2, y, current) &&
                        IsSame(grid, x, y, x, y + 1, current) &&
                        IsSame(grid, x, y, x, y + 2, current))
                    {
                        // AddLMatch(grid, x, y, matchModels);
                        AddLMatch(grid, x, y,1, 0,0,1, matchModels);
                    }
                }

                //  направо + вниз
                if (x + 2 < width && y - 2 >= 0)
                {
                    if (IsSame(grid, x, y, x + 1, y, current) &&
                        IsSame(grid, x, y, x + 2, y, current) &&
                        IsSame(grid, x, y, x, y - 1, current) &&
                        IsSame(grid, x, y, x, y - 2, current))
                    {
                        AddLMatch(grid, x, y, 1, 0, 0, -1, matchModels);
                    }
                }

                //  налево + вверх
                if (x - 2 >= 0 && y + 2 < height)
                {
                    if (IsSame(grid, x, y, x - 1, y, current) &&
                        IsSame(grid, x, y, x - 2, y, current) &&
                        IsSame(grid, x, y, x, y + 1, current) &&
                        IsSame(grid, x, y, x, y + 2, current))
                    {
                        AddLMatch(grid, x, y, -1, 0, 0, 1, matchModels);
                    }
                }

                //  налево + вниз
                if (x - 2 >= 0 && y - 2 >= 0)
                {
                    if (IsSame(grid, x, y, x - 1, y, current) &&
                        IsSame(grid, x, y, x - 2, y, current) &&
                        IsSame(grid, x, y, x, y - 1, current) &&
                        IsSame(grid, x, y, x, y - 2, current))
                    {
                        AddLMatch(grid, x, y, -1, 0, 0, -1, matchModels);
                    }
                }
            }
        }
    }

    private bool IsSame(Tile[,] grid, int x1, int y1, int x2, int y2, Tile center)
    {
        Tile t = grid[x2, y2];
        return t != null && t.Type == center.Type;
    }
    private void AddLMatch(
     Tile[,] grid,
     int x,
     int y,
     int dx1, int dy1, // горизонталь
     int dx2, int dy2, // вертикаль
     List<MatchModel> matchModels)
    {
        var tiles = new List<Tile>
    {
        grid[x, y],
        grid[x + dx1, y + dy1],
        grid[x + dx1 * 2, y + dy1 * 2],
        grid[x + dx2, y + dy2],
        grid[x + dx2 * 2, y + dy2 * 2]
    };

        matchModels.Add(new MatchModel
        {
            ListTile = tiles,
            MatchType = MatchType.LForm
        });
    }



    private void FindHorizontal(Tile[,] grid, int width, int height, List<MatchModel> matchModels)
    {
        for (int y = 0; y < height; y++)
        {
            List<Tile> currentMatch = new List<Tile>();

            for (int x = 0; x < width; x++)
            {
                Tile current = grid[x, y];

                // Пропускаем пустые ячейки
                if (current == null)
                {
                    CheckAndAddMatch(currentMatch, matchModels);
                    currentMatch.Clear();
                    continue;
                }

                // Если стек пуст или тип совпадает с предыдущим — добавляем
                if (currentMatch.Count == 0 || currentMatch[0].Type == current.Type)
                {
                    currentMatch.Add(current);
                }
                else
                {
                    // Тип не совпадает — проверяем текущую последовательность
                    CheckAndAddMatch(currentMatch, matchModels);
                    currentMatch.Clear();
                    currentMatch.Add(current); // Начинаем новую последовательность
                }
            }

            // Проверяем последнюю последовательность в строке
            CheckAndAddMatch(currentMatch, matchModels);
        }
    }

    private void FindVertical(Tile[,] grid, int width, int height, List<MatchModel> matchModels)
    {
        for (int x = 0; x < width; x++)
        {
            List<Tile> currentMatch = new List<Tile>();

            for (int y = 0; y < height; y++)
            {
                Tile current = grid[x, y];

                if (current == null)
                {
                    CheckAndAddMatch(currentMatch, matchModels);
                    currentMatch.Clear();
                    continue;
                }

                if (currentMatch.Count == 0 || currentMatch[0].Type == current.Type)
                {
                    currentMatch.Add(current);
                }
                else
                {
                    CheckAndAddMatch(currentMatch, matchModels);
                    currentMatch.Clear();
                    currentMatch.Add(current);
                }
            }

            CheckAndAddMatch(currentMatch, matchModels);
        }
    }

    private void CheckAndAddMatch(List<Tile> match, List<MatchModel> matchModels)
    {
        if (match.Count >= 3)
        {
            MatchType matchType = match.Count switch
            {
                3 => MatchType.Three,
                4 => MatchType.Four,
                5 => MatchType.Five

            };

            matchModels.Add(new MatchModel { ListTile = new List<Tile>(match), MatchType = matchType });
        }
    }

    private void AddMatchComplexForm(List<Tile> match, List<MatchModel> matchModels, MatchType matchType)
    {
        matchModels.Add(new MatchModel { ListTile = new List<Tile>(match), MatchType = matchType });
    }

}
