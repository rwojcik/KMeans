using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

namespace KMeans.Calc.Models
{
  [NotifyPropertyChanged]
  public class Point
  {
    public double[] Values { get; }

    public Cluster Cluster { get; set; } = null;

    public Cluster PreviousCluster { get; set; } = null;

    public Point(params double[] values)
    {
      Values = values;
    }

    public Point([StrictlyPositive] int dimensions)
    {
      Values = new double[dimensions];
    }
  }
}