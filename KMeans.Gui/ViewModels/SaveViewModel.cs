using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KMeans.Gui.Annotations;
using PropertyChanged;

namespace KMeans.Gui.ViewModels
{
  [ImplementPropertyChanged]
  public class SaveViewModel : INotifyPropertyChanged
  {
    public Stack<int> RandomPointNum { get; set; }
    public Stack<int> RandomClusterNum { get; set; }
    public Stack<int> AlgorithmMaxStepNum { get; set; }
    public string LastLoadPath { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}