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
    public int Finishes = 0;
  }

  class Program
  {
    static SortedDictionary<string, Step> available = new SortedDictionary<string, Step>();
    static Dictionary<string, Step> steps = new Dictionary<string, Step>();

    static string[] input = new string[]
    {
        "Step C must be finished before step A can begin.",
        "Step C must be finished before step F can begin.",
        "Step A must be finished before step B can begin.",
        "Step A must be finished before step D can begin.",
        "Step B must be finished before step E can begin.",
        "Step D must be finished before step E can begin.",
        "Step F must be finished before step E can begin."
    };
    static Step[] workers = new Step[2];
    static int clock = 0;
    static int baseTime = 0;

    static void Main(string[] args)
    {
      Dictionary<string, bool> isDependent = new Dictionary<string, bool>();

      input = File.ReadAllLines("input.txt");
      workers = new Step[5];
      baseTime = 60;

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

      while (!steps.Values.All(f => f.Done))
      {
        workTick();
        dump();
        clock++;
      }
      Console.WriteLine(clock - 1);
    }

    static void workTick()
    {
      for (var i=0; i<workers.Length; i++)
      {
        if (workers[i] == null) continue;
        if (workers[i].Finishes == clock)
        {
          workers[i].Done = true;
          foreach (var next in workers[i].Opens)
          {
            available.TryAdd(next.Name, next);
          }
          workers[i] = null;
        }
      }

      for (var i=0;i<workers.Length;i++)
      {
        if (workers[i] == null)
        {
          workers[i] = available.Values.Where(f => !f.Done && f.Prereqs.All(g => g.Done)).FirstOrDefault();
          if (workers[i] == null) continue;
          available.Remove(workers[i].Name);
          workers[i].Finishes = clock + baseTime + (workers[i].Name[0] - '@');
        }
      }

    }

    static void dump()
    {
      Console.WriteLine($"{clock}\t{workers.Aggregate("", (accum, f) => accum + (f == null ? "." : f.Name) + "\t")}{string.Join("", steps.Values.Where(f => f.Done).OrderBy(f => f.Finishes).ThenBy(f => f.Name).Select(f => f.Name))}");
    }
  }
}
