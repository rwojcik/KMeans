using PostSharp.Patterns.Model;

namespace KMeans.Gui.ViewModels
{
  [NotifyPropertyChanged]
  public class StatusViewModel
  {
    public string StatusText { get; set; }

    public bool AlgorithmRunning { get; set; }

    public bool AlgorithmNotRunning => !AlgorithmRunning;
  }
}