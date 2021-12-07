var input = File.ReadAllText("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-7-1/input.txt");

var values = Array.ConvertAll(input.Split(','), x => int.Parse(x));
var maxValue = values.Max();
var map = new int[maxValue + 1];
foreach (var value in values)
{
    map[value] += 1;
}

var lowestCost = int.MaxValue;

for (var i = 0; i <= maxValue; i++)
{
    var cost = 0;
    for (var j = 0; j <= maxValue; j++)
    {
        cost += (Math.Abs(j - i) * map[j]);
    }

    if (cost < lowestCost)
    {
        lowestCost = cost;
    }
}

Console.WriteLine(lowestCost);