using System.Diagnostics;

var input = File.ReadAllText("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-16-2/input.txt");

Console.WriteLine(input);
Console.WriteLine(string.Join(string.Empty, GetBits(input).Select(x => x ? '1' : '0')));
Console.WriteLine();

var bitStream = GetBits(input).GetEnumerator();

var result = TryReadOnePacket(bitStream, out var value);
Console.WriteLine($"Success: {result}");
Console.WriteLine($"Value: {value}");

static bool TryReadOnePacket(IEnumerator<bool> bitStream, out long value)
{
    if (!TryRead3Bits(bitStream, out var version))
    {
        Console.WriteLine($"End of stream.");
        value = 0;
        return false;
    }
    
    Console.WriteLine($"Version: {version}");

    var typeId = Read3Bits(bitStream);
    Console.WriteLine($"Type: {typeId}");

    if (typeId == 4)
    {
        Console.WriteLine("Literal packet.");

        byte prefix;
        value = 0;
        do {
            prefix = Read1Bit(bitStream);
            var chunk = Read4Bits(bitStream);
            value <<= 4;
            value |= (long)chunk;
        } while (prefix == 1);
        Console.WriteLine($"  > Value: {value}");
        Console.WriteLine();
        return true;
    }
    else
    {
        var lengthTypeId = Read1Bit(bitStream);
        Console.WriteLine($"Operator packet (type: {lengthTypeId} = {GetName(typeId)})");
        long[] subpacketValues;

        if (lengthTypeId == 0)
        {
            var subpacketLength = Read15Bits(bitStream);
            Console.WriteLine($"  > The next {subpacketLength} bits are subpackets.");
            var buffer = BufferBits(bitStream, subpacketLength);
            Console.WriteLine($"  > Buffer: {string.Join(string.Empty, buffer.Select(x => x ? '1' : '0'))}");
            
            var enumerator = buffer.GetEnumerator();
            subpacketValues = ReadPacketValues(enumerator).ToArray();
        }
        else if (lengthTypeId == 1)
        {
            var numSubPackets = Read11Bits(bitStream);
            Console.WriteLine($"  > This packet contains {numSubPackets} packets");

            subpacketValues = new long[numSubPackets];
            for (var i = 0; i < numSubPackets; i++)
            {
                var read = TryReadOnePacket(bitStream, out var v);
                Debug.Assert(read);
                subpacketValues[i] = v;
            }
            Debug.Assert(subpacketValues.Length == numSubPackets);
        }
        else
        {
            throw new InvalidOperationException();
        }

        switch (typeId)
        {
            case 0:
                Console.WriteLine("  > Sum.");
                Console.WriteLine($"   > {string.Join(" + ", subpacketValues)}");
                value = subpacketValues.Sum();
                Console.WriteLine($"  > Sum = {value}");
                break;

            case 1:
                Console.WriteLine("  > Product.");
                Console.WriteLine($"   > {string.Join(" x ", subpacketValues)}");
                value = subpacketValues.Product();
                Console.WriteLine($"  > Product = {value}");
                break;

            case 2:
                Console.WriteLine("  > Min.");
                Console.WriteLine($"   > Min({string.Join(", ", subpacketValues)})");
                value = subpacketValues.Min();
                Console.WriteLine($"  > Min = {value}");
                break;

            case 3:
                Console.WriteLine("  > Max.");
                Console.WriteLine($"   > Max({string.Join(", ", subpacketValues)})");
                value = subpacketValues.Max();
                Console.WriteLine($"  > Max = {value}");
                break;

            case 5:
                Debug.Assert(subpacketValues.Length == 2);
                Console.WriteLine("  > Gt.");
                Console.WriteLine($"  > {subpacketValues[0]} > {subpacketValues[1]}");
                value = subpacketValues[0] > subpacketValues[1] ? 1 : 0;
                Console.WriteLine($"  > Eq = {value}");
                break;

            case 6:
                Debug.Assert(subpacketValues.Length == 2);
                Console.WriteLine("  > Lt.");
                Console.WriteLine($"  > {subpacketValues[0]} < {subpacketValues[1]}");
                value = subpacketValues[0] < subpacketValues[1] ? 1 : 0;
                Console.WriteLine($"  > Lt = {value}");
                break;

            case 7:
                Debug.Assert(subpacketValues.Length == 2);
                Console.WriteLine("  > Eq.");
                Console.WriteLine($"  > {subpacketValues[0]} == {subpacketValues[1]}");
                value = subpacketValues[0] == subpacketValues[1] ? 1 : 0;
                Console.WriteLine($"  > Eq = {value}");
                break;

            default:
                throw new NotSupportedException();
        }
    }

    Console.WriteLine();
    return true;
}

static string GetName(int typeId) => typeId switch {
    0 => "Sum",
    1 => "Product",
    2 => "Min",
    3 => "Max",
    4 => "Literal",
    5 => "Greater-Than",
    6 => "Less-Than",
    7 => "Equals",
    _ => throw new ArgumentOutOfRangeException(nameof(typeId))
};

static IEnumerable<long> ReadPacketValues(IEnumerator<bool> bitStream)
{
    var values = new List<long>();
    try
    {
        while (TryReadOnePacket(bitStream, out var value))
        {
            values.Add(value);
        }
    }
    catch (EndOfStreamException)
    {
        // choked on the final padding :(
    }

    return values;
}

static byte Read1Bit(IEnumerator<bool> bits)
{
    var moved = bits.MoveNext();
    if (!moved) throw new EndOfStreamException();
    return bits.Current ? (byte)1 : (byte)0;
}

static bool TryRead3Bits(IEnumerator<bool> bits, out byte value)
{
    value = 0;
    for (var i = 0; i < 3; i++)
    {
        if (!bits.MoveNext())
        {
            return false;
        }
        
        value <<= 1;
        value |= (bits.Current ? (byte)1 : (byte)0);
    }

    return true;
}

static int Read3Bits(IEnumerator<bool> bits)
{
    var value = 0;
    for (var i = 0; i < 3; i++)
    {
        var moved = bits.MoveNext();
        if (!moved) throw new EndOfStreamException();
        value <<= 1;
        value |= (bits.Current ? 1 : 0);
    }

    return value;
}

static int Read4Bits(IEnumerator<bool> bits)
{
    var value = 0;
    for (var i = 0; i < 4; i++)
    {
        var moved = bits.MoveNext();
        if (!moved) throw new EndOfStreamException();
        value <<= 1;
        value |= (bits.Current ? 1 : 0);
    }

    return value;
}

static int Read15Bits(IEnumerator<bool> bits)
{
    var value = 0;
    for (var i = 0; i < 15; i++)
    {
        var moved = bits.MoveNext();
        if (!moved) throw new EndOfStreamException();
        value <<= 1;
        value |= (bits.Current ? 1 : 0);
    }

    return value;
}

static int Read11Bits(IEnumerator<bool> bits)
{
    var value = 0;
    for (var i = 0; i < 11; i++)
    {
        var moved = bits.MoveNext();
        if (!moved) throw new EndOfStreamException();
        value <<= 1;
        value |= (bits.Current ? 1 : 0);
    }

    return value;
}


static IEnumerable<bool> GetBits(IEnumerable<char> text)
{
    foreach (var c in text)
    {
        byte value;

        if (c >= '0' && c <= '9') value = (byte)(c - '0');
        else if (c >= 'A' && c <= 'F') value = (byte)(c - 'A' + 10);
        else throw new FormatException($"Unknown input character '{c}'");

        for (var i = 3; i >= 0; i--)
        {
            var bit = (value >> i) & 1;
            yield return bit == 0 ? false : true;
        }
    }
}

static IEnumerable<bool> BufferBits(IEnumerator<bool> source, int numBits)
{
    var data = new bool[numBits];

    for (var i = 0; i < numBits; i++)
    {
        var moved = source.MoveNext();
        if (!moved) throw new EndOfStreamException();
        data[i] = source.Current;
    }

    return data;
}

static class EnumerableExtensions
{
    public static long Product(this IEnumerable<long> source)
    {
        var enumerator = source.GetEnumerator();
        var init = enumerator.MoveNext();
        var value = enumerator.Current;

        while (enumerator.MoveNext())
        {
            value *= enumerator.Current;
        }

        return value;
    }
}