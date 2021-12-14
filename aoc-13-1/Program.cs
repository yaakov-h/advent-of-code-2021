var input = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-13-1/input.txt");

var coordinateLines = input.TakeWhile(x => x.Length > 0).ToArray();
var foldLines = input.Where(x => x.StartsWith("fold ")).ToArray();
var coordinates = coordinateLines.Select(x => x.Split(',', 2)).Select(x => (x: int.Parse(x[0]), y: int.Parse(x[1]))).ToArray();
var maxX = coordinates.Max(p => p.x);
var maxY = coordinates.Max(p => p.y);

var grid = new bool[maxX + 1, maxY + 1];
var viewport = (width: maxX + 1, height: maxY + 1);

foreach (var coord in coordinates)
{
    grid[coord.x, coord.y] = true;
}

PrintGrid(grid, viewport.width, viewport.height);

foreach (var line in foldLines.Take(1))
{
    var instruction = line[11..].Split('=', 2);
    var dimension = instruction[0];
    var fold = int.Parse(instruction[1]);
    Console.WriteLine($"Folding along {dimension} = {fold}");
    
    switch (dimension)
    {
        case "x":
            FoldLeft(grid, fold, viewport.width, viewport.height);
            viewport.width = fold;
        break;

        case "y":
            FoldUp(grid, fold, viewport.width, viewport.height);
            viewport.height = fold;
            break;

        default:
            throw new NotSupportedException();
    }
    PrintGrid(grid, viewport.width, viewport.height);
    Console.WriteLine($"There are {CountDots(grid, viewport.width, viewport.height)} dots visible.");
}

static void PrintGrid(bool[,] grid, int viewportWidth, int viewportHeight)
{
    for (var y = 0; y < viewportHeight; y++)
    {
        for (var x = 0; x < viewportWidth; x++)
        {
            var c = grid[x, y] ? '#' : '.';
            Console.Write(c);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

static void FoldLeft(bool[,] grid, int fold, int viewportWidth, int viewportHeight)
{
    for (var x = fold + 1; x < viewportWidth; x++)
    {
        var targetX = fold - (x - fold);
        for (var y = 0; y < grid.GetLength(1); y++)
        {
            grid[targetX, y] |= grid[x, y];
        }
    }
}

static void FoldUp(bool[,] grid, int fold, int viewportWidth, int viewportHeight)
{
    for (var y = fold + 1; y < viewportHeight; y++)
    {
        var targetY = fold - (y - fold);
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            grid[x, targetY] |= grid[x, y];
        }
    }
}

static long CountDots(bool[,] grid, int viewportWidth, int viewportHeight)
{
    var counter = 0;
    
    for (var y = 0; y < viewportHeight; y++)
    {
        for (var x = 0; x < viewportWidth; x++)
        {
            if (grid[x, y]) counter++;
        }
        Console.WriteLine();
    }

    return counter;
}