using System.Net.Http;

namespace a.Models;

public class ConfigAPI
{
    private HttpClient _client;

    private AuthenAPI _authenAPI;    
    public string Url { get; private set;}

    public ConfigAPI(HttpClient client,string url, AuthenAPI authenAPI)
    {
        _client = client;
        Url = "http://" + url + "/getmsginfo";
        _authenAPI = authenAPI;
    }
}
