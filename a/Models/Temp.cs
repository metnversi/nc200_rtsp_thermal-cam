using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CommunityToolkit.Mvvm.ComponentModel;

namespace a.Models;

public partial class Temp : ObservableObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey("Cam")]
    public string? IpAddress { get; set; }
    public DateTime TimeReading { get; set; }
    public string? MinTemp { get; set; }
    public string? MaxTemp { get; set; }
}


public class Cam
{
    [Key]
    public string? IpAddress { get; set; }
    public string? CamName { get; set; }
    public ICollection<Temp>? Temps { get; set; }
}


