using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day_04
{
  class Program
  {
    static void Main(string[] args)
    {
      Dictionary<string, int[]> guardDownTimes = new Dictionary<string, int[]>();

      string[] lines = File.ReadAllLines("input.txt").OrderBy(f => f).ToArray();

      string guardId = null;
      int? sleepTime = null;
      foreach (var line in lines)
      {
        Match m = Regex.Match(line, "Guard #(\\d+) begins");
        if (m.Success)
        {
          if (sleepTime.HasValue)
          {
            markTime(guardDownTimes, guardId, sleepTime.Value, 60);
          }
          guardId = m.Groups[1].Value;
          if (!guardDownTimes.ContainsKey(guardId)) guardDownTimes.Add(guardId, new int[61]);
          continue;
        }

        m = Regex.Match(line, "00:(\\d\\d)] (w|f)");
        if (m.Success == false) throw new InvalidOperationException();

        if (m.Groups[2].Value == "f") {
          sleepTime = int.Parse(m.Groups[1].Value);
        }
        else
        {
          markTime(guardDownTimes, guardId, sleepTime.Value, int.Parse(m.Groups[1].Value));
          sleepTime = null;
        }
      }

      var sleepiest = guardDownTimes.OrderByDescending(f => f.Value[60]).First();
      var enumerable = sleepiest.Value.Take(60).Select((f, i) => new { i = i, v = f });
      var orderedEnumerable = enumerable.OrderByDescending(f => f.v);
      int goodTime = orderedEnumerable.Select(f => f.i).First();
      var result = int.Parse(sleepiest.Key) * goodTime;
      Console.WriteLine(result);
    }

    private static void markTime(Dictionary<string,int[]> guardDownTimes, string guardId, int sleep, int end)
    {
      guardDownTimes[guardId][60] += end - sleep;
      for (var i = sleep; i<end; i++)
      {
        guardDownTimes[guardId][i]++;
      }
    }
  }
}
