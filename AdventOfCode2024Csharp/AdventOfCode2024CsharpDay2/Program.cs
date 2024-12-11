// See https://aka.ms/new-console-template for more information

var reports = GetReports("input.txt");
var howManyAreSafe = 0;
var howManyAreReallySafe = 0;
await foreach (var report in reports)
{
    if (IsReportSafe(report))
    {
        howManyAreSafe++;
    }

    if (IsReportReallySafe(report))
    {
        howManyAreReallySafe++;
    }
}

Console.WriteLine(howManyAreSafe);
Console.WriteLine(howManyAreReallySafe);


bool IsReportReallySafe(IEnumerable<int> report)
{
    if (IsReportSafe(report))
    {
        return true;
    }

    var reportList = report.ToList();
    for (int i = 0; i < reportList.Count; i++)
    {
        var modifiedReport = reportList.Take(i).Concat(reportList.Skip(i + 1));
        if (IsReportSafe(modifiedReport))
        {
            return true;
        }
    }

    return false;
}

bool IsReportSafe(IEnumerable<int> report)
{
    var order = Order.NOT_DETERMINED;
    var lastValue = report.First();
    foreach (var level in report.Skip(1))
    {
        var diff = lastValue - level;
        var absDiff = Math.Abs(diff);
        if (absDiff < 1 || absDiff > 3) return false;
        switch (order)
        {
            case Order.ASC:
                if (diff > 0) return false;
                break;
            case Order.DESC:
                if (diff < 0) return false;
                break;
            case Order.NOT_DETERMINED:
                order = diff > 0 ? Order.DESC : Order.ASC;
                break;
        }

        lastValue = level;
    }

    return true;
}

async IAsyncEnumerable<IEnumerable<int>> GetReports(string fileName)
{
    await foreach (var line in File.ReadLinesAsync(fileName))
    {
        if (!string.IsNullOrWhiteSpace(line))
            yield return line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
    }
}

enum Order
{
    ASC,
    DESC,
    NOT_DETERMINED
}