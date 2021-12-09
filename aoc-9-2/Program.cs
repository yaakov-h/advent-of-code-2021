var map = File.ReadAllText("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-9-2/input.txt");

var array = map.Split('\n').Select(x => x.Trim().AsEnumerable().Select(x => ParseInt(x)).ToArray()).ToArray();

var basins = new List<int>();

for (var x = 0; x < array.Length; x++)
{
    for (var y = 0; y < array[x].Length; y++)
    {
        if (IsLowPoint(array, x, y))
        {
            basins.Add(GetBasinSize(array, x, y));
        }
    }
}

basins.Sort();
basins.Reverse();

Console.WriteLine(basins[0] * basins[1] * basins[2]);

static int ParseInt(char c)
{
    Span<char> s = stackalloc char[1];
    s[0] = c;
    return int.Parse(s);
}

static bool IsLowPoint(int[][] map, int x, int y)
{
    var value = map[x][y];
    
    if (x >= 1 && map[x - 1][y] <= value)
    {
        return false;
    }

    if (x < map.Length - 1 && map[x + 1][y] <= value)
    {
        return false;
    }

    if (y >= 1 && map[x][y - 1] <= value)
    {
        return false;
    }

    if (y < map[x].Length - 1 && map[x][y + 1] <= value)
    {
        return false;
    }

    return true;
}

static int GetBasinSize(int[][] map, int x, int y)
{
    var visited = new HashSet<(int, int)>();

    var queue = new Queue<(int, int)>();
    queue.Enqueue((x, y));

    var count = 0;

    while (queue.TryDequeue(out var point))
    {
        var (pX, pY) = point;

        if (!visited.Add((pX, pY)))
        {
            continue;
        }

        if (map[pX][pY] == 9)
        {
            continue;
        }

        count++;

        if (pX > 0)
        {
            queue.Enqueue((pX - 1, pY));
        }

        if (pY > 0)
        {
            queue.Enqueue((pX, pY - 1));
        }

        if (pX + 1 < map.Length)
        {
            queue.Enqueue((pX + 1, pY));
        }

        if (pY + 1 < map[pX].Length)
        {
            queue.Enqueue((pX, pY + 1));
        }
    }

    return count;
}