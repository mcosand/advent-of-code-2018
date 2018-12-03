using System;
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
      for (int i=0;i<1000;i++)
      {
        squares[i] = new int[1000];
      }

      foreach (var m in File.ReadAllLines("input.txt").Select(f => Regex.Match(f, "(\\d+),(\\d+): (\\d+)x(\\d+)")))
      {
        int x = int.Parse(m.Groups[1].Value);
        int y = int.Parse(m.Groups[2].Value);
        for (int h = 0; h < int.Parse(m.Groups[4].Value); h++)
        {
          for (int w = 0; w < int.Parse(m.Groups[3].Value); w++)
          {
            squares[x + w][y + h]++;
          }
        }


      }
      int result = squares.SelectMany(f => f).Count(g => g > 1);
      Console.WriteLine(result);
    }
  }
}
