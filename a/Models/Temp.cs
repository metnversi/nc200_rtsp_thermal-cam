using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using a.ViewModels;

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

    public int? AreaId { get; set; } // Optional foreign key property
    public Area? Area { get; set; } // Optional reference navigation to principal

}
public class Area
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<Cam> Cameras { get; } = new List<Cam>(); 
    public ICollection<Active> Actives { get; } = new List<Active>();
}

public class Active
{
    public int Id { get; set; }
    public string? IpAddress { get; set; }
    public bool IsActive { get; set; }
    public int? AreaId { get; set; } // Optional foreign key property
    public Area? Area { get; set; } // Optional reference navigation to principal
}

public record class ActiveUpdates(Active Active);
public record class MaximizeMessage(CamViewModel Content);
public record class TempMessage(Temp Temp);

