using System.Diagnostics;

using var fs = File.OpenRead("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-4-1/input.txt");
using var reader = new StreamReader(fs);

var numbersToCall = Array.ConvertAll(reader.ReadLine()!.Split(','), static x => int.Parse(x));
Debug.Assert(reader.ReadLine() == string.Empty);

var boards = new List<Board>();

string line;
int[,]? board = null;
int i = 0;
while ((line = reader.ReadLine()) != null)
{
    if (line.Length == 0)
    {
        boards.Add(new Board(board!));
        board = null;
        continue;
    }

    var nums = Array.ConvertAll(line.Split(' ', StringSplitOptions.RemoveEmptyEntries), static x => int.Parse(x));

    if (board is null)
    {
        i = 0;
        board = new int[nums.Length, nums.Length];
    }

    ArrayCopyRank(nums, board, i);
    i++;
}

if (board is not null)
{
    boards.Add(new Board(board));
}

Board? lastBoardToWin = null;
var lastNumberCalled = 0;

foreach (var num in numbersToCall)
{
    if (boards.Count == 0)
    {
        break;
    }

    lastNumberCalled = num;

    foreach (var b in boards.ToArray())
    {
        if (b.Match(num))
        {
            lastBoardToWin = b;
            boards.Remove(b);
        }
    }
}

if (lastBoardToWin is null)
{
    Console.WriteLine("No matches (???)");
}
else
{
    Console.WriteLine($"First board to match sum * num = {lastBoardToWin.GetSumUnmatched() * lastNumberCalled}");
}

static void ArrayCopyRank<T>(T[] source, T[,] destination, int dimension)
{
    for (var i = 0; i < source.Length; i++)
    {
        destination[dimension, i] = source[i];
    }
}

class Board
{
    public Board(int[,] values)
    {
        this.values = values;
        matched = new bool[values.GetLength(0), values.GetLength(1)];
    }

    int[,] values;
    bool[,] matched;

    public bool Match(int value)
    {
        for (var i = 0; i < values.GetLength(0); i++)
        {
            for (var j = 0; j < values.GetLength(1); j++)
            {
                if (values[i, j] == value)
                {
                    matched[i, j] = true;
                    if (CheckBoard())
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool CheckBoard()
    {
        for (var i = 0; i < values.GetLength(0); i++)
        {
            if (CheckRow(i))
            {
                return true;
            }
        }

        for (var i = 0; i < values.GetLength(1); i++)
        {
            if (CheckColumn(i))
            {
                return true;
            }
        }

        return false;
    }

    bool CheckColumn(int index)
    {
        for (var i = 0; i < values.GetLength(0); i++)
        {
            if (!matched[i, index])
            {
                return false;
            }
        }

        return true;
    }

    bool CheckRow(int index)
    {
        for (var i = 0; i < values.GetLength(1); i++)
        {
            if (!matched[index, i])
            {
                return false;
            }
        }

        return true;
    }

    public int GetSumUnmatched()
    {
        var sum = 0;

        for (var i = 0; i < values.GetLength(0); i++)
        {
            for (var j = 0; j < values.GetLength(1); j++)
            {
                if (!matched[i, j])
                {
                    sum += values[i, j];
                }
            }
        }

        return sum;
    }
}