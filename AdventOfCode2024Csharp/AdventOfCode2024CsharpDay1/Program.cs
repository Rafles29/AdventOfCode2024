// See https://aka.ms/new-console-template for more information

var fileName = "input.txt";

var listA = new List<int>();
var listB = new List<int>();

await foreach (var line in File.ReadLinesAsync(fileName))
{
    var splitLine = line.Trim().Split(' ');
    
    listA.Add(int.Parse(splitLine.First()));
    listB.Add(int.Parse(splitLine.Last()));
}

listA.Sort();
listB.Sort();

var ans1 = listA.Zip(listB).Aggregate(0, (sum, next) => sum += Math.Abs(next.First - next.Second));
Console.WriteLine(ans1);

var ans2 = listA.GroupJoin(listB, a => a, b => b, (id, list) => id * list.Count()).Sum();
Console.WriteLine(ans2);