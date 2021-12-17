using System.Collections.Immutable;

var input = // File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-15-1/input.txt");

@"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581".Split('\n').Select(x => x.Trim().ToArray());

var map = new int[input[0].Length, input.Length];
for (var y = 0; y < input.Length; y++)
{
    for (var x = 0; x < input[0].Length; x++)
    {
        map[x, y] = Parse(input[y][x]);
    }
}

long? lowestPathSum = null;
var target = (x: map.GetLength(0) - 1, y: map.GetLength(1) - 1);

var queue = new Stack<Track>();
queue.Push(new Track(0, (0, 0)));
while (queue.TryPop(out var item))
{
    //Console.WriteLine();
    //Console.WriteLine($"Queue size: {queue.Count}");
    //Console.WriteLine($"Point: {item.Point.x:000}, {item.Point.y:000}");
    if (item.Point == target)
    {
        //Console.WriteLine($"Found path with sum {item.Sum}");
        if (lowestPathSum is null || lowestPathSum.Value > item.Sum)
        {
            Console.WriteLine($"New lowest path: {item.Sum}.");
            lowestPathSum = item.Sum;
        }
        continue;
    }
    
    if (lowestPathSum != null && lowestPathSum.Value < item.Sum)
    {
        continue;
    }
    
    QueueIfInBounds((item.Point.x + 1, item.Point.y));
    QueueIfInBounds((item.Point.x, item.Point.y + 1));

    void QueueIfInBounds((int x, int y) pt)
    {
        if (pt.x < 0) return;
        if (pt.y < 0) return;
        if (pt.x >= map.GetLength(0)) return;
        if (pt.y >= map.GetLength(1)) return;
        if (item.Visited(pt)) return; // Don't revisit.

        //Console.WriteLine($"Considering {pt.x:000}, {pt.y:000} ...");

        var value = map[pt.x, pt.y];

        if (lowestPathSum != null)
        {
            if (value + item.Sum > lowestPathSum)
            {
                //Console.WriteLine("Abandoning path, exceeded minimum.");
                return;
            }

            var budget = lowestPathSum.Value - value;
            var distance = target.x - pt.x + target.y - pt.y;
            if (distance > budget)
            {
                Console.WriteLine("Abandoning path, will never make it.");
                return;
            }
        }

        //Console.WriteLine("We'll give it a go.");
        queue.Push(item.Add(value, pt));
    }
}

Console.WriteLine($"Lowest risk path: {lowestPathSum}");

static byte Parse(char c)
{
    Span<char> buffer = stackalloc char[1];
    buffer[0] = c;
    return byte.Parse(buffer);
}

class Track
{
    public Track(long initialValue, (int x, int y) point)
    {
        this.sum = initialValue;
        this.visited = ImmutableHashSet.Create(point);
        this.point = point;
    }

    
    public Track(long sum, ImmutableHashSet<(int x, int y)> visited, (int x, int y) point)
    {
        this.sum = sum;
        this.visited = visited;
        this.point = point;
    }

    readonly ImmutableHashSet<(int x, int y)> visited;
    long sum;
    (int x, int y) point;

    public long Sum => sum;
    public bool Visited((int x, int y) pt) => visited.Contains(pt);
    public (int x, int y) Point => point;

    public Track Add(long value, (int x, int y) point)
        => new Track(
            sum + value,
            visited.Add(point),
            point
        );
}