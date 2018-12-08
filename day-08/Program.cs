using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_08
{
  class Program
  {
    int[] array;

    static void Main(string[] args)
    {
      new Program().run();
    }

    void run() { 
      string input = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";

      input = File.ReadAllText("input.txt");

      array = input.Split(" ").Select(f => int.Parse(f)).ToArray();
      var root = readNode(0);
      Console.WriteLine(root.Item2);
      var answer = root.Item2;
    }

    Tuple<int,int> readNode(int start) {
      Console.WriteLine("Start reading at " + start);

      int[] childValues = new int[array[start]];

      int offset = 0;
      for (int i=0; i<array[start]; i++)
      {
        var ret = readNode(start+ offset + 2);
        offset += ret.Item1;
        childValues[i] = ret.Item2;
      }

      int value = 0;
      for (int i=0; i<array[start + 1]; i++)
      {
        int v = array[start + offset + 2];
        if (array[start] > 0)
        {
          value += (childValues.Length < v) ? 0 : childValues[v - 1];
        }
        else
        {
          value += v;
        }
        offset++;
      }

      return new Tuple<int,int>(offset + 2, value);
    }
  }
}
