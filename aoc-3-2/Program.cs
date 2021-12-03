// See https://aka.ms/new-console-template for more information
var binary = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-3-2/input.txt");
var length = binary[0].Length;

var oxygen = 0;
var co2 = 0;

var oxygen_lines = binary.ToList();
var co2_lines = binary.ToList();

for (var i = 0; i < length; i++)
{
    var num_zeros = 0;
    var num_ones = 0;

    if (oxygen_lines.Count == 1)
    {
        break;
    }

    foreach (var line in oxygen_lines)
    {
        var @char = line[i];
        if (@char == '0') {
            num_zeros++;
        }
        else if (@char == '1') {
            num_ones++;
        }
        else {
            throw new NotImplementedException();
        }
    }
    
    var mostCommon = num_ones >= num_zeros ? '1' : '0';
    Console.WriteLine($"Most common at {i} is {mostCommon}");

    oxygen_lines = oxygen_lines
    .Where(l => l[i] == mostCommon)
    .ToList();
}

Console.WriteLine("Oxygen: " + string.Join(",", oxygen_lines));
oxygen = ToInt(oxygen_lines[0]);

for (var i = 0; i < length; i++)
{
    var num_zeros = 0;
    var num_ones = 0;

    if (co2_lines.Count == 1)
    {
        break;
    }

    foreach (var line in co2_lines)
    {
        var @char = line[i];
        if (@char == '0') {
            num_zeros++;
        }
        else if (@char == '1') {
            num_ones++;
        }
        else {
            throw new NotImplementedException();
        }
    }
    
    var leastCommon = num_ones < num_zeros ? '1' : '0';
    Console.WriteLine($"Least common at {i} is {leastCommon}");

    co2_lines = co2_lines
    .Where(l => l[i] == leastCommon)
    .ToList();
}

Console.WriteLine("CO2: " + string.Join(",", co2_lines));
co2 = ToInt(co2_lines[0]);

Console.WriteLine(oxygen * co2);

static int ToInt(ReadOnlySpan<char> bits)
{
    var value = 0;
    
    for (var i = 0; i < bits.Length; i++)
    {
        if (i > 0)
        {
            value <<= 1;
        }

        if (bits[i] == '1')
        {
            value |= 1;
        }
    }

    return value;
}