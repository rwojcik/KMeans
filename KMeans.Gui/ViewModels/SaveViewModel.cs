using System.Collections.Generic;
using PostSharp.Patterns.Model;

namespace KMeans.Gui.ViewModels
{
  [NotifyPropertyChanged]
  public class SaveViewModel
  {
    public Stack<int> RandomPointNum { get; set; }
    public Stack<int> RandomClusterNum { get; set; }
    public Stack<int> AlgorithmMaxStepNum { get; set; }
    public string LastLoadPath { get; set; }
  }
}