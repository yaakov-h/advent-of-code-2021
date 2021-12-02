using var input = File.OpenRead("/tmp/aoc/input-1.txt");
using var reader = new StreamReader(input);
string line;
const int windowSize = 3;
int? lastWindowSum = null;
var window = new List<int>(capacity: windowSize);
int count = 0;
while ((line = reader.ReadLine()) != null)
{
    var value = int.Parse(line);
    window.Add(value);

    if (window.Count < windowSize)
    {
        continue;
    }
    
    if (lastWindowSum is not null)
    {
        window.RemoveAt(0);
        var windowSum = window.Sum();
        if (windowSum > lastWindowSum.Value)
        {
            count++;
        }
    }

    lastWindowSum = window.Sum();
}
Console.WriteLine(count);
