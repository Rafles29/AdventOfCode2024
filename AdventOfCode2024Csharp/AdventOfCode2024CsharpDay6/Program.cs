// See https://aka.ms/new-console-template for more information

using Map = char[][];
using MapVector = (int row, int column);

var lines = await File.ReadAllLinesAsync("input.txt");
var map = lines.Select(x => x.ToArray()).ToArray();

TraceSteps(map);
var count = map.SelectMany(x => x).Where(x => x == 'x').Select(x => 1).Sum();
Console.WriteLine(count);

static void TraceSteps(Map map)
{
    // Directions mapped to row/column deltas
    var directions = new Dictionary<char, (int dRow, int dCol)>
    {
        { '^', (-1, 0) },
        { '>', (0, 1) },
        { 'v', (1, 0) },
        { '<', (0, -1) },
    };

    // Turning right: '^' -> '>', '>' -> 'v', 'v' -> '<', '<' -> '^'
    var directionChanges = new Dictionary<char, char>()
    {
        { '^', '>' },
        { '>', 'v' },
        { 'v', '<' },
        { '<', '^' },
    };

    char obstacle = '#';
    char visited = 'x';

    var guardPosition = FindGuard(map, directions.Keys.ToList())
                        ?? throw new ArgumentException("No guard found on the map.");
    var guardDirection = map[guardPosition.row][guardPosition.column];

    int rows = map.Length;
    int columns = map[0].Length;
    
    // important!
    map[guardPosition.row][guardPosition.column] = visited;

    while (true)
    {
        var (dRow, dCol) = directions[guardDirection];
        var nextPos = (row: guardPosition.row + dRow, column: guardPosition.column + dCol);

        if (nextPos.row < 0 || nextPos.row >= rows || nextPos.column < 0 || nextPos.column >= columns)
        {
            break;
        }

        if (map[nextPos.row][nextPos.column] == obstacle)
        {
            guardDirection = directionChanges[guardDirection];
            continue;
        }
        
        guardPosition = nextPos;
        
        char currentCell = map[guardPosition.row][guardPosition.column];
        if (currentCell != obstacle && currentCell != visited)
        {
            map[guardPosition.row][guardPosition.column] = visited;
        }
    }
}

static MapVector? FindGuard(Map map, List<char> possibleSigns)
{
    for (int row = 0; row < map.Length; row++)
    {
        for (int column = 0; column < map[row].Length; column++)
        {
            if (possibleSigns.Contains(map[row][column]))
            {
                return new MapVector(row, column);
            }
        }
    }

    return default;
}