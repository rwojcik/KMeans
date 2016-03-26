using PostSharp.Patterns.Model;

namespace KMeans.Gui.ViewModels
{
  [NotifyPropertyChanged]
  public class StatusViewModel
  {
    public CursorPositionViewModel CursorPositionViewModel { get; set; } = new CursorPositionViewModel();

    public string MousePosition => CursorPositionViewModel.PositionText;

    public string StatusText { get; set; }

  }
}