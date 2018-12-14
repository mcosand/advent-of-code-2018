using System;
using System.Collections.Generic;
using System.Linq;

namespace day_14
{
  class Program
  {
    static void Main(string[] args)
    {
      string input = "51589";
      input = "157901";

      List<int> scores = new List<int>(new[] { 3, 7 } );
      int elf1 = 0;
      int elf2 = 1;

      bool found = false;
      while (!found)
      {
        int start = Math.Max(0, scores.Count - input.Length);
        int sum = scores[elf1] + scores[elf2];
        scores.AddRange(sum.ToString().Select(c => (int)(c - '0')));
        elf1 = (elf1 + 1 + scores[elf1]) % scores.Count;
        elf2 = (elf2 + 1 + scores[elf2]) % scores.Count;

        string search = string.Join("", scores.Skip(start));
        int offset = search.IndexOf(input);
        if (offset >= 0) {
          int answer = start + offset;
          Console.WriteLine(answer);
          return;
        }

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

    }

  }
}
