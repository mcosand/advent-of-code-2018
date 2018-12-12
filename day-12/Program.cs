using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace day_12
{
  class Program
  {
    static void Main(string[] args)
    {
      string state = new string('.', 30) + "#..#.#..##......###...###" + new string('.', 30);
      string[] rulesInput = new[]
      {
"...## => #",
"..#.. => #",
".#... => #",
".#.#. => #",
".#.## => #",
".##.. => #",
".#### => #",
"#.#.# => #",
"#.### => #",
"##.#. => #",
"##.## => #",
"###.. => #",
"###.# => #",
"####. => #"
      };

      state = "#.##.#.##..#.#...##...#......##..#..###..##..#.#.....##..###...#.#..#...######...#####..##....#..###";
      rulesInput = new[]
      {
"##.## => .",
"##... => #",
"..#.# => #",
"#.... => .",
"#..#. => #",
".#### => .",
".#..# => .",
".##.# => .",
"#.##. => #",
"####. => .",
"..##. => .",
"##..# => .",
".#.## => #",
".#... => .",
".##.. => #",
"..#.. => #",
"#..## => #",
"#.#.. => #",
"..### => #",
"...#. => #",
"###.. => .",
"##.#. => #",
"#.#.# => #",
"##### => #",
"....# => .",
"#.### => .",
".#.#. => #",
".###. => #",
"...## => .",
"..... => .",
"###.# => #",
"#...# => ."
      };

      state = new string('.', 30) + state + new string('.', 30);

      Dictionary<string, bool> rules = rulesInput.ToDictionary(f => f.Substring(0, 5), f => f[f.Length - 1] == '#');

      for (int gen=0; gen<20;gen++)
      {
        Console.WriteLine(state.Substring(27, state.Length - 50));
        StringBuilder result = new StringBuilder();
        for (int i=0; i<state.Length - 4; i++)
        {
          string current = state.Substring(i, 5);

          if (!rules.TryGetValue(current, out bool birth))
          {
            result.Append('.');
          } else
          {
            result.Append(birth ? '#' : '.');
          }
        }
        state = ".." + result.ToString() + "..";
      }
      Console.WriteLine(state.Substring(27, state.Length - 50));

      int sum = 0;
      for (int i=0;i<state.Length;i++)
      {
        if (state[i] == '#') sum += i - 30;
      }
    }
  }
}
