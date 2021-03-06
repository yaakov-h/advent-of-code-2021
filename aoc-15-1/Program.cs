using System.Diagnostics;

var input = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-15-1/input.txt");

// @"1163751742
// 1381373672
// 2136511328
// 3694931569
// 7463417111
// 1319128137
// 1359912421
// 3125421639
// 1293138521
// 2311944581".Split('\n').Select(x => x.Trim()).ToArray();

var map = new int[input[0].Length, input.Length];
for (var y = 0; y < input.Length; y++)
for (var x = 0; x < input[0].Length; x++)
{
    map[x, y] = Parse(input[y][x]);
}

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

            if (x > 0 && risks[x - 1, y] is var leftCost && leftCost != null)
            {
                risk = Math.Min(risk, leftCost.Value);
            }

            if (y < map.GetLength(1) - 1 && risks[x, y + 1] is var downCost && downCost != null)
            {
                risk = Math.Min(risk, downCost.Value);
            }

            if (y > 0 && risks[x, y - 1] is var upCost && upCost != null)
            {
                risk = Math.Min(risk, upCost.Value);
            }

            var newRisk = risk + value;
            if (risks[x, y] != newRisk)
            {
                Console.WriteLine($"Updated ({x}, {y}): {risks[x, y]} => {newRisk}");
                risks[x, y] = newRisk;
                updated++;
            }
        }
    }

    Console.WriteLine($"Updated {updated} risks.");
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
