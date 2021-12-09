var lines = File.ReadLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-8-1/input.txt");

var count1478 = 0;

foreach (var line in lines)
{
    var parts = line.Trim().Split(" | ");
    var signals = parts[0].Split(' ');
    var digits = parts[1].Split(' ');

    foreach (var digit in digits)
    {
        if (digit is { Length: 2 or 4 or 3 or 7 })
        {
            count1478++;
        }
    }
}

Console.WriteLine(count1478);