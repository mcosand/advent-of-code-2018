using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace day_03
{
  class Program
  {
    static void Main(string[] args)
    {
      int[][] squares = new int[1000][];
      Dictionary<int, bool> nonconflicts = new Dictionary<int, bool>();

      for (int i=0;i<1000;i++)
      {
        squares[i] = new int[1000];
      }


      foreach (var m in File.ReadAllLines("input.txt").Select(f => Regex.Match(f, "(\\d+) @ (\\d+),(\\d+): (\\d+)x(\\d+)")))
      {
        int id = int.Parse(m.Groups[1].Value);
        int x = int.Parse(m.Groups[2].Value);
        int y = int.Parse(m.Groups[3].Value);
        bool conflicts = false;
        for (int h = 0; h < int.Parse(m.Groups[5].Value); h++)
        {
          for (int w = 0; w < int.Parse(m.Groups[4].Value); w++)
          {
            int oldclaim = squares[x + w][y + h];
            if (oldclaim == 0)
            {

            }
            else
            {
              conflicts = true;

              if (nonconflicts.ContainsKey(oldclaim))
              {
                conflicts = true;
                nonconflicts.Remove(oldclaim);
              }
            }
            squares[x + w][y + h] = id;
          }
        }

        if (!conflicts) nonconflicts.Add(id, true);

      }
      var result = nonconflicts.Keys;
      Console.WriteLine(result);
    }
  }
}
