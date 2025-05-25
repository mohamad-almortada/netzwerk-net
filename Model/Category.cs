using System.ComponentModel.DataAnnotations;

namespace Netzwerk.Model;

public class Category
{
    public int Id { get; set; }
    [MaxLength(128)] public string Name { get; set; } = string.Empty;

    public IList<Marker> Markers { get; set; } = [];

}