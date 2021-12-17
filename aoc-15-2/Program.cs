using System.Diagnostics;

var input = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-15-2/input.txt");

var tiling = 5;
var map = new int[input[0].Length * tiling, input.Length * tiling];
var size = (width: input[0].Length, height: input.Length);

for (var tilex = 0; tilex < tiling; tilex++)
for (var tiley = 0; tiley < tiling; tiley++)
{
    FillMap(map, tilex * size.width, tiley * size.height, tilex + tiley);
}

void FillMap(int[,] map, int xOffset, int yOffset, int increment)
{
    for (var y = 0; y < input.Length; y++)
    for (var x = 0; x < input[0].Length; x++)
    {
        var value = Parse(input[y][x]) + increment;
        while (value > 9) value -= 9;
        map[x + xOffset, y + yOffset] = value;
    }
}


for (var y = 0; y < map.GetLength(1); y++)
{
    for (var x = 0; x < map.GetLength(0); x++)
    {
        Console.Write(map[x, y]);
    }
    Console.WriteLine();
}
Console.WriteLine();

var risks = new int?[map.GetLength(0), map.GetLength(1)];
risks[map.GetLength(0) - 1, map.GetLength(1) - 1] = map[map.GetLength(0) - 1, map.GetLength(1) - 1];

int updated;
do
{
    updated = 0;
    for (var x = map.GetLength(0) - 1; x >= 0; x--)
    for (var y = map.GetLength(1) - 1; y >= 0; y--)
    {
        if (x > 0)
        {
            Update(x - 1, y);
        }

        if (y > 0)
        {
            Update(x, y - 1);
        }

        void Update(int x, int y)
        {
            var value = map[x, y];
            var risk = int.MaxValue;

            if (x < map.GetLength(0) - 1 && risks[x + 1, y] is var rightCost && rightCost != null)
            {
                risk = Math.Min(risk, rightCost.Value);
            }

            if (y < map.GetLength(1) - 1 && risks[x, y + 1] is var downCost && downCost != null)
            {
                risk = Math.Min(risk, downCost.Value);
            }

            // The rules don't specify this but just in case the best path snakes back at some point,
            // Consider values to the left and above us.

            if (x > 0 && risks[x - 1, y] is var leftCost && leftCost != null)
            {
                risk = Math.Min(risk, leftCost.Value);
            }

            if (y > 0 && risks[x, y - 1] is var upCost && upCost != null)
            {
                risk = Math.Min(risk, upCost.Value);
            }

            var newRisk = risk + value;
            if (risks[x, y] != newRisk)
            {
                //Console.WriteLine($"Updated ({x}, {y}): {risks[x, y]} => {newRisk}");
                risks[x, y] = newRisk;
                updated++;
            }
        }
    }

    //Console.WriteLine($"Updated {updated} risks.");
}
while (updated > 0);

var lowestRiskPath = risks[0, 0] - map[0, 0];
Console.WriteLine($"Lowest risk path: {lowestRiskPath}");

static byte Parse(char c)
{
    Span<char> buffer = stackalloc char[1];
    buffer[0] = c;
    return byte.Parse(buffer);
}
