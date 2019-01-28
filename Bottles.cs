using BenchmarkDotNet.Attributes;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestBenchmark
{
  [MemoryDiagnoser]
  public class Bottles
  {
    [Benchmark]
    public static void Run()
    {
      string nl = System.Environment.NewLine;
      string desc(int x, bool y) =>
         $"{(x == 0 ? "No more" : x.ToString())} bottle{(x == 1 ? "" : "s")} of beer{(y ? " on the wall" : "")}";
      string act(int x) => x > 0 ? "Take one down and pass it around" : "Go to the store and buy some more";
      for (int i = 99; i >= 0; i--)
        Console.WriteLine($"{desc(i, true)}, {desc(i, false).ToLower()}.{nl}{act(i)}, {desc(i > 0 ? i - 1 : 99, true).ToLower()}.{nl}");
    }

    [Benchmark]
    public static void Run1()
    {
      StringBuilder beerLyric = new StringBuilder();
      string nl = System.Environment.NewLine;

      var beers =
          (from n in Enumerable.Range(0, 100)
           select new
           {
             Say = n == 0 ? "No more bottles" :
                 (n == 1 ? "1 bottle" : n.ToString() + " bottles"),
             Next = n == 1 ? "no more bottles" :
                 (n == 0 ? "99 bottles" :
                     (n == 2 ? "1 bottle" : n.ToString() + " bottles")),
             Action = n == 0 ? "Go to the store and buy some more" :
                 "Take one down and pass it around"
           }).Reverse();

      foreach (var beer in beers)
      {
        beerLyric.AppendFormat("{0} of beer on the wall, {1} of beer.{2}",
            beer.Say, beer.Say.ToLower(), nl);
        beerLyric.AppendFormat("{0}, {1} of beer on the wall.{2}",
            beer.Action, beer.Next, nl);
        beerLyric.AppendLine();
      }
      Console.WriteLine(beerLyric.ToString());
    }

    [Benchmark]
    public static void Run2()
    {
      int countOfBottles = 100;
      string lineTemplate = @"{X} bottles of beer on the wall, {X} bottles " +
                             "of beer.\r\nTake one down and pass it around, {Y} " +
                             "bottles of beer on the wall.\r\n";

      string lastLine = @"No more bottles of beer on the wall, no more " +
                         "bottles of beer.\r\nGo to the store and buy some " +
                         "more, {X} bottles of beer on the wall.";

      List<string> songLines = new List<string>();
      Enumerable.Range(1, countOfBottles)
          .Reverse()
          .ToList()
          .ForEach
          (c => songLines.Add(lineTemplate.Replace("{X}",
              c.ToString()).Replace("{Y}", (c - 1) != 0 ? (c - 1).ToString() :
              @"no more")));

      //Add the last line
      songLines.Add(lastLine.Replace("{X}", countOfBottles.ToString()));

      songLines.ForEach(c => Console.WriteLine(c));
    }

    [Benchmark]
    public static void Run3()
    {
      for (int i = 99; i > -1; i--)
      {
        if (i == 0)
        {
          Console.WriteLine("No more bottles of beer on the wall, no more bottles of beer.");
          Console.WriteLine("Go to the store and buy some more, 99 bottles of beer on the wall.");
          break;
        }
        if (i == 1)
        {
          Console.WriteLine("1 bottle of beer on the wall, 1 bottle of beer.");
          Console.WriteLine("Take one down and pass it around, no more bottles of beer on the wall.");
          Console.WriteLine();
        }
        else
        {
          Console.WriteLine("{0} bottles of beer on the wall, {0} bottles of beer.", i);
          Console.WriteLine("Take one down and pass it around, {0} bottles of beer on the wall.", i - 1);
          Console.WriteLine();
        }
      }
    }

    [Benchmark]
    public static void Run4()
    {
      const int bottlesCount = 99;
      for (int i = bottlesCount; i > 0; i--)
      {
        string current = "bottle".ToQuantity(i);
        Console.WriteLine("{0} of beer on the wall, {0} of beer.", current);
        string next = i > 1 ? "bottle".ToQuantity(i - 1) : "no more bottles";
        Console.WriteLine("Take one down and pass it around, {0} of beer on the wall.", next);
      }
      Console.WriteLine("No more bottles of beer on the wall, no more bottles of beer.");
      Console.WriteLine("Go to the store and buy some more, {0} bottles of beer on the wall.", bottlesCount);
    }
  }
}
