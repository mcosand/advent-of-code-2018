using System;
using System.IO;
using System.Linq;

namespace day_02
{
  class Program
  {
    static void Main(string[] args)
    {
      string[] lines = File.ReadAllLines("input.txt");
      for (var i=0;i<lines[0].Length; i++)
      {
        var result = lines.Select(f => f.Substring(0, i) + (i < f.Length - 1 ? f.Substring(i + 1, f.Length - 1 - i) : "")).GroupBy(f => f).Where(f => f.Count() > 1).Select(f => f.Key);
        if (result.Count() != 0)
        {
          Console.WriteLine(result.ToArray()[0]);
        }
      }
    }
  }
}
