// See https://aka.ms/new-console-template for more information
var input = File.ReadAllText("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-6-1/input.txt");
var initialValues = input.Split(',');

var values = new List<int>(capacity: initialValues.Length);
values.AddRange(initialValues.Select(static s => int.Parse(s)));

var days = 80;

for (var day = 0; day < days; day++)
{
    var initialCount = values.Count;

    for (var i = 0; i < initialCount; i++)
    {
        var value = values[i];
        if (value > 0)
        {
            values[i] = values[i] - 1;
        }
        else
        {
            values[i] = 6;
            values.Add(8);
        }
    }
}

Console.WriteLine($"Total fish after {days} days is {values.Count}.");
