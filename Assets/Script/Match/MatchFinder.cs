using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MatchFinder : IMatchFinder
{
    public List<MatchModel> FindMatches(Tile[,] grid, int width, int height)
    {
        List<MatchModel> matchModels = new List<MatchModel>();

        FindHorizontal(grid, width, height, matchModels);
        FindVertical(grid, width, height, matchModels);
        FindComplexMatches(grid, width, height, matchModels);

        return matchModels;
    }

    private void FindComplexMatches(Tile[,] grid, int width, int height, List<MatchModel> matchModels)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile center = grid[x, y];
                if (center == null) continue;

                int left = CountDirection(grid, x, y, -1, 0, width, height, center.Type);
                int right = CountDirection(grid, x, y, 1, 0, width, height, center.Type);
                int up = CountDirection(grid, x, y, 0, 1, width, height, center.Type);
                int down = CountDirection(grid, x, y, 0, -1, width, height, center.Type);

                bool horizontal = left + right >= 2;
                bool vertical = up + down >= 2;

                if (!(horizontal && vertical)) //данные линии обрабатываются в FindHorizontal и FindVertical
                    continue;

                MatchType type;
                int directions = 0;

                if (left > 0) directions++;
                if (right > 0) directions++;
                if (up > 0) directions++;
                if (down > 0) directions++;

                if (directions >= 3)
                    type = MatchType.TForm;
                else if (horizontal && vertical)
                    type = MatchType.LForm;
                else
                    continue; // это обычная линия

                var tiles = new List<Tile> { center };

                AddDirectionTiles(grid, tiles, x, y, -1, 0, left);
                AddDirectionTiles(grid, tiles, x, y, 1, 0, right);
                AddDirectionTiles(grid, tiles, x, y, 0, 1, up);
                AddDirectionTiles(grid, tiles, x, y, 0, -1, down);

                matchModels.Add(new MatchModel
                {
                    ListTile = tiles,
                    MatchType = type
                });
            }
        }
    }

    private int CountDirection(Tile[,] grid, int startX,int startY,int dx,int dy,int width,
        int height,TileType type)
    {
        int count = 0;

        int x = startX + dx;
        int y = startY + dy;

        while (x >= 0 && x < width && y >= 0 && y < height)
        {
            Tile t = grid[x, y];
            if (t == null || t.Type != type)
                break;

            count++;
            x += dx;
            y += dy;
        }

        return count;
    }

    private void AddDirectionTiles(Tile[,] grid,List<Tile> tiles,int startX, int startY,
        int dx,int dy,int count)
    {
        for (int i = 1; i <= count; i++)
        {
            tiles.Add(grid[startX + dx * i, startY + dy * i]);
        }
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
        if (match.Count >= 5)
        {
            matchModels.Add(new MatchModel { ListTile = new List<Tile>(match), MatchType = MatchType.Five });
            return;
        }

        if (match.Count >= 3)
        {
            MatchType matchType = match.Count switch
            {
                3 => MatchType.Three,
                4 => MatchType.Four
            };

            matchModels.Add(new MatchModel { ListTile = new List<Tile>(match), MatchType = matchType });
        }

    }


}
