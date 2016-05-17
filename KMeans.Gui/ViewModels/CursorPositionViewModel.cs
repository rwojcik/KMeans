using System.ComponentModel;
using System.Runtime.CompilerServices;
using KMeans.Gui.Annotations;
using PropertyChanged;
using Point = System.Windows.Point;

namespace KMeans.Gui.ViewModels
{
  [ImplementPropertyChanged]
  public class CursorPositionViewModel :INotifyPropertyChanged
  {
    public double X { get; set; }
    public double Y { get; set; }

    public Point Position
    {
      get
      {
        return new Point(X, Y);
      }
      set
      {
        X = value.X;
        Y = value.Y;
      }
    }

    public string PositionText => $"{nameof(X)}: {X:0000} {nameof(Y)}: {Y:0000}";

    public bool CursorInsideCanvas { get; set; }
    
    public override string ToString() => PositionText;

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
    }
  }
}