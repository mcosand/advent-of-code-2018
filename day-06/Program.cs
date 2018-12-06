using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_06
{
  class Program
  {
    List<Tuple<int, int>> location = new List<Tuple<int, int>>();

    static void Main(string[] args)
    {
      new Program().run();
    }


    void run() {
      var print = true;
      var limit = 32;
      var input = new string[] { "1,1", "1,6", "8,3", "3,4", "5,5", "8,9" };

      print = false;
      limit = 10000;
      input = File.ReadAllLines("input.txt");

      foreach (var line in input)
      {
        var parts = line.Split(",").Select(f => int.Parse(f.Trim())).ToArray();
        location.Add(new Tuple<int, int>(parts[0], parts[1]));
      }

      List<Tuple<int, int>> wavefront = new List<Tuple<int, int>>();
      List<Tuple<int, int>> nextWave = new List<Tuple<int, int>>();

      wavefront.Add(new Tuple<int, int>((int)location.Select(f => f.Item1).Average(), (int)location.Select(f => f.Item2).Average()));
      Dictionary<string, bool> hits = new Dictionary<string, bool>();
      hits.Add($"{wavefront[0].Item1}-{wavefront[0].Item2}", true);
      showPoint(print, wavefront[0].Item2, wavefront[0].Item2);

      do
      {
        nextWave.Clear();
        foreach (var photon in wavefront)
        {
          for (int dx = -1; dx < 2; dx++)
          {
            for (int dy = -1; dy < 2; dy++)
            {
              if (dx == 0 && dy == 0) continue;

              int x = photon.Item1 + dx;
              int y = photon.Item2 + dy;

              string key = $"{x}-{y}";
              if (!hits.ContainsKey(key) && location.Select(f => Math.Abs(f.Item1 - x) + Math.Abs(f.Item2 - y)).Sum() < limit)
              {
                showPoint(print, x, y);
                hits.Add(key, true);
                nextWave.Add(new Tuple<int, int>(photon.Item1 + dx, photon.Item2 + dy));
              }
            }
          }
        }

        wavefront = nextWave.ToList();
      } while (nextWave.Count > 0);

      Console.WriteLine(hits.Count);
    }

    private static void showPoint(bool print, int x, int y)
    {
      if (print) { Console.SetCursorPosition(x, y); Console.Write('#'); }
    }
  }
}
