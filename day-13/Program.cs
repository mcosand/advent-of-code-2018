using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace day_13
{
  class Program
  {
    enum Direction
    {
      Right,
      Down,
      Left,
      Up
    }

    class Cart
    {
      public Cart(int x, int y, Direction dir)
      {
        this.x = x;
        this.y = y;
        this.dir = dir;
      }
      public int x;
      public int y;
      public Direction dir;
      public int turn;
      public bool crashed = false;
    }

    static void Main(string[] args)
    {
      string file = "sample.txt";
      file = "input.txt";

      string[] lines = File.ReadAllLines(file);
      char[][] grid = new char[lines.Length][];
      List<Cart> carts = new List<Cart>();

      for (var y = 0; y < lines.Length; y++)
      {
        var line = lines[y];
        grid[y] = new char[line.Length];
        for (var x = 0; x < line.Length; x++)
        {
          char now = line[x];
          if (now == '>')
          {
            carts.Add(new Cart(x, y, Direction.Right));
            grid[y][x] = '-';
          }
          else if (now == '<')
          {
            carts.Add(new Cart(x, y, Direction.Left));
            grid[y][x] = '-';
          }
          else if (now == '^')
          {
            carts.Add(new Cart(x, y, Direction.Up));
            grid[y][x] = '|';
          }
          else if (now == 'v')
          {
            carts.Add(new Cart(x, y, Direction.Down));
            grid[y][x] = '|';
          }
          else
          {
            grid[y][x] = now;
          }
        }
      }
      bool collided = false;
      int ticks = 0;
      while (carts.Count > 1)
      {
        foreach (var cart in carts.Where(f => !f.crashed).OrderBy(f => f.y).ThenBy(f => f.x))
        {
          int nextX = cart.x;
          int nextY = cart.y;
          if (cart.dir == Direction.Left) nextX--;
          else if (cart.dir == Direction.Right) nextX++;
          else if (cart.dir == Direction.Up) nextY--;
          else if (cart.dir == Direction.Down) nextY++;

          if (carts.Any(f => f.x == nextX && f.y == nextY))
          {
            //Console.WriteLine($"Collision at {nextX},{nextY}");
            carts.Where(f => f.x == nextX && f.y == nextY).ToList().ForEach(f => f.crashed = true);
            cart.crashed = true;
            collided = true;
          }

          cart.x = nextX;
          cart.y = nextY;

          char next = grid[nextY][nextX];
          if (next == '/' && cart.dir == Direction.Left) cart.dir = Direction.Down;
          else if (next == '/' && cart.dir == Direction.Up) cart.dir = Direction.Right;
          else if (next == '/' && cart.dir == Direction.Down) cart.dir = Direction.Left;
          else if (next == '/' && cart.dir == Direction.Right) cart.dir = Direction.Up;
          else if (next == '\\' && cart.dir == Direction.Up) cart.dir = Direction.Left;
          else if (next == '\\' && cart.dir == Direction.Down) cart.dir = Direction.Right;
          else if (next == '\\' && cart.dir == Direction.Left) cart.dir = Direction.Up;
          else if (next == '\\' && cart.dir == Direction.Right) cart.dir = Direction.Down;
          else if (next == '+')
          {
            if (cart.turn == 0)
            {
              cart.dir = (Direction)(((int)cart.dir + 3) % 4);
            }
            else if (cart.turn == 2)
            {
              cart.dir = (Direction)(((int)cart.dir + 1) % 4);
            }
            cart.turn = (cart.turn + 1) % 3;
          }

        }
        carts = carts.Where(f => !f.crashed).ToList();
        //dump(grid, carts);
      }
      Console.WriteLine($"Last cart at {carts[0].x},{carts[0].y}");
    }

    static void dump(char[][] grid, List<Cart> carts)
    {
      var color = Console.ForegroundColor;
      for (var y = 0; y < grid.Length; y++)
      {
        for (var x = 0; x < grid[y].Length; x++)
        {
          var cart_s = carts.Where(f => f.x == x && f.y == y).ToArray();
          if (cart_s.Length > 1)
          {
            Console.WriteLine($"Collision at {x},{y}");
          } else if (cart_s.Length == 1) { 
            var cart = cart_s[0];
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(cart.dir == Direction.Up ? '^' : cart.dir == Direction.Down ? 'v' : cart.dir == Direction.Left ? '<' : '>');
            Console.ForegroundColor = color;            
          }
          else
          {
            Console.Write(grid[y][x]);
          }
        }
        Console.WriteLine();
      }
      Console.WriteLine();
    }
  }
}
