using CommunityToolkit.Mvvm.ComponentModel;

namespace a.Models;

public partial class Temp : ObservableObject
{
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //public int Id { get; set; }

    public string? IpAddress { get; set; }
    public DateTime TimeReading { get; set; }
    public string? MinTemp { get; set; }
    public string? MaxTemp { get; set; }

}
