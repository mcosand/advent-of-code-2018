﻿using System;
using System.Linq;
using System.IO;

namespace day_11
{
  class Program
  {
    static void Main(string[] args)
    {
      int gridSerial = 5153;
      int width = 300;
      int height = 300;

      int[][] grid = new int[height][];
      int[][] squares = new int[height][];
      Tuple<int, int, int> maxPower = new Tuple<int, int, int>(-100, 0, 0);

      for (var y = 299; y >= 0; y--)
      {
        grid[y] = new int[width];
        squares[y] = new int[width];

        for (var x = 299; x >= 0; x--)
        {
          int rack = x + 10;
          grid[y][x] = (((((rack * y) + gridSerial) * rack) / 100) % 10) - 5;

          if (x < 298 && y < 298)
          {
            for (var sx=0;sx<3;sx++)
            {
              for (var sy=0;sy<3;sy++)
              {
                squares[y][x] += grid[y + sy][x + sx];
              }
            }
            if (squares[y][x] > maxPower.Item1)
            {
              maxPower = new Tuple<int, int, int>(squares[y][x], x, y);
            }
          }
        }
      }
      string coord = $"{maxPower.Item2},{maxPower.Item3}";
      Console.WriteLine($"Best square at {coord}");
    }
  }
}
