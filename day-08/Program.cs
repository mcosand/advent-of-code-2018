using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day_08
{
  class Program
  {
    int[] array;
    List<int> metadata = new List<int>();

    static void Main(string[] args)
    {
      new Program().run();
    }

    void run() { 
      string input = "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2";

      input = File.ReadAllText("input.txt");

      array = input.Split(" ").Select(f => int.Parse(f)).ToArray();
      readNode(0);
      int sum = metadata.Sum();
      Console.WriteLine(sum);
    }

    int readNode(int start) {
      Console.WriteLine("Start reading at " + start);

      int offset = 0;
      for (int i=0; i<array[start]; i++)
      {
        offset += readNode(start + offset + 2);
      }
      for (int i=0; i<array[start + 1]; i++)
      {
        int meta = array[start + offset + 2];
        Console.Write(meta + " ");
        metadata.Add(meta);
        offset++;
      }

      Console.WriteLine($"Read {offset} values");
      return offset + 2;
    }
  }
}
