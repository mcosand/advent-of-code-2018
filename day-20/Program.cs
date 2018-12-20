using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace day_20
{
  class Room
  {
    public int x;
    public int y;
    public string path = "";
    public int doors;
    public Dictionary<char, Room> next = new Dictionary<char, Room>();
    public Room(int doors)
    {
      this.doors = doors;
    }
  }

  class Program
  {
    static Dictionary<char, int> dx = new Dictionary<char, int> { { 'N', 0 }, { 'S', 0 }, { 'E', 1 }, { 'W', -1 } };
    static Dictionary<char, int> dy = new Dictionary<char, int> { { 'W', 0 }, { 'E', 0 }, { 'S', 1 }, { 'N', -1 } };
    static Dictionary<char, char> opposites = new Dictionary<char, char> { { 'N', 'S' }, { 'S', 'N' }, { 'E', 'W' }, { 'W', 'E' } };

    readonly string input;
    readonly SortedDictionary<int, SortedDictionary<int, Room>> rooms = new SortedDictionary<int, SortedDictionary<int, Room>>();

    static void Main(string[] args)
    {
      string input = "^WNE$";
      input = "^ENWWW(NEEE|SSE(EE|N))$";
      input = "^ESSWWN(E|NNENN(EESS(WNSE|)SSS|WWWSSSSE(SW|NNNE)))$";
      input = "^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$";
      input = File.ReadAllText("input.txt");

      new Program(input).Run();
    }

    public Program(string input)
    {
      this.input = input;
    }

    private void Run() {

      Room myLocation = new Room(0);
      SortedDictionary<int, Room> row = new SortedDictionary<int, Room>();
      row.Add(0, myLocation);
      rooms.Add(0, row);

      Walk(myLocation, 0);
      print();
      int farthest = rooms.SelectMany(f => f.Value.Select(g => g.Value)).Max(f => f.doors);
    }

    private int Walk(Room current, int i)
    {
      Room startRoom = current;
      int start = i;

      while (true)
      {
        var c = input[i++];
        if (c > 'A' && c < 'Z')
        {
          int x = current.x + dx[c];
          int y = current.y + dy[c];

          SortedDictionary<int, Room> row;
          if (!rooms.TryGetValue(y, out row))
          {
            row = new SortedDictionary<int, Room>();
            rooms.Add(y, row);
          }

          Room neighbor;
          if (!row.TryGetValue(x, out neighbor))
          {
            neighbor = new Room(current.doors + 1);
            neighbor.x = current.x + dx[c];
            neighbor.y = current.y + dy[c];
            row.Add(x, neighbor);
          }

          neighbor.next.TryAdd(opposites[c], current);
          current.next.TryAdd(c, neighbor);

          neighbor.path = current.path + c;
          current = neighbor;

        }
        else if (c == '(')
        {
          i += Walk(current, i);
        }
        else if (c == '|')
        {
          Console.WriteLine();
          print();
          current = startRoom;
        }
        else if (c == ')' || c == '$')
        {
          break;
        }
      }

      return i - start;
    }

    private void print()
    {
      return;
      int miny = rooms.Keys.Min() - 1;
      int minx = rooms.Values.SelectMany(f => f.Keys).Min() - 1;

      foreach (var row in rooms)
      {
        foreach (var col in row.Value)
        {
          Console.SetCursorPosition(2 * (col.Key - minx) - 1, 2 * (row.Key - miny) - 1);
          Console.Write("#" + (col.Value.next.ContainsKey('N') ? ' ' : '#') + "#");

          Console.SetCursorPosition(2 * (col.Key - minx) - 1, 2 * (row.Key - miny));
          Console.Write(col.Value.next.ContainsKey('W') ? ' ' : '#');
          Console.Write(col.Key == 0 && row.Key == 0 ? 'X' : '.');
          Console.Write(col.Value.next.ContainsKey('E') ? ' ' : '#');

          Console.SetCursorPosition(2 * (col.Key - minx) - 1, 2 * (row.Key - miny) + 1);
          Console.Write("#" + (col.Value.next.ContainsKey('S') ? ' ' : '#') + "#");
        }
      }
    }
  }
}

