using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_07
{
  class Step
  {
    public string Name;
    public List<Step> Opens = new List<Step>();
    public List<Step> Prereqs = new List<Step>();
    public bool Done = false;
  }

  class Program
  {
    static SortedDictionary<string, Step> available = new SortedDictionary<string, Step>();
    static Dictionary<string, Step> steps = new Dictionary<string, Step>();
    static void Main(string[] args)
    {
      Dictionary<string, bool> isDependent = new Dictionary<string, bool>();

      var input = new string[]
      {
        "Step C must be finished before step A can begin.",
        "Step C must be finished before step F can begin.",
        "Step A must be finished before step B can begin.",
        "Step A must be finished before step D can begin.",
        "Step B must be finished before step E can begin.",
        "Step D must be finished before step E can begin.",
        "Step F must be finished before step E can begin."
      };
      input = File.ReadAllLines("input.txt");

      foreach (var line in input.Distinct())
      {
        string nameLeft = line.Substring(5, 1);
        string nameRight = line.Substring(36, 1);

        Step left;
        Step right;

        if (!steps.TryGetValue(nameLeft, out left))
        {
          left = new Step() { Name = nameLeft };
          steps.Add(nameLeft, left);
        }

        if (!steps.TryGetValue(nameRight, out right))
        {
          right = new Step() { Name = nameRight };
          steps.Add(nameRight, right);
        }

        left.Opens.Add(right);
        right.Prereqs.Add(left);
        isDependent.TryAdd(nameRight, true);
      }

      foreach (var start in steps.Values.Where(f => f.Prereqs.Count == 0))
      {
        available.Add(start.Name, start);
      }
      
      print();
    }

    static void print()
    {      
      if (available.Count == 0) return;

      Step current = available[available.Keys.First()];
      Console.Write(current.Name);
      current.Done = true;
      available.Remove(current.Name);
      foreach (Step next in current.Opens) {
        if (next.Done) continue;
        if (!next.Prereqs.All(f => f.Done)) continue;
        available.TryAdd(next.Name, next);
      }
      print();
    }
  }
}
