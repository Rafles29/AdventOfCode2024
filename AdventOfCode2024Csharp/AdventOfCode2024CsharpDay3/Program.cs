// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var ans1 = GetMul("input.txt").ToBlockingEnumerable().Select(x => x.Item1 * x.Item2).Sum();
var ans2 = GetMul2("input.txt").ToBlockingEnumerable().Select(x => x.Item1 * x.Item2).Sum();
Console.WriteLine(ans1);
Console.WriteLine(ans2);

async IAsyncEnumerable<(int, int)> GetMul(string fileName)
{
    var regexPatern = "mul\\(\\d{1,3},\\d{1,3}\\)";
    await foreach (var line in File.ReadLinesAsync(fileName))
    {
        var matches = Regex.Matches(line, regexPatern);
        foreach (var match in matches.ToList())
        {
            var numbers = match.Value
                .Remove(0, 4)
                .Replace(")", "")
                .Split(",")
                .Select(int.Parse);

            yield return (numbers.First(), numbers.Last());
        }
    }
}

async IAsyncEnumerable<(int, int)> GetMul2(string fileName)
{
    var regexPatern = "mul\\(\\d{1,3},\\d{1,3}\\)|do\\(\\)|don't\\(\\)";
    var enabled = true;
    await foreach (var line in File.ReadLinesAsync(fileName))
    {
        var matches = Regex.Matches(line, regexPatern);
        foreach (var match in matches.ToList())
        {
            var value = match.Value;
            if (value.StartsWith("do()"))
            {
                enabled = true;
                continue;
            }

            if (value.StartsWith("don't"))
            {
                enabled = false;
                continue;
            }

            if (!enabled)
            {
                continue;
            }
            
            var numbers = value
                .Remove(0, 4)
                .Replace(")", "")
                .Split(",")
                .Select(int.Parse);

            yield return (numbers.First(), numbers.Last());
        }
    }
}