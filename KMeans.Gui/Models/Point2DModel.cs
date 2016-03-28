using System.Windows;
using PostSharp.Patterns.Model;

namespace KMeans.Gui.Models
{
  [NotifyPropertyChanged]
  public class Point2DModel
  {
    public double X { get; set; }
    public double Y { get; set; }

    public Point Point
    {
      get { return new Point(X, Y); }
      set
      {
        X = value.X;
        Y = value.Y;
      }
    }

    public string Text => $"{nameof(X)}: {X:0000} {nameof(Y)}: {Y:0000}";

    public override string ToString() => Text;
  }
}