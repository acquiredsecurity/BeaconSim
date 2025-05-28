using System;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading.Tasks;

public static class BeaconUtils
{
    public static async Task TryPing(string host)
    {
        using var ping = new Ping();
        var reply = await ping.SendPingAsync(host, 3000);
        Console.WriteLine(reply.Status == IPStatus.Success
            ? $"Ping to {host} successful"
            : $"Ping to {host} failed: {reply.Status}");
    }

    public static async Task TryHttp(string host, HttpClient client)
    {
        try
        {
            string url = host.StartsWith("http") ? host : $"http://{host}";
            var response = await client.GetAsync(url);
            Console.WriteLine($"HTTP to {url}: {(int)response.StatusCode} {response.ReasonPhrase}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"HTTP request to {host} failed: {e.Message}");
        }
    }

    public static async Task TryHttps(string host)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        using var client = new HttpClient(handler);
        try
        {
            var response = await client.GetAsync($"https://{host}");
            Console.WriteLine($"HTTPS to {host}: {(int)response.StatusCode} {response.ReasonPhrase}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"HTTPS to {host} failed: {ex.Message}");
        }
    }

    public static async Task TryDns(string host)
    {
        try
        {
            var result = await Dns.GetHostAddressesAsync(host);
            Console.WriteLine($"DNS for {host}: {result.Length} address(es) resolved");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DNS lookup for {host} failed: {ex.Message}");
        }
    }

    public static async Task TryFtp(string host, int port = 21)
    {
        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(host, port);
            using var stream = client.GetStream();
            using var writer = new StreamWriter(stream) { AutoFlush = true };
            await writer.WriteLineAsync("USER anonymous");

            Console.WriteLine($"FTP connection to {host}:{port} successful, sent USER command");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FTP to {host}:{port} failed: {ex.Message}");
        }
    }

    public static async Task TryWebSocket(string host)
    {
        try
        {
            using var ws = new ClientWebSocket();
            var uri = new Uri($"wss://{host}");
            await ws.ConnectAsync(uri, CancellationToken.None);
            Console.WriteLine($"WebSocket connected to {uri}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket to {host} failed: {ex.Message}");
        }
    }

    public static async Task TrySsh(string host, int port = 22)
    {
        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(host, port);
            Console.WriteLine($"SSH connection to {host}:{port} successful");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SSH to {host}:{port} failed: {ex.Message}");
        }
    }


    public static async Task TryTcp(string host, int port)
    {
        using var client = new TcpClient();
        var connectTask = client.ConnectAsync(host, port);
        var timeout = Task.Delay(3000);

        if (await Task.WhenAny(connectTask, timeout) == connectTask && client.Connected)
        {
            Console.WriteLine($"TCP connect to {host}:{port} successful");
        }
        else
        {
            Console.WriteLine($"TCP connect to {host}:{port} failed or timed out");
        }
    }
}
