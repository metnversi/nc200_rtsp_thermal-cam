using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using CommunityToolkit.Mvvm.ComponentModel;

namespace a.Models;

public partial class Temp : ObservableObject
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? IpAddress { get; set; }

    public DateTime Time { get; set; }

    public string? MinTemp { get; set; }

    public string? MaxTemp { get; set; }
}


public partial class Cam : ObservableObject
{
    public int Id { get; set; }
    [ObservableProperty]
    public string? _ipAddress;
    [ObservableProperty]
    public string? _username;
    [ObservableProperty]
    public string? _password;
    [ObservableProperty]
    public bool? _isSelected;
    public ICollection<Temp>? Temps { get; set; }
}


