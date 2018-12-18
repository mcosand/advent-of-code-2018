using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace day_17
{
  class Program
  {
    static readonly int OPEN = 0;
    static readonly int RUNNING = 1;
    static readonly int STILL = 2;
    static readonly int WALL = 3;

    static readonly Dictionary<int, char> drawMap = new Dictionary<int, char> { { OPEN, '.' }, { WALL, '#' }, { RUNNING, '|' }, { STILL, '~' } };
    
    private int[,] grid;
    private int minX = int.MaxValue;
    int minY = int.MaxValue;
    int capacity = 0;
    int atRest = 0;
    Dictionary<string, Segment> pendingSegments = new Dictionary<string, Segment>();

    static void Main(string[] args)
    {
      new Program().Run();
    }

    class Segment
    {
      public int Y;
      public int Left;
      public int Width;
      public bool Contained = false;
      public Segment Previous;
    }

     private void Run()
    {
      Console.WriteLine("Hit enter when window positioned");
      Console.ReadLine();
      Console.Clear();

      setupGrid();
      printGrid();
      Console.ForegroundColor = ConsoleColor.Cyan;

      Segment segment = new Segment { Left = 500 - minX, Y = 0, Width = 1, Previous = null };
      pendingSegments.Add(getKey(segment), segment);

      while (pendingSegments.Count > 0)
      {
        segment = pendingSegments.Values.First();
        pendingSegments.Remove(getKey(segment));
        percolate(segment);
      }

      Console.SetCursorPosition(0, grid.GetLength(1) + 1);
      Console.WriteLine($"Capacity: {capacity}, Contained: {atRest}");
    }

    private string getKey(Segment s)
    {
      return $"{s.Left}-{s.Y}";
    }

    int iterates = 0;

    private void percolate(Segment segment)
    {
      iterates++;
      if (grid[segment.Left, segment.Y] == STILL)
        return;

      MakeWater(segment.Left, segment.Y);
      if (segment.Y == grid.GetLength(1) - 1)
      {
        return;
      }

      if (grid[segment.Left, segment.Y + 1] == RUNNING) return;

      // ooze right
      while (grid[segment.Left + segment.Width - 1, segment.Y + 1] > RUNNING && grid[segment.Left + segment.Width, segment.Y] < STILL)
      {
        MakeWater(segment.Left + segment.Width, segment.Y);
        segment.Width++;
      }

      // ooze left
      while (grid[segment.Left, segment.Y + 1] > RUNNING && grid[segment.Left - 1, segment.Y] < STILL)
      {
        MakeWater(segment.Left - 1, segment.Y);
        segment.Left--;
        segment.Width++;
      }

      bool contained = true;

      if (grid[segment.Left + segment.Width - 1, segment.Y + 1] == OPEN)
      {
        contained = false;
        Segment next = new Segment { Left = segment.Left + segment.Width - 1, Y = segment.Y + 1, Previous = segment, Width = 1 };
        pendingSegments.TryAdd(getKey(next), next);
      }

      if (grid[segment.Left, segment.Y + 1] == OPEN && segment.Width > 1)
      {
        contained = false;
        Segment next = new Segment { Left = segment.Left, Y = segment.Y + 1, Previous = segment, Width = 1 };
        pendingSegments.TryAdd(getKey(next), next);
      }

      if (contained && !segment.Contained) MarkAsContained(segment);

      if (segment.Contained && segment.Previous != null)
      {
        pendingSegments.TryAdd(getKey(segment.Previous), segment.Previous);
      }
    }

    private void MakeWater(int x, int y)
    {
      if (grid[x, y] != OPEN) return;
      grid[x, y] = RUNNING;
      capacity++;
      Console.SetCursorPosition(x, y);
      Console.Write('|');
      pendingSegments.Remove($"{x}-{y}");
    }

    private void MarkAsContained(Segment s)
    {
      for (var x = 0; x < s.Width; x++)
      {
        grid[x + s.Left, s.Y] = STILL;
        pendingSegments.Remove($"{x + s.Left}-{s.Y}");
      }
      atRest += s.Width;
      Console.SetCursorPosition(s.Left, s.Y);
      Console.Write(new string('~', s.Width));
      s.Contained = true;
    }

    private void setupGrid()
    {
      int maxY = 0;
      int maxX = 0;
      string file = "sample.txt";
      file = "input.txt";
      foreach (var line in File.ReadAllLines(file))
      {
        var match = Regex.Match(line, "([xy])=(\\d+), [xy]=(\\d+)\\.\\.(\\d+)");
        int first = int.Parse(match.Groups[2].Value);
        int second = int.Parse(match.Groups[3].Value);
        int third = int.Parse(match.Groups[4].Value);

        if (match.Groups[1].Value == "x" && first > maxX) maxX = first;
        if (match.Groups[1].Value == "x" && first < minX) minX = first;
        if (match.Groups[1].Value == "y" && first > maxY) maxY = first;
        if (match.Groups[1].Value == "y" && second < minX) minX = second;
        if (match.Groups[1].Value == "x" && second < minY) minY = second;
        if (match.Groups[1].Value == "y" && third > maxX) maxX = third;
        if (match.Groups[1].Value == "x" && third > maxY) maxY = third;
      }

      // allow for spillage over the side
      minX--;
      maxX++;

      grid = new int[maxX - minX + 1, maxY - minY + 1];
      foreach (var line in File.ReadAllLines(file))
      {
        var match = Regex.Match(line, "([xy])=(\\d+), [xy]=(\\d+)\\.\\.(\\d+)");
        bool isCol = match.Groups[1].Value == "x";
        int first = int.Parse(match.Groups[2].Value);
        int second = int.Parse(match.Groups[3].Value);
        int third = int.Parse(match.Groups[4].Value);

        for (var i = second; i<= third; i++)
        {
          grid[(isCol ? first : i) - minX, (isCol ? i : first) - minY] = WALL;
        }
      }
    }

    private void printGrid()
    {
      Console.ForegroundColor = ConsoleColor.Gray;
      var color = Console.ForegroundColor;
      for (var y = 0; y < grid.GetLength(1); y++)
      {
        StringBuilder sb = new StringBuilder();
        for (var x = 0; x < grid.GetLength(0); x++)
        {
          sb.Append(drawMap[grid[x, y]]);
        }
        Console.WriteLine(sb);
      }
    }
  }
}
