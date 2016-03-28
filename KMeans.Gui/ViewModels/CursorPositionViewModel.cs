using KMeans.Gui.Models;
using PostSharp.Patterns.Model;

namespace KMeans.Gui.ViewModels
{
  [NotifyPropertyChanged]
  public class CursorPositionViewModel
  {
    public Point2DModel Position { get; set; } = new Point2DModel();

    public bool CursorInsideCanvas { get; set; }

    public string PositionText => Position.Text;
  }
}