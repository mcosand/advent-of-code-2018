using System;
using System.Collections.Generic;
using System.Linq;

namespace day_14
{
  class Program
  {
    static void Main(string[] args)
    {
      int input = 157901;

      List<int> scores = new List<int>(new[] { 3, 7 } );
      int elf1 = 0;
      int elf2 = 1;

      while (scores.Count < input + 10)
      {
        int sum = scores[elf1] + scores[elf2];
        scores.AddRange(sum.ToString().Select(c => (int)(c - '0')));
        elf1 = (elf1 + 1 + scores[elf1]) % scores.Count;
        elf2 = (elf2 + 1 + scores[elf2]) % scores.Count;

        //var color = Console.ForegroundColor;
        //for (int i=0;i<scores.Count; i++)
        //{
        //  if (i == elf1) Console.ForegroundColor = ConsoleColor.Magenta;
        //  else if (i == elf2) Console.ForegroundColor = ConsoleColor.Green;
        //  else Console.ForegroundColor = color;
        //  Console.Write(scores[i] + " ");
        //}
        //Console.ForegroundColor = color;
        //Console.WriteLine();

      }

      string answer = string.Join("", scores.Skip(input).Take(10));
      Console.WriteLine(answer);
    }

  }
}
