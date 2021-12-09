using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

var lines = File.ReadLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-8-2/input.txt");

var total = 0;

foreach (var line in lines)
{
    var parts = line.Trim().Split(" | ");
    var signals = parts[0].Split(' ');
    var digits = parts[1].Split(' ');

    var one = signals.FirstOrDefault(s => s.Length == 2);
    var four = signals.FirstOrDefault(s => s.Length == 4);
    var seven = signals.FirstOrDefault(s => s.Length == 3);
    var eight = signals.FirstOrDefault(s => s.Length == 7);

    var zero = signals.FirstOrDefault(x => x.Length == 6 && x.ContainsAllChars(one ?? seven) == true && x.ContainsAllChars(four) == false);
    var nine = signals.FirstOrDefault(x => x.Length == 6 && x.ContainsAllChars(one ?? seven) == true && x.ContainsAllChars(four) == true);
    var six = signals.FirstOrDefault(x => x.Length == 6 && x.ContainsAllChars(four ?? one) == false && x.ContainsAllChars(seven) == false);

    var three = signals.FirstOrDefault(x => x.Length == 5 && x.ContainsAllChars(seven ?? one) == true && x.ContainsAllChars(four) == false);

    var two = signals.FirstOrDefault(x => x.Length == 5 && x.ContainsAllChars(seven ?? one) == false  && four != null && x.Intersect(four).Count() == 2);
    var five = signals.FirstOrDefault(x => x.Length == 5 && x.ContainsAllChars(seven ?? one) == false && four != null && x.Intersect(four).Count() == 3);

    var lookup = new Dictionary<string, int>(new UnorderedCharacterStringComparer());
    if (one is not null) lookup[one] = 1;
    if (two is not null) lookup[two] = 2;
    if (three is not null) lookup[three] = 3;
    if (four is not null) lookup[four] = 4;
    if (five is not null) lookup[five] = 5;
    if (six is not null) lookup[six] = 6;
    if (seven is not null) lookup[seven] = 7;
    if (eight is not null) lookup[eight] = 8;
    if (nine is not null) lookup[nine] = 9;
    if (zero is not null) lookup[zero] = 0;
    
    Console.WriteLine($"Keys: {string.Join(", ", lookup.Keys)}");

    var value = 0;
    for (var i = 0; i < digits.Length; i++)
    {
        Console.WriteLine($"Fetching: {digits[i]}");
        var gotValue = lookup.TryGetValue(digits[i], out var num);
        Debug.Assert(gotValue);

        if (i > 0)
        {
            value *= 10;
        }

        value += num;
    }

    total += value;
}

Console.WriteLine(total);

static class StringExtensions
{
    public static bool? ContainsAllChars(this string first, string? second)
    {
        if (second is null) return null;

        foreach (var c in second)
        {
            if (!first.Contains(c))
            {
                return false;
            }
        }

        return true;
    }
}

class UnorderedCharacterStringComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        Debug.Assert(x != null);
        Debug.Assert(y != null);

        Span<char> spanX = stackalloc char[x.Length];
        x.CopyTo(spanX);
        spanX.Sort();

        Span<char> spanY = stackalloc char[y.Length];
        y.CopyTo(spanY);
        spanY.Sort();

        return MemoryExtensions.Equals(spanX, spanY, StringComparison.Ordinal);
    }

    public int GetHashCode([DisallowNull] string obj)
    {
        Debug.Assert(obj != null);

        Span<char> span = stackalloc char[obj.Length];
        obj.CopyTo(span);
        span.Sort();

        return span.ToString().GetHashCode();
    }
}