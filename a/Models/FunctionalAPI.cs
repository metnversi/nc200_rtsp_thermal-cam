using a.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace b4;

public class FunctionalAPI
{
    private HttpClient _client;
    private AuthenAPI _authenAPI;

    public string Url { get; private set; }
    public FunctionalAPI(HttpClient client,string url, AuthenAPI authenAPI)
    {
        _client = client;
        Url = "http://" + url + "/getmsginfo";
        _authenAPI = authenAPI;
    }

    public async Task GetTempObj()
    {
        var request = new Request
        {
            action = "request",
            cmdtype = 515,
            sequence = 1,
            token = _authenAPI.token,
            message = new Message
            {
                query_start_time = "2024-02-10 12:22:33",
                query_end_time = "2024-02-26 12:22:33",
                query_type = 2,
                query_interval = 2,
                query_temp_type = 1,
                report = 0,
                page_index = 1,
                page_count = 2
            },
        };
        var response = await HttpUtility.ProcessJson(request,_client, Url);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            await Console.Out.WriteLineAsync(result);
            var jsonObject = JObject.Parse(result);
            var reportPath = jsonObject["message"]["report_path"].ToString();
            Console.WriteLine($"Report Path: {reportPath}");
        }
        else
        {
            Console.WriteLine($"HTTP request failed: {response.StatusCode}");
        }
    }

    public async void ThermalCapture()
    {
        var request = new Request
        {
            action = "request",
            cmdtype = 522,
            sequence = 1,
            token = _authenAPI.token,
            message = new Message { }
        };

        var response = await HttpUtility.ProcessJson(request,_client,Url);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            if (responseObject.retcode == 0)
            {
                var imageUrl = responseObject.message.image_url.ToString();

                var absoluteUrl = new Uri(new Uri("http://192.168.1.168/"), imageUrl);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = absoluteUrl.ToString(),
                    UseShellExecute = true
                });
            }
            else
            {
                Console.WriteLine($"Request failed: {responseObject.retmsg}");
            }
        }
        else
        {
            Console.WriteLine($"HTTP request failed: {response.StatusCode}");
        }
    }

    public async Task<string?> GetLog()
    {
        var request = new Request
        {
            action = "request",
            cmdtype = 510,
            sequence = 1,
            token = _authenAPI.token,
            message = new Message { }
        };

        var response = await HttpUtility.ProcessJson(request,_client,Url);
        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            if (responseObject.retcode == 0)
            {
                var url = responseObject.message.url.ToString();

                var absoluteUrl = new Uri(new Uri("http://192.168.1.168/"), url).ToString();

                return absoluteUrl;
            }
            else
            {
                Console.WriteLine($"Request failed: {responseObject.retmsg}");
                return null;
            }
        }
        else
        {
            Console.WriteLine($"HTTP request failed: {response.StatusCode}");
            return null;
        }
    }

    public async Task<(string irRtspUrl, string vlRtspUrl)> OpenStream()
    {
        var request = new Request
        {
            action = "request",
            cmdtype = 500,
            sequence = 1,
            message = new Message { }
        };

        var response = await HttpUtility.ProcessJson(request,_client,Url);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);

            if (responseObject.retcode == 0)
            {
                var irRtspUrl = responseObject.message.ir_rtsp_url.ToString();
                var vlRtspUrl = responseObject.message.vl_rtsp_url.ToString();
                return (irRtspUrl, vlRtspUrl);
            }
            else
            {
                Console.WriteLine($"Request failed: {responseObject.retmsg}");
                return (null, null);
            }
        }
        else
        {
            Console.WriteLine($"HTTP request failed: {response.StatusCode}");
        }
        return (null, null);
    }
}
