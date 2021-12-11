var input = File.ReadLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-11-1/input.txt");

var inputs = input.Select(x => x.Trim().Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
var map = new int[inputs.Length, inputs[0].Length];

for (var x = 0; x < inputs[0].Length; x++)
{
    for (var y = 0; y < inputs.Length; y++)
    {
        map[x, y] = inputs[x][y];
    }
}

var flashes_count = 0;
var iterations = 100;

Console.WriteLine("Before any steps:");
PrintMap(map);

for (var i = 0; i < iterations; i++)
{
    // Step 1: Increment
    for (var x = 0; x < map.GetLength(0); x++)
    {
        for (var y = 0; y < map.GetLength(1); y++)
        {
            map[x, y] += 1;
        }
    }
    
    // Step 2: Flash
    var flashes = new HashSet<(int x, int y)>();

    for (var x = 0; x < map.GetLength(0); x++)
    {
        for (var y = 0; y < map.GetLength(1); y++)
        {
            ProcessFlash(map, x, y, flashes);
        }
    }

    // Step 3: Reset
    foreach (var point in flashes)
    {
        map[point.x, point.y] = 0;
    }

    Console.WriteLine($"After step {i+1}:");
    PrintMap(map);

    flashes_count += flashes.Count;
}

Console.WriteLine($"Total flashes: {flashes_count}");

static void ProcessFlash(int[,] map, int x, int y, ISet<(int x, int y)> flashTracker)
{
    if (map[x, y] <= 9 || !flashTracker.Add((x, y)))
    {
        return;
    }
        
    //Console.WriteLine($"({x},{y}) flashed.");

    foreach (var adjacent in GetAdjacent(x, y, map))
    {
        var energy = map[adjacent.x, adjacent.y];
        map[adjacent.x, adjacent.y] += 1;
        //Console.WriteLine($"  > ({adjacent.x},{adjacent.y}) gained energy: {energy} => ({energy + 1})");
        ProcessFlash(map, adjacent.x, adjacent.y, flashTracker);
    }
}

static IEnumerable<(int x, int y)> GetAdjacent(int x, int y, int[,] map)
{
    var maxX = map.GetLength(0);
    var maxY = map.GetLength(1);

    if (x > 0)
    {
        if (y > 0) yield return (x - 1, y - 1);
        yield return (x - 1, y);
        if (y + 1 < maxY) yield return (x - 1, y + 1);
    }

    if (y > 0) yield return (x, y - 1);

    if (x + 1 < maxX)
    {
        if (y > 0) yield return (x + 1, y - 1);
        yield return (x + 1, y);
        if (y + 1 < maxY) yield return (x + 1, y + 1);
    }

    if (y + 1 < maxY) yield return (x, y + 1);
}

static void PrintMap(int[,] map)
{
    for (var x = 0; x < map.GetLength(0); x++)
    {
        for (var y = 0; y < map.GetLength(1); y++)
        {
            var value = map[x, y];
            if (value > 9)
            {
                Console.Write('+');
            }
            else
            {
                Console.Write(value);
            }
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}