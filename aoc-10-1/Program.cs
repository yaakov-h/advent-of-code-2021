var input = File.ReadLines("/Users/yaakov/Developer/Miniprojects/advent-of-code/aoc-10-1/input.txt");

var score = 0;
foreach (var line in input)
{
    var result = GetFirstInvalidClosingChar(line);
    if (result is null) continue;
    var (expected, found) = result.Value;
    Console.WriteLine($"Line '{line}' is corrupt. Expected '{expected}' but found '{found}'.");
    score += GetScore(found);
}
Console.WriteLine(score);

char GetClosingChar(char openingChar) => openingChar switch {
    '(' => ')',
    '[' => ']',
    '{' => '}',
    '<' => '>',
    _ => throw new NotSupportedException(),
};

int GetScore(char illegalChar) => illegalChar switch {
    ')' => 3,
    ']' => 57,
    '}' => 1197,
    '>' => 25137,
    _ => throw new NotSupportedException(),
};

(char expected, char found)? GetFirstInvalidClosingChar(string line)
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
                    return (expectedClosingChar, c);
                }
            break;

            default:
                throw new NotSupportedException();
        }
    }

    return null;
}
