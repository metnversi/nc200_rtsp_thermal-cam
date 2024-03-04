using a.Models;

using Newtonsoft.Json.Linq;

using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace b4;

public class AuthenAPI
{     
    public HttpClient _client;
    private string key = @"<RSAKeyValue><Modulus>rJ//2AF7b1Js33HVElXPNDYDDDRSHPxMszpyynFDOl5vrqfCuwMpi+k6TqYHbcQwd9BpUIyu0TcCS4DUs0+3ezlyat/NnF9FemmFwPaHmfd27HaYgzTjDbtXs5VmTSQUtrCaFJ0Je5kxiTdm+OMFHKoffBd8D01Kf/q8nYd6AZE=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
    public string Url { get; private set; }
    private string _username;
    private string _password;
    public string? token { get; private set; }

    public AuthenAPI(HttpClient client, string url, string username, string password)
    {
        _client = client;
        Url = "http://" + url + "/getmsginfo";
        _username = username;
        _password = password;
    }
    private string Encrypt()
    {
        var rsa = new RSACryptoServiceProvider();
        rsa.FromXmlString(key);

        if(_password == null)
        {
            throw new ArgumentNullException(nameof(_password));
        }
        var data = Encoding.UTF8.GetBytes(_password);
        var encryptedData = rsa.Encrypt(data, false);
        var base64Encrypted = Convert.ToBase64String(encryptedData);
        return base64Encrypted;
    }

    public async Task<string?> Login()
    {
        Console.WriteLine(Encrypt());
        var data1 = new Request
        {
            action = "request",
            cmdtype = 501,
            sequence = 1,
            message = new Message
            {
                username = _username,
                password = $"{Encrypt()}"
            },

        };
        var response = await HttpUtility.ProcessJson(data1, _client, Url);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            await Console.Out.WriteLineAsync(result);
            var jsonObject = JObject.Parse(result);

            token = jsonObject["message"]["token"].ToString();
            await Console.Out.WriteLineAsync(token);
            return token;
        }
        return null;
    }

    public async Task SendHeartbeat(string token)
    {
        var data = new Request
        {
            action = "notify",
            cmdtype = 9999,
            sequence = 2,
            token = token,
            subtype = 0,
            message = new Message{ }
        };

        var response = await HttpUtility.ProcessJson(data,_client, Url);

        if (response.IsSuccessStatusCode)
        {
            await Console.Out.WriteLineAsync("Heartbeat successful.");
        }
        else
        {
            await Console.Out.WriteLineAsync("Heartbeat failed.");
        }
    }

}