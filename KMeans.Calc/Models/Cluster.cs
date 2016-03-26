using PostSharp.Patterns.Contracts;
using PostSharp.Patterns.Model;

namespace KMeans.Calc.Models
{
  [NotifyPropertyChanged]
  public class Cluster
  {
    public double[] Values { get; }

    public Cluster(double[] values)
    {
      Values = values;
    }

    public Cluster([StrictlyPositive] int dimensions)
    {
      Values = new double[dimensions];
    }
  }
}
