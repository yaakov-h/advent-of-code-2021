var map = File.ReadAllText("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-9-1/input.txt");

var array = map.Split('\n').Select(x => x.Trim().AsEnumerable().Select(x => ParseInt(x)).ToArray()).ToArray();

var riskLevelSum = 0;

for (var x = 0; x < array.Length; x++)
{
    for (var y = 0; y < array[x].Length; y++)
    {
        if (IsLowPoint(array, x, y))
        {
            riskLevelSum += (array[x][y] + 1);
        }
    }
}

Console.WriteLine(riskLevelSum);

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