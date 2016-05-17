using System;
using System.Linq;

namespace KMeans.Calc.Models
{
  //[NotifyPropertyChanged]
  public struct Point : IComparable<Point>, IEquatable<Point>
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
      return (int) Values.Zip(other.Values, (thisValue, otherValue) => Math.Abs(thisValue - otherValue)).Sum();
    }

    public bool Equals(Point other)
    {
      return Values.Length == other.Values.Length &&
             Values.Zip(other.Values, (thisValue, otherValue) => Math.Abs(thisValue - otherValue) < 0.001d).All(b => b);
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
      return $"({string.Join(", ", Values)}), {nameof(Cluster)}: {Cluster}";
    }
  }
}