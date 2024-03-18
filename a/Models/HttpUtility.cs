using Newtonsoft.Json;

using System.Net.Http;
using System.Text;

namespace a.Models;

public static class HttpUtility
{
    public static async Task<HttpResponseMessage> ProcessJson(Request request, HttpClient client, string url)
    {
        var jsonRequest = JsonConvert.SerializeObject(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);
        return response;
    }
}
