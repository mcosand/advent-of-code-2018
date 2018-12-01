using System;
using System.IO;
using System.Linq;

namespace day_01
{
  class Program
  {
    static void Main(string[] args)
    {
      int result = File.ReadAllLines("input.txt").Aggregate<String, int>(0, (accum, val) => accum += int.Parse(val));
      Console.WriteLine(result);

    }
  }
}
