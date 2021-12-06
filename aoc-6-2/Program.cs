using System.Collections;
using System.Diagnostics;

var input = File.ReadAllText("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-6-2/input.txt");
var initialValues = input.Split(',');

var ocean = new RotatingRingBuffer<long>(size: 9);

foreach (var value in initialValues)
{
    ocean[int.Parse(value)] += 1;
}

var days = 256;
var sw = Stopwatch.StartNew();

for (var i = 0; i < days; i++)
{
    var numCreatingNewLanternfish = ocean.Pop();
    ocean[6] += numCreatingNewLanternfish; // Reset
    ocean[8] += numCreatingNewLanternfish; // Babies!
}

Console.WriteLine($"Calculation time: {sw.Elapsed}");
Console.WriteLine($"Ocean State: " + string.Join(", ", ocean.Select((val, idx) => $"{idx}={val}")));
Console.WriteLine($"Total fish after {days} days is {ocean.Sum()}.");

class RotatingRingBuffer<T> : IEnumerable<T>
{
    public RotatingRingBuffer(int size)
    {
        storage = new T[size];
    }

    public readonly T[] storage;
    int offset;

    public ref T this[int index]
    {
        get => ref storage[(index + offset) % storage.Length];
    }

    public T Pop()
    {
        var item = storage[offset];
        storage[offset] = default!;
        offset = (offset + 1) % storage.Length;
        return item;
    }

    public Enumerator GetEnumerator() => new Enumerator(this);
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public struct Enumerator : IEnumerator<T>
    {
        public Enumerator(RotatingRingBuffer<T> buffer)
        {
            this.buffer = buffer;
        }

        RotatingRingBuffer<T> buffer;
        int index = -1;
        
        public T Current => buffer[index];

        object IEnumerator.Current => Current!;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (++index >= buffer.storage.Length)
            {
                return false;
            }

            return true;
        }

        public void Reset()
        {
            index = 0;
        }
    }
}
