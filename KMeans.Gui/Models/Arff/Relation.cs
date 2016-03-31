using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMeans.Gui.Models.Arff
{
  public class Relation
  {
    public string Name { get; set; }
    public string Type { get; set; }
    
    public override string ToString()
    {
      return $"{nameof(Name)}: {Name}, {nameof(Type)}: {Type}";
    }
  }
}
