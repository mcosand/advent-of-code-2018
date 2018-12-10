using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day_10
{
  class Program
  {
    class Point
    {
      public int x;
      public int y;
      public int vx;
      public int vy;
      public int x0;
      public int y0;

      public Point(int x0, int y0, int vx, int vy)
      {
        x = this.x0 = x0;
        y = this.y0 = y0;
        this.vx = vx;
        this.vy = vy;
      }

      public override string ToString()
      {
        return $"<{x},{y}>  v=<{vx},{vy}>";
      }
    }

    static void Main(string[] args)
    {
      string file = "sample.txt";
      file = "input.txt";

      Point[] points = File.ReadAllLines(file)
        .Where(f => !string.IsNullOrWhiteSpace(f))
        .Select(line => Regex.Match(line, "\\< *(-?\\d+), *(-?\\d+)\\> velocity=\\< *(-?\\d+), *(-?\\d+)\\>"))
        .Select(m => new Point(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value)))
        .ToArray();

      int minSingles = int.MaxValue;
      int minTime = 0;
      int t = 0;

      while(true)
      {
        foreach (var p in points)
        {
          p.x += p.vx;
          p.y += p.vy;
        }
        t++;

        int c = countSingles(points);
        if (c == 0) break;
        if (c < minSingles)
        {
          minSingles = c;
          minTime = t;
          Console.WriteLine($"t: {t}, singles: {minSingles}");
        }
        if (t % 1000 == 0) Console.WriteLine(t);
      }

      int minx = points.Min(f => f.x);
      int miny = points.Min(f => f.y);

      Console.SetBufferSize(Math.Max(Console.WindowWidth, points.Max(f => f.x) - minx + 3), Math.Max(Console.WindowHeight, points.Max(f => f.y) + 3));
      Console.Clear();
      foreach (var p in points)
      {
        Console.SetCursorPosition(p.x - minx, p.y - miny);
        Console.Write('#');
      }
    }

    static int countSingles(Point[] points)
    {
      int count = 0;
      foreach (var p in points)
      {
        if (!points.Any(f => Math.Abs(p.x - f.x) < 2 && Math.Abs(p.y - f.y) < 2 && !(p.x == f.x && p.y == f.y))) {
          count++;
        }
      }
      return count;
    }

  }
}
