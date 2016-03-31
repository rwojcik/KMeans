using System;
using System.Linq;
using PostSharp.Patterns.Contracts;

namespace KMeans.Calc.Models
{
  //[NotifyPropertyChanged]
  public class Point : IComparable<Point>, IEquatable<Point>
  {
    public Point(params double[] values)
    {
      Values = values;
      Cluster = null;
      PreviousCluster = null;
    }

    public Point([StrictlyPositive] int dimensions)
    {
      Values = new double[dimensions];
      Cluster = null;
      PreviousCluster = null;
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