using System;
using System.Linq;

namespace KMeans.Calc.Models
{
  public class Cluster : IComparable<Cluster>, IEquatable<Cluster>
  {
    public Cluster(params double[] values)
    {
      Values = values;
    }

    public double[] Values { get; }

    public int CompareTo(Cluster other)
    {
      return (int)Values.Zip(other.Values, (thisValue, otherValue) => thisValue - otherValue).Sum();
    }

    public bool Equals(Cluster other)
    {
      return Values.Length == other.Values.Length &&
             Math.Abs(
               Values.Zip(other.Values, (thisValue, otherValue) => thisValue - otherValue)
                 .Select(diff => Math.Pow(diff, 2))
                 .Sum()) < 0.0001d;
    }

    public override bool Equals(object obj)
    {
      return obj is Cluster && Equals((Cluster)obj);
    }

    public override int GetHashCode()
    {
      return (int)Values.Aggregate((left, right) => left.GetHashCode() ^ right.GetHashCode());
    }

    public override string ToString()
    {
      return $"({string.Join(", ", Values)})";
    }
  }
}