using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_16
{
  class Program
  {
    static void Main(string[] args)
    {
      new Program().Run();
    }

    readonly int[] registers = new int[4];
    Dictionary<string, Action<int, int, int>> instructions;
    Dictionary<int, string> opcodeNames = new Dictionary<int, string>();

    private void Run()
    {
      instructions = new Dictionary<string, Action<int, int, int>>
      {
        { "addr", (a, b, c) => registers[c] = registers[a] + registers[b] },
        { "addi", (a, b, c) => registers[c] = registers[a] + b },
        { "mulr", (a, b, c) => registers[c] = registers[a] * registers[b] },
        { "muli", (a, b, c) => registers[c] = registers[a] * b },
        { "banr", (a, b, c) => registers[c] = registers[a] & registers[b] },
        { "bani", (a, b, c) => registers[c] = registers[a] & b },
        { "borr", (a, b, c) => registers[c] = registers[a] | registers[b] },
        { "bori", (a, b, c) => registers[c] = registers[a] | b },
        { "setr", (a, b, c) => registers[c] = registers[a] },
        { "seti", (a, b, c) => registers[c] = a },
        { "gtir", (a, b, c) => registers[c] = a > registers[b] ? 1 : 0 },
        { "gtri", (a, b, c) => registers[c] = registers[a] > b ? 1 : 0 },
        { "gtrr", (a, b, c) => registers[c] = registers[a] > registers[b] ? 1 : 0 },
        { "eqir", (a, b, c) => registers[c] = a == registers[b] ? 1 : 0 },
        { "eqri", (a, b, c) => registers[c] = registers[a] == b ? 1 : 0 },
        { "eqrr", (a, b, c) => registers[c] = registers[a] == registers[b] ? 1 : 0 },
      };

      var input = "Before: [3, 2, 1, 1]\n9 2 1 2\nAfter:  [3, 2, 2, 1]\n\n".Split("\n");
      input = File.ReadAllLines("input.txt");
      string[] lines = new string[4];

      int confusing = 0;

      Dictionary<string, Action<int, int, int>> unknownInstructions = new Dictionary<string, Action<int, int, int>>(instructions);
      while (unknownInstructions.Count > 0)
      {
        int i = 0;
        for ( ; i < input.Length; i++)
        {
          lines[i % 4] = input[i];
          if (i % 4 == 3)
          {
            if (lines[0].StartsWith("Before") == false) break;
            int[] before = lines[0].Substring(9, lines[0].Length - 10).Split(',').Select(f => int.Parse(f)).ToArray();
            int[] after = lines[2].Substring(9, lines[2].Length - 10).Split(',').Select(f => int.Parse(f)).ToArray();

            int[] instruction = lines[1].Split(" ").Select(f => int.Parse(f)).ToArray();

            // We know this instruction
            if (opcodeNames.ContainsKey(instruction[0])) continue;
            
            // We don't know this instruction. Test it.
            var matches = unknownInstructions.Where(f =>
            {
              for (int j = 0; j < 4; j++)
              {
                registers[j] = before[j];
              }

              f.Value(instruction[1], instruction[2], instruction[3]);

              for (int j = 0; j < 4; j++)
              {
                if (registers[j] != after[j]) return false;
              }

              return true;
            }).ToList();

            if (matches.Count == 1)
            {
              opcodeNames.Add(instruction[0], matches[0].Key);
              unknownInstructions.Remove(matches[0].Key);
            }
          }
        }

        // reset
        for (int j=0;j<registers.Length;j++) { registers[j] = 0; }

        for (i--; i < input.Length; i++)
        {
          var line = input[i];
          int[] instruction = line.Split(" ").Select(f => int.Parse(f)).ToArray();
          instructions[opcodeNames[instruction[0]]](instruction[1], instruction[2], instruction[3]);
        }
      }

      var answer = registers[0];
      Console.WriteLine(answer);
    }
  }
}
