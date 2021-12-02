using var input = File.OpenRead("/tmp/aoc/input-1.txt");
using var reader = new StreamReader(input);
string line;
int? lastValue = null;
int count = 0;
while ((line = reader.ReadLine()) != null)
{
    var value = int.Parse(line);
    if (lastValue is not null)
    {
        if (value > lastValue.Value)
        {
            count++;
        }
    }
    lastValue = value;
}
Console.WriteLine(count);
