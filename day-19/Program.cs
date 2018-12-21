using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_19
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

      registers[0] = 1;
      for (int instructionPointer = 0; instructionPointer < program.Count; instructionPointer++)
      {
        registers[ipBinding] = instructionPointer;
        program[instructionPointer]();
        Console.WriteLine($"ip={instructionPointer} [{string.Join(' ', registers)}]");
        instructionPointer = registers[ipBinding];

        // After figuring out what the body of the input program was...
        if (instructionPointer == 3)
        {
          registers[0] = sumOfFactors(registers[5]);
          break;
        }
      }

      int answer = registers[0];
      Console.WriteLine(answer);
    }

    private int sumOfFactors(int target)
    {
      int sum = 0;
      for (int i=1;i<=target;i++)
      {
        if (target % i == 0)
        {
          sum += i;
        }
      }
      return sum;
    }
  }
}
