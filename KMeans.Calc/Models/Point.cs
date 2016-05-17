using System;
using System.Linq;

namespace KMeans.Calc.Models
{
  //[NotifyPropertyChanged]
  public class Point : IComparable<Point>, IEquatable<Point>
  {
    public Point(params double[] values)
    {
      Values = values;
      Cluster = default(Cluster);
      PreviousCluster = default(Cluster);
      Class = string.Empty;
    }

    public Point(string className, params double[] values)
    {
      Values = values;
      Cluster = default(Cluster);
      PreviousCluster = default(Cluster);
      Class = className;
    }

    public double[] Values { get; }

    public Cluster Cluster { get; set; }

    public Cluster PreviousCluster { get; set; }

    public string Class { get; set; }

    public int CompareTo(Point other)
    {
      return (int) Values.Zip(other.Values, (thisValue, otherValue) => thisValue - otherValue).Sum();
    }

    public bool Equals(Point other)
    {
      return Values.Length == other.Values.Length &&
             Math.Abs(
               Values.Zip(other.Values, (thisValue, otherValue) => thisValue - otherValue)
                 .Select(diff => Math.Pow(diff, 2))
                 .Sum()) < 0.0001d;
    }

    public override bool Equals(object obj)
    {
      return obj is Point && Equals((Point) obj);
    }

    public override int GetHashCode()
    {
      return (int) Values.Aggregate((left, right) => left.GetHashCode() ^ right.GetHashCode());
    }

    public override string ToString()
    {
      return $"({string.Join(", ", Values)}), {nameof(Cluster)}: {Cluster?.ToString() ?? "unknown"}";
    }
  }
}