namespace a.Models;

public class Messenger
{
    private static Messenger _instance;
    public static Messenger Instance => _instance ??= new Messenger();

    public event Action<Temp> TempAdded;

    public void OnTempAdded(Temp temp)
    {
        TempAdded?.Invoke(temp);
    }
}
