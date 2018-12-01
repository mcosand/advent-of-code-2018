using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_01
{
  class Program
  {
    static void Main(string[] args)
    {
      Dictionary<int, int> hash = new Dictionary<int, int>();
      int current = 0;
      int index = 0;

      int[] values = File.ReadAllLines("input.txt").Select(f => int.Parse(f)).ToArray();

      while (!hash.TryGetValue(current, out int dummy))
      {
        hash.Add(current, 1);
        current += values[index];
        index = (index + 1) % values.Length;
      }
      
      Console.WriteLine(current);

    }
  }
}
