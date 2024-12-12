// See https://aka.ms/new-console-template for more information

var rules = await GetRules("rules.txt");
var updates = await GetUpdates("updates.txt");

var correctUpdates = updates.Where(update => AreRulesCorrect(update, rules));
var sum1 = correctUpdates.Select(GetMiddlePage).Sum();
Console.WriteLine(sum1);

var incorrectUpdates = updates.Where(update => !AreRulesCorrect(update, rules));
Console.WriteLine(incorrectUpdates.Select(update => CorrectUpdateOrder(update, rules)).Select(GetMiddlePage).Sum());


async static Task<List<(int, int)>> GetRules(string fileName)
{
    var allLines = await File.ReadAllLinesAsync(fileName);
    return allLines.Select(x =>
    {
        var splitted = x.Split('|');
        return (int.Parse(splitted.First()), int.Parse(splitted.Last()));
    }).ToList();
}

async static Task<List<List<int>>> GetUpdates(string fileName)
{
    var allLines = await File.ReadAllLinesAsync(fileName);
    return allLines.Select(x => x.Split(',').Select(int.Parse).ToList()).ToList();
}

static bool AreRulesCorrect(List<int> pages, List<(int, int)> rules)
{
    return rules.All(rule => IsRuleCorrect(pages, rule.Item1, rule.Item2));
}

static bool IsRuleCorrect(List<int> pages, int first, int last)
{
    if (!pages.Contains(first) || !pages.Contains(last)) return true;
    return pages.FindIndex(x => x == first) < pages.FindIndex(x => x == last);
}

static int GetMiddlePage(List<int> pages)
{
    return pages.ElementAt(pages.Count / 2);
}

// it's really hard we need to implement topological sorting 
static List<int> CorrectUpdateOrder(List<int> update, List<(int, int)> rules)
{
    var pagesSet = update.ToHashSet();
    var relevantRules = rules.Where(rule => pagesSet.Contains(rule.Item1) && pagesSet.Contains(rule.Item2)).ToList();

    if (!relevantRules.Any())
    {
        return update;
    }
    var adjacency = new Dictionary<int, List<int>>();
    var inDegree = new Dictionary<int, int>();
    
    foreach (var p in update)
    {
        adjacency[p] = new List<int>();
        inDegree[p] = 0;
    }
    
    foreach (var (first, last) in relevantRules)
    {
        adjacency[first].Add(last);
        inDegree[last]++;
    }

    var originalIndex = update
        .Select((page, index) => (page, index))
        .ToDictionary(x => x.page, x => x.index);

    var zeroInDegree = inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key).ToList();
    zeroInDegree.Sort((a, b) => originalIndex[a].CompareTo(originalIndex[b]));

    var sortedOrder = new List<int>();

    while (zeroInDegree.Count > 0)
    {
        var current = zeroInDegree[0];
        zeroInDegree.RemoveAt(0);
        sortedOrder.Add(current);
        
        foreach (var neighbor in adjacency[current])
        {
            inDegree[neighbor]--;
            if (inDegree[neighbor] == 0)
            {
                zeroInDegree.Add(neighbor);
            }
        }
        
        zeroInDegree.Sort((a, b) => originalIndex[a].CompareTo(originalIndex[b]));
    }
    
    if (sortedOrder.Count == update.Count)
    {
        return sortedOrder;
    }
    return update;
}