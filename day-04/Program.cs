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

      string maxGuard = null;
      int bigCount = 0;
      var maxMinute = 0;
      foreach (var guard in guardDownTimes)
      {
        for (int i=0;i<60;i++)
        {
          if (guard.Value[i] > bigCount) {
            maxGuard = guard.Key;
            bigCount = guard.Value[i];
            maxMinute = i;
          }
        }
      }

      var result = int.Parse(maxGuard) * maxMinute;
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
