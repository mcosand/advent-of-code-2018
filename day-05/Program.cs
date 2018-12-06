using System;
using System.IO;

namespace day_05
{
  class Node
  {
    public char value;
    public Node next;
    public Node prev;
    public int i;
  }

  class Program
  {
    static void Main(string[] args)
    {
      int min = int.MaxValue;
      for (char problem = 'A'; problem <= 'Z'; problem++)
      {
        int count = 0;
        int translate = 'a' - 'A';
        Node current = null;
        Node first = null;

        string input = "dabAcCaCBAcCcaDA";
        input = File.ReadAllText("input.txt");
        foreach (var c in input)
        {
          if (c == problem || c == problem + translate) continue;

          var newNode = new Node() { value = c, prev = current, i = count };
          if (current != null) current.next = newNode;
          if (first == null) first = newNode;
          current = newNode;
          count++;
        }

        current = first;
        while (current != null && current.next != null)
        {
          if (current.value + translate == current.next.value || current.value - translate == current.next.value)
          {
            if (current.next.next != null)
            {
              current.next.next.prev = current.prev;
            }

            if (current.prev == null)
            {
              current = first = current.next.next;
            }
            else
            {
              current.prev.next = current.next.next;
              current = current.prev;
            }
            count -= 2;
          }
          else
          {
            current = current.next;
          }
        }

        Console.WriteLine(problem + ": " + count);
        min = Math.Min(count, min);
      }
      Console.WriteLine(min);
    }
  }
}
