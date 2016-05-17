using System.Collections.Generic;

namespace KMeans.Gui.Models.Arff
{
  public class Data
  {
    public Dictionary<string, double> Values { get; set; }
    public string Class { get; set; }
    
    public override string ToString()
    {
      return $"({string.Join(", ", Values.Values)}), {nameof(Class)}: {Class}";
    }
  }
}
