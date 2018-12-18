using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day_18
{
  class Program
  {
    static void Main(string[] args)
    {
      new Program().Run();
    }

    enum Land
    {
      Open,
      Trees,
      Yard
    }

    Land[,] area;

    private void Run()
    {
      int size = 10;
      string file = "sample.txt";

      size = 50;
      file = "input.txt";

      area = new Land[size, size];

      var lines = File.ReadAllLines(file);
      for (int y = 0; y < lines.Length; y++)
      {
        var line = lines[y];
        for (int x = 0; x < line.Length; x++)
        {
          area[x, y] = line[x] == '.' ? Land.Open : line[x] == '|' ? Land.Trees : Land.Yard;
        }
      }
      print(0);
      for (int i=0; i<10; i++)
      {
        Land[,] newArea = new Land[area.GetLength(0), area.GetLength(1)];
        for (int y = 0; y < area.GetLength(1); y++)
        {
          for (int x = 0; x < area.GetLength(0); x++)
          {
            newArea[x, y] = area[x, y];
            TryMutate(x, y, Land.Open, new Dictionary<Land, int> { { Land.Trees, 3 } }, Land.Trees, Land.Open, newArea);
            TryMutate(x, y, Land.Trees, new Dictionary<Land, int> { { Land.Yard, 3 } }, Land.Yard, Land.Trees, newArea);
            TryMutate(x, y, Land.Yard, new Dictionary<Land, int> { { Land.Yard, 1 }, { Land.Trees, 1 } }, Land.Yard, Land.Open, newArea);
          }
        }
        area = newArea;

        print(i);
      }

      int treeCount = 0;
      int woodCount = 0;
      for (int y = 0; y < area.GetLength(1); y++)
      {
        for (int x = 0; x < area.GetLength(0); x++)
        {
          treeCount += area[x, y] == Land.Trees ? 1 : 0;
          woodCount += area[x, y] == Land.Yard ? 1 : 0;
        }
      }
      int answer = treeCount * woodCount;
    }

    private void TryMutate(int x, int y, Land from, Dictionary<Land, int> test, Land onMatch, Land otherwise, Land[,] newArea)
    {
      if (area[x, y] != from) return;
      newArea[x, y] = CheckNeighbors(x, y, test) ? onMatch : otherwise;
    }

    private void print(int i)
    {
      Console.WriteLine(i);
      for (var y = 0; y < area.GetLength(1); y++)
      {
        StringBuilder sb = new StringBuilder();
        for (var x = 0; x < area.GetLength(0); x++)
        {
          sb.Append(area[x, y] == Land.Open ? '.' : area[x, y] == Land.Trees ? '|' : '#');
        }
        Console.WriteLine(sb);
      }
    }

    private bool CheckNeighbors(int x, int y, Dictionary<Land,int> seeking)
    {

      for (int dy=-1;dy<2;dy++)
      {
        for (int dx=-1;dx<2;dx++)
        {
          if (dx == 0 && dy == 0) continue;
          if (x + dx < 0) continue;
          if (x + dx >= area.GetLength(0)) continue;
          if (y + dy < 0) continue;
          if (y + dy >= area.GetLength(1)) continue;

          if (seeking.TryGetValue(area[x + dx,y + dy], out int needed))
          {
            seeking[area[x + dx, y + dy]]--;
          }

        }
      }
      bool matches = !seeking.Any(f => f.Value > 0);
      return matches;
    }
  }
}
