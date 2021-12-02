using var fs = File.OpenRead("/tmp/aoc/aoc-2-1/movement.txt");
using var reader = new StreamReader(fs);

var horiz = 0;
var depth = 0;

string line;

while (!string.IsNullOrEmpty((line = reader.ReadLine())))
{
    if (line.StartsWith("forward "))
    {
        var forward = int.Parse(line.Substring(8));
        horiz += forward;
    }
    else if (line.StartsWith("down "))
    {
        var down = int.Parse(line.Substring(5));
        depth += down;
    }
    else if (line.StartsWith("up "))
    {
        var up = int.Parse(line.Substring(3));
        depth -= up;
    }
}

Console.WriteLine(horiz * depth);