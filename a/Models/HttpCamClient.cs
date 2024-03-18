using System.Net.Http;

namespace a.Models;

// urlapi: 192.168.1.168
// url: http://192.168.1.168/getmsginfo = $"http://{urlapi}/getmsginfo"


public class HttpCamClient
{
    public HttpClient _client;
    public string? Url { get; set; }
    public FunctionalAPI F { get; private set; }
    public ConfigAPI C { get; private set; }
    public AuthenAPI A { get; private set; }
    private readonly string? _username;
    private readonly string? _password;

    public HttpCamClient(string url, string username, string password)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Referrer = new Uri("http://192.168.1.168/Default.aspx");
        _client.DefaultRequestHeaders.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        _client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.8,zh-CN;q=0.6,zh;q=0.4,ru;q=0.2");
        _client.DefaultRequestHeaders.Add("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
        _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.122 Safari/537.36");
        A = new AuthenAPI(_client, url, username, password);
        if (username != null || password != null)
        {
            _username = username;
            _password = password;
            Url = "http://" + url + "/getmsginfo";
            F = new FunctionalAPI(_client, url, A);
            C = new ConfigAPI(_client, url, A);
        }
        else Url = url;
    }
}

public class HttpCamClientFactory
{
    public HttpCamClient Create(string ip, string username, string password)
    {
        return new HttpCamClient(ip, username, password);
    }
}
