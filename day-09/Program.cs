using System;
using System.IO;
using System.Linq;

namespace day_09
{
  class Program
  {
    static void Main(string[] args)
    {
      //new Program().run(30, 5807);
      new Program().run(452, 7125000);
    }

    void run(int players, int maxMarble)
    {
      Place current = new Place() { Value = 0 };
      current.CCW = current;
      current.CW = current;
      Place first = current;

      int nextMarble = 1;
      long[] scores = new long[players];

      while (nextMarble <= maxMarble)
      {
        for (int i=0; i < players; i++)
        {
          if (nextMarble % 23 == 0)
          {
            scores[i] += nextMarble;
            Place booty = current;
            for (var b = 0; b < 7; b++) booty = booty.CCW;
            booty.CW.CCW = booty.CCW;
            booty.CCW.CW = booty.CW;
            scores[i] += booty.Value;
            current = booty.CW;
          }
          else
          {
            Place newMarble = new Place() { Value = nextMarble, CW = current.CW.CW, CCW = current.CW };
            current.CW.CW.CCW = newMarble;
            current.CW.CW = newMarble;
            current = newMarble;
          }
          nextMarble++;
        }
      }

      long win = scores.Max();
      Console.WriteLine(win);
    }

    class Place
    {
      public int Value;
      public Place CCW;
      public Place CW;
    }
  }
}
