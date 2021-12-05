using System.Diagnostics;

var lines = File.ReadLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-5-1/input.txt")
    .Select(x => SplitCoords(x))
    .ToList();
var maxX = lines.Max(l => Math.Max(l.Start.X, l.End.X));
var maxY = lines.Max(l => Math.Max(l.Start.Y, l.End.Y));
var map = new int[maxX + 1, maxY + 1];

foreach (var line in lines)
{
    foreach (var point in line.GetCoordinates())
    {
            map[point.X, point.Y] += 1;
    }
}

var overlaps = 0;
for (var i = 0; i <= maxX; i++)
{
    for (var j = 0; j < maxY; j++)
    {
        if (map[i,j] > 1)
        {
            overlaps++;
        }
    }
}

Console.WriteLine(overlaps);


Line SplitCoords(string line)
{
    var parts = line.Split(' ');
    Debug.Assert(parts.Length == 3);
    return new Line(GetPoint(parts[0]), GetPoint(parts[2]));
}

Point GetPoint(string text)
{
    var parts = text.Split(',');
    Debug.Assert(parts.Length == 2);
    return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
}

record struct Point(int X, int Y);
record struct Line(Point Start, Point End)
{
    public IEnumerable<Point> GetCoordinates()
    {
        if (Start.X == End.X)
        {
            // Horizontal line
            var start = Math.Min(Start.Y, End.Y);
            var end = Math.Max(Start.Y, End.Y);
            for (var i = start; i <= end; i++)
            {
                yield return new Point(Start.X, i);
            }
        }
        else if (Start.Y == End.Y)
        {
            // Vertical line
            var start = Math.Min(Start.X, End.X);
            var end = Math.Max(Start.X, End.X);
            for (var i = start; i <= end; i++)
            {
                yield return new Point(i, Start.Y);
            }
        }
        else if (Start.X < End.X && Start.Y < End.Y)
        {
            // Diagonal (top-left -> bottom-right)
            var distance = End.X - Start.X;
            for (var i = 0; i <= distance; i++)
            {
                yield return new Point(Start.X + i, Start.Y + i);
            }
        }
        else if (Start.X < End.X && Start.Y > End.Y)
        {
            // Diagonal (bottom-left -> top-right)
            var distance = End.X - Start.X;
            for (var i = 0; i <= distance; i++)
            {
                yield return new Point(Start.X + i, Start.Y - i);
            }
        }
        else if (Start.X > End.X && Start.Y < End.Y)
        {
            // Diagonal (top-right -> bottom-left)
            var distance = Start.X - End.X;
            for (var i = 0; i <= distance; i++)
            {
                yield return new Point(Start.X - i, Start.Y + i);
            }
        }
        else if (Start.X > End.X && Start.Y > End.Y)
        {
            // Diagonal (bottom-right -> top-left)
            var distance = Start.X - End.X;
            for (var i = 0; i <= distance; i++)
            {
                yield return new Point(Start.X - i, Start.Y - i);
            }
        }
    }
}
