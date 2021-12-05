using System.Diagnostics;

var lines = File.ReadLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-5-1/input.txt")
    .Select(x => SplitCoords(x))
    .ToList();
var maxX = lines.Max(l => Math.Max(l.Start.X, l.End.X));
var maxY = lines.Max(l => Math.Max(l.Start.Y, l.End.Y));
var map = new int[maxX + 1, maxY + 1];

foreach (var line in lines)
{
    if (line.Start.X == line.End.X)
    {
        // Horizontal line
        var start = Math.Min(line.Start.Y, line.End.Y);
        var end = Math.Max(line.Start.Y, line.End.Y);
        for (var i = start; i <= end; i++)
        {
            map[line.Start.X, i] += 1;
        }
    }
    else if (line.Start.Y == line.End.Y)
    {
        // Vertical line
        var start = Math.Min(line.Start.X, line.End.X);
        var end = Math.Max(line.Start.X, line.End.X);
        for (var i = start; i <= end; i++)
        {
            map[i, line.Start.Y] += 1;
        }
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
record struct Line(Point Start, Point End);