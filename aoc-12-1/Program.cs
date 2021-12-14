using System.Collections.Immutable;

var input = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-12-1/input.txt");

var numRoutesToEnd = new Dictionary<string, int>();
var edges = input.Select(x => x.Split('-', 2)).Select(x => (x[0], x[1])).ToArray();
var vertices = edges.Select(x => x.Item1).Concat(edges.Select(x => x.Item2)).Distinct().ToArray();
var edgesByEnd = edges.Concat(edges.Select(e => (e.Item2, e.Item1))).ToLookup(x => x.Item2, x => x.Item1);
var smallCaves = vertices.Where(x => x.All(c => char.IsLower(c)));

var routes = new List<ImmutableList<string>>();

var cavesToCheck = new Queue<ImmutableList<string>>();
cavesToCheck.Enqueue(ImmutableList.Create("end"));

while (cavesToCheck.TryDequeue(out var route))
{
    Console.WriteLine($"Current route: {string.Join(",", route)}");
    var currentCave = route[0];
    
    if (currentCave == "start")
    {
        Console.WriteLine($"  > Found a path: {string.Join(",", route)}");
        routes.Add(route);
        continue;
    }

    foreach (var inbound in edgesByEnd[currentCave])
    {
        if (inbound.All(x => char.IsLower(x)) && route.Contains(inbound))
        {
            continue; // visit small caves (plus start/end) at most once
        }

        if (ListContainsAdjacentElements(route, inbound, currentCave))
        {
            Console.WriteLine($"  > Already checked {inbound} -> {currentCave} on current route.");
            continue;
        }

        Console.WriteLine($"  > Checking next: {inbound} -> {currentCave}");
        cavesToCheck.Enqueue(route.Insert(0, inbound));
    }
}

Console.WriteLine($"Found {routes.Count} routes.");

bool ListContainsAdjacentElements<T>(ImmutableList<T> list, T first, T second)
    where T : class
{
    for (var i = 0; i < list.Count - 1; i++)
    {
        if (list[i] == first && list[i + 1] == second)
        {
            return true;
        }
    }
    return false;
}