using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_02
{
  class Program
  {
    static void Main(string[] args)
    {
      string[] lines = File.ReadAllLines("input.txt");
      int count2 = 0;
      int count3 = 0;
      for (int i = 0; i < lines.Length; i++)
      {
        var list = makeList();
        for (var j = 0; j<lines[i].Length; j++)
        {
          list[lines[i][j]]++;
        }
        if (list.Any(f => f.Value == 2)) count2++;
        if (list.Any(f => f.Value == 3)) count3++;

      }

      Console.WriteLine(count2 * count3);
    }

    private static Dictionary<char, int> makeList()
    {
      Dictionary<char, int> list = new Dictionary<char, int>();
      for (char a = 'a'; a <= 'z'; a++)
      {
        list.Add(a, 0);
      }
      return list;
    }
  }
}
