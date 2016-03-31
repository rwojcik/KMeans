using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMeans.Gui.Models
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
