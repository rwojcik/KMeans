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
