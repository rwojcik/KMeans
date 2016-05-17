using System.ComponentModel;
using System.Runtime.CompilerServices;
using KMeans.Gui.Annotations;
using PropertyChanged;

namespace KMeans.Gui.ViewModels
{
  [ImplementPropertyChanged]
  public class StatusViewModel : INotifyPropertyChanged
  {
    public string StatusText { get; set; }

    public bool AlgorithmRunning { get; set; }

    public bool AlgorithmNotRunning => !AlgorithmRunning;

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}