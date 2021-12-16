using System.Diagnostics;

var input = File.ReadAllText("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-16-1/input.txt");

Console.WriteLine(input);
Console.WriteLine(string.Join(string.Empty, GetBits(input).Select(x => x ? '1' : '0')));
Console.WriteLine();

var bitStream = GetBits(input).GetEnumerator();
var versionSum = 0;
try
{
    while (ReadOnePacket(bitStream, ref versionSum)) { }
}
catch (EndOfStreamException)
{
    // choked on the final padding :(
}

Console.WriteLine();
Console.WriteLine($"Version sum = {versionSum}");

static bool ReadOnePacket(IEnumerator<bool> bitStream, ref int versionSum)
{
    if (!TryRead3Bits(bitStream, out var version))
    {
        Console.WriteLine($"End of stream.");
        return false;
    }
    
    Console.WriteLine($"Version: {version}");
    versionSum += version;

    var typeId = Read3Bits(bitStream);
    Console.WriteLine($"Type: {typeId}");

    if (typeId == 4)
    {
        Console.WriteLine("Literal packet.");

        long value = 0;
        byte prefix;
        do {
            prefix = Read1Bit(bitStream);
            var chunk = Read4Bits(bitStream);
            value <<= 4;
            value |= (long)chunk;
        } while (prefix == 1);
        Console.WriteLine($"  > Value: {value}");
    }
    else
    {
        var lengthTypeId = Read1Bit(bitStream);
        Console.WriteLine($"Operator packet (type: {lengthTypeId})");

        if (lengthTypeId == 0)
        {
            var subpacketLength = Read15Bits(bitStream);
            Console.WriteLine($"  > The next {subpacketLength} bits are subpackets.");
            var buffer = BufferBits(bitStream, subpacketLength);
            Console.WriteLine($"  > Buffer: {string.Join(string.Empty, buffer.Select(x => x ? '1' : '0'))}");
            
            var enumerator = buffer.GetEnumerator();
            while (ReadOnePacket(enumerator, ref versionSum)) { }
        }
        else if (lengthTypeId == 1)
        {
            var numSubPackets = Read11Bits(bitStream);
            Console.WriteLine($"  > This packet contains {numSubPackets} packets");

            for (var i = 0; i < numSubPackets; i++)
            {
                var read = ReadOnePacket(bitStream, ref versionSum);
                Debug.Assert(read);
            }
        }
    }

    return true;
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
            Console.WriteLine($"dbg: Failed to read bit {i+1}/3.");
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