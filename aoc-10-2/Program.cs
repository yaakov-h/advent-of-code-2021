var input = File.ReadLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-10-2/input.txt");

var scores = new List<long>();

foreach (var line in input)
{
    var result = GetHypotheticalClosingChars(line);
    if (result is null || result.Count == 0) continue;
    var score = GetScore(result);
    Console.WriteLine($"Line '{line}' is incomplete. To close, add: \"{string.Join(string.Empty, result)}\" with score {score}.");
    scores.Add(score);
}

if ((scores.Count % 2) != 1)
{
    throw new Exception("Should have an odd amount of lines. :/");
}

scores.Sort();
Console.WriteLine(string.Join(", ", scores));
var middleIndex = ((scores.Count - 1) / 2);
Console.WriteLine(scores[middleIndex]);

char GetClosingChar(char openingChar) => openingChar switch {
    '(' => ')',
    '[' => ']',
    '{' => '}',
    '<' => '>',
    _ => throw new NotSupportedException(),
};

long GetScore(List<char> chars)
{
    var score = 0L;

    foreach (var c in chars)
    {
        score *= 5;
        score += c switch {
            ')' => 1,
            ']' => 2,
            '}' => 3,
            '>' => 4,
            _ => throw new ArgumentException(),
        };
    }

    return score;
};

List<char>? GetHypotheticalClosingChars(string line)
{
    var state = new Stack<char>();

    foreach (var c in line)
    {
        switch (c)
        {
            case '(' or '[' or '{' or '<':
                state.Push(c);
                break;

            case ')' or ']' or '}' or '>':
                var expectedClosingChar = GetClosingChar(state.Pop());
                if (expectedClosingChar != c)
                {
                    return null;
                }
            break;

            default:
                throw new NotSupportedException();
        }
    }

    return state.Select(x => GetClosingChar(x)).ToList();
}
