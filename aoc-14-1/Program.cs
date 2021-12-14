var input = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-14-1/input.txt");

var polymer = input[0].ToList();
var rules = input.Skip(2).ToDictionary(x => (x[0], x[1]), x => x[^1]);

for (var i = 0; i < 10; i++)
{
    for (var idx = 0; idx < polymer.Count - 1; idx++)
    {
        var key = (polymer[idx], polymer[idx + 1]);
        var addition = rules[key];
        polymer.Insert(idx + 1, addition);
        idx++;
    }

    Console.WriteLine($"After step {i+1}: {string.Join(string.Empty, polymer)}");
}

var popularity = polymer.GroupBy(x => x).OrderBy(x => x.Count());
var leastCommonQty = popularity.First().Count();
var mostCommonQty = popularity.Last().Count();

Console.WriteLine(mostCommonQty - leastCommonQty);