// See https://aka.ms/new-console-template for more information
var binary = File.ReadAllLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-3-1/input.txt");
var length = binary[0].Length;

int gamma = 0;
int epsilon = 0;
for (var i = 0; i < length; i++)
{
    var num_zeros = 0;
    var num_ones = 0;

    foreach (var line in binary)
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

    if (i > 0)
    {
        epsilon = epsilon << 1;
        gamma = gamma << 1;
    }

    if (num_zeros > num_ones)
    {
        epsilon = epsilon | 1;
    }
    else 
    {
        gamma = gamma | 1;
    }
}

Console.WriteLine(gamma * epsilon);