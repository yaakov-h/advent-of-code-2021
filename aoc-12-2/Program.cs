using System.Collections.Immutable;

var input = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-12-2/input.txt");

// @"fs-end
// he-DX
// fs-he
// start-DX
// pj-DX
// end-zg
// zg-sl
// zg-pj
// pj-he
// RW-he
// fs-DX
// pj-RW
// zg-RW
// start-pj
// he-WI
// zg-he
// pj-fs
// start-RW".Split('\n').Select(x => x.Trim()).ToArray();

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
    //Console.WriteLine($"Current route: {string.Join(",", route)}");
    var currentCave = route[0];
    
    if (currentCave == "start")
    {
        //Console.WriteLine($"  > Found a path: {string.Join(",", route)}");
        routes.Add(route);
        continue;
    }

    foreach (var inbound in edgesByEnd[currentCave])
    {
        if (IsSmallCave(inbound) && route.Contains(inbound))
        {
            if (inbound == "end") continue;
            
            if (route.Where(x => IsSmallCave(x)).GroupBy(x => x).Any(x => x.Count() > 1))
            {
                // We can visit _one_ small cave per route, and we already did.
                //Console.WriteLine($"  > Already have one duplicate small cave on this route.");
                continue;
            }
        }

        if (ListContainsLoop(route, inbound, currentCave))
        {
            //Console.WriteLine($"  > Already checked {inbound} -> {currentCave} on current route.");
            continue;
        }

        //Console.WriteLine($"  > Checking next: {inbound} -> {currentCave}");
        cavesToCheck.Enqueue(route.Insert(0, inbound));
    }
}

Console.WriteLine($"Found {routes.Count} routes.");

bool IsSmallCave(string cave) => cave.All(x => char.IsLower(x));

bool ListContainsLoop<T>(ImmutableList<T> list, T first, T second)
    where T : class
{
    var count = 0;
    for (var i = 0; i < list.Count - 1; i++)
    {
        if (list[i] == first && list[i + 1] == second)
        {
            count++;
        }
    }
    return count > 2;
}