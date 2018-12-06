using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_06
{
  class Program
  {
    List<Tuple<int, int>> location = new List<Tuple<int, int>>();

    int[] sizes;
    int minx = int.MaxValue;
    int miny = int.MaxValue;
    int maxx = int.MinValue;
    int maxy = int.MinValue;

    static void Main(string[] args)
    {
      new Program().run();
    }

    int[][] field;

    void run() {
      var input = new string[] { "1,1", "1,6", "8,3", "3,4", "5,5", "8,9" };
      input = File.ReadAllLines("input.txt");

      foreach (var line in input)
      {
        var parts = line.Split(",").Select(f => int.Parse(f.Trim())).ToArray();
        location.Add(new Tuple<int, int>(parts[0], parts[1]));
        if (parts[0] < minx) minx = parts[0];
        if (parts[0] > maxx) maxx = parts[0];
        if (parts[1] < miny) miny = parts[1];
        if (parts[1] > maxy) maxy = parts[1];
      }
      sizes = new int[location.Count()];

      field = new int[maxy + 1][];
      for (var i = 0; i < field.Length; i++)
      {
        field[i] = new int[maxx + 1];
        Array.Fill(field[i], -1);
      }
      for (var i = 0; i < location.Count(); i++)
      {
        field[location[i].Item2][location[i].Item1] = 10000 + i;
      }

      int dist = 0;
      bool growth;
      do
      {
        growth = false;
        dist++;

        Dictionary<string, Tuple<int, int, int>> claims = new Dictionary<string, Tuple<int, int, int>>();

        for (var d = 0; d < dist; d++)
        {
          checkClaims(claims, d, d - dist);
          checkClaims(claims, dist - d, d);
          checkClaims(claims, -d, dist - d);
          checkClaims(claims, d - dist, -d);
        }


        foreach (var claim in claims.Values)
        {
          if (claim.Item3 >= 0)
          {
            field[claim.Item2][claim.Item1] = claim.Item3;

            if (claim.Item1 == minx || claim.Item1 == maxx || claim.Item2 == miny || claim.Item2 == maxy)
            {
              sizes[claim.Item3] = int.MinValue;
              continue;
            }

            sizes[claim.Item3]++;
            growth = true;
          }
          else
          {
            field[claim.Item2][claim.Item1] = -2;
          }
        }

   //     print();
      } while (growth);

      var big = sizes.Select((f, i) => new { i = i, v = f + 1 }).OrderByDescending(f => f.v).First();
      Console.WriteLine(big.v);
    }

    void checkClaims(Dictionary<string, Tuple<int, int, int>> claims, int dx, int dy)
    {
      for (var i = 0; i < location.Count; i++)
      {
        int x = location[i].Item1 + dx;
        int y = location[i].Item2 + dy;

        if (x < minx || x > maxx || y < miny || y > maxy) { continue; }

        int o = field[y][x];
        // if it's not claimed by a shorter distance
        if (o == -1)
        {
          // see if someone's trying to claim it this round
          if (claims.TryGetValue($"{x}-{y}", out Tuple<int, int, int> claim))
          {
            // If someone's trying to claim it, invalidate the claim
            claims[$"{x}-{y}"] = new Tuple<int, int, int>(x, y, int.MinValue);
          }
          else
          {
            claims.Add($"{x}-{y}", new Tuple<int, int, int>(x, y, i));
          }
        }
      }
    }
    
    void print()
    {
      Console.SetCursorPosition(0, 0);
      for (int y=miny; y<= maxy; y++)
      {
        for (int x=minx; x <= maxx; x++)
        {
          int v = field[y][x];
          Console.Write(v == -1 ? '.' : v == -2 ? 'x' : v >= 10000 ? (char)('A' + v - 10000) : (char)('a' + v));
        }
        Console.WriteLine();
      }
    }
  }
}
