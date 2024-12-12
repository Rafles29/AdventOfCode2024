// See https://aka.ms/new-console-template for more information

var lines = await File.ReadAllLinesAsync("input.txt");
var chars = lines.Select(x => x.ToArray()).ToArray();

Console.WriteLine(CountWordOccurrences(chars, "XMAS"));
Console.WriteLine(CountXMas(chars));

static int CountWordOccurrences(char[][] grid, string word)
{
    int rows = grid.Length;
    int cols = grid[0].Length;
    int wordLength = word.Length;
    int count = 0;

    // all 8 posible direction to go left, right, up, down, all for diagonals
    int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
    int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

    for (int r = 0; r < rows; r++)
    {
        for (int c = 0; c < cols; c++)
        {
            // you try to go from every point in every direction
            for (int dir = 0; dir < 8; dir++)
            {
                int nr = r;
                int nc = c;
                int index = 0;

                // while you are not out of bound and letters correspond to given word
                while (index < wordLength &&
                       nr >= 0 && nr < rows &&
                       nc >= 0 && nc < cols &&
                       grid[nr][nc] == word[index])
                {
                    nr += dx[dir];
                    nc += dy[dir];
                    index++;
                }

                // if it matches it means that you went through all the characters
                // and every one was the same in the same order
                if (index == wordLength)
                {
                    count++;
                }
            }
        }
    }

    return count;
}

static int CountXMas(char[][] grid)
{
    int rows = grid.Length;
    int cols = grid[0].Length;
    int count = 0;
    
    // we are staring from the middle so we can skip first/last row/column
    for (int r = 1; r < rows-1; r++)
    {
        for (int c = 1; c < cols-1; c++)
        {
            if (CheckDiagonalsForMas(grid, r, c))
            {
                count++;
            }

        }
    }

    return count;
}

static bool CheckDiagonalsForMas(char[][] grid, int r, int c)
{
    // helpers
    char topLeft = grid[r - 1][c - 1];
    char topRight = grid[r - 1][c + 1];
    char bottemLeft = grid[r + 1][c - 1];
    char bottemRight = grid[r + 1][c + 1];
    char middle = grid[r][c];
    
    // checks both diagonals
    return IsMassequence(topLeft, middle, bottemRight) && IsMassequence(bottemLeft, middle, topRight);
}

static bool IsMassequence(char first, char middle, char last)
{
    if (middle != 'A') return false;
    return (first == 'M' && last == 'S') || (first == 'S' && last == 'M');
}