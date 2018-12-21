using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_21
{
  class Program
  {
    static void Main(string[] args)
    {
      new Program().Run();
    }

    readonly int registerCount = 6;
    int[] registers;
    Dictionary<string, Action<int, int, int>> instructions;
    int instructionPointer = 0;
    int ipBinding = 0;

    private void Run()
    {
      registers = new int[registerCount];
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

      string[] input = "#ip 0\nseti 5 0 1\nseti 6 0 2\naddi 0 1 0\naddr 1 2 3\nsetr 1 0 0\nseti 8 0 4\nseti 9 0 5".Split('\n');
      input = File.ReadAllLines("input.txt");
      ipBinding = int.Parse(input[0].Substring(4));
      List<Action> program = input.Skip(1).Select(f => f.Split(' ')).Select(args => (Action)(
        () => instructions[args[0]](int.Parse(args[1]), int.Parse(args[2]), int.Parse(args[3]))
      )).ToList();

      Dictionary<int, bool> hits = new Dictionary<int, bool>();
      int lastHit = 0;

      registers[0] = 57;
      for (int instructionPointer = 0; instructionPointer < program.Count; instructionPointer++)
      {
        // lines 18+ spin r3 from 1 until r3 * 256 > r5. short circuit that.
        if (instructionPointer == 18) registers[1] = registers[5] / 256;
        // There's a jump at program instruction 28 that we want to take. R0 must == r4 there. R0 is not modified
        // in the program.
        if (instructionPointer == 28)
        {
          if (hits.Count % 100 == 0) Console.WriteLine(hits.Count);
          if (hits.TryGetValue(registers[4], out bool already))
          {
            registers[0] = lastHit;
            break;
          }
          lastHit = registers[4];
          hits.Add(lastHit, true);
        }

        registers[ipBinding] = instructionPointer;
        program[instructionPointer]();
        instructionPointer = registers[ipBinding];
      }

      int answer = registers[0];
      Console.WriteLine(answer);
    }
  }
}
