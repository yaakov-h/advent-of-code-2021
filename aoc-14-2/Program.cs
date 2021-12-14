var input = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-14-2/input.txt");

var polymer = input[0].ToList();
var rules = input.Skip(2).ToDictionary(x => (x[0], x[1]), x => x[^1]);
var states = rules.ToDictionary(x => x.Key, x => new Box<long>());
var letterCount = polymer.GroupBy(x => x).ToDictionary(x => x.Key, x => new Box<long> { Value = x.Count() });

for (var i = 0; i < polymer.Count - 1; i++)
{
    var key = (polymer[i], polymer[i + 1]);
    states[key].Value += 1;
}

for (var i = 0; i < 40; i++)
{
    var changes = states.ToDictionary(x => x.Key, x => new Box<long>());
    foreach (var kvp in states)
    {
        if (kvp.Value.Value == 0) continue;

        var addition = rules[kvp.Key];
        changes[(kvp.Key.Item1, addition)].Value += kvp.Value.Value;
        changes[(addition, kvp.Key.Item2)].Value += kvp.Value.Value;
        changes[kvp.Key].Value -= kvp.Value.Value;

        if (!letterCount.TryGetValue(addition, out var counter))
        {
            letterCount[addition] = counter = new Box<long>();
        }
        counter.Value += kvp.Value.Value;
    }

    foreach (var change in changes)
    {
        states[change.Key].Value += change.Value.Value;
    }
}

var popularity = letterCount.OrderBy(x => x.Value.Value);
var leastCommonQty = popularity.First().Value.Value;
var mostCommonQty = popularity.Last().Value.Value;

Console.WriteLine(mostCommonQty - leastCommonQty);

class Box<T> where T : struct
{
    public T Value { get; set; }
}