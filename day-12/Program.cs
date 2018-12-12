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
      int generations = 2000;
      int pad = (int)(generations * 1.5);
      state = new string('.', (int)(pad)) + state + new string('.', (int)(pad));

      Dictionary<string, bool> rules = rulesInput.ToDictionary(f => f.Substring(0, 5), f => f[f.Length - 1] == '#');
      int lastSum = 0;

      for (int gen=0; gen<generations;gen++)
      {
        //Console.WriteLine(state.Substring(27, state.Length - 50));
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

        int sum = 0;
        for (int i = 0; i < state.Length; i++)
        {
          if (state[i] == '#') sum += i - pad;
        }
        Console.WriteLine($"{gen + 1} {sum} {sum - lastSum}" );
        lastSum = sum;
      }

      // Within 200 steps, sum starts incrementing by 42 each generation. Formula ends up as 50000000000 * 42 + 61
    }
  }
}
