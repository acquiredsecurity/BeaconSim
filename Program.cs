using System;
using System.Net.Http;
using System.Threading.Tasks;
using BeaconSim;

class Program
{
    static async Task Main()
    {
        var rand = new Random();
        var httpClient = new HttpClient();
        var targets = TargetLoader.LoadYamlTargets("targets.yaml");

        while (true)
        {
            var target = targets[rand.Next(targets.Count)];
            string method = target.Protocols[rand.Next(target.Protocols.Count)];

            Console.WriteLine($"[{DateTime.Now}] Trying {method.ToUpper()} to {target.Host}");

            try
            {
                switch (method)
                {
                    case "ping":
                        await BeaconUtils.TryPing(target.Host);
                        break;
                    case "http":
                        await BeaconUtils.TryHttp(target.Host, httpClient);
                        break;
                    case "https":
                        await BeaconUtils.TryHttps(target.Host);
                        break;
                    case "dns":
                        await BeaconUtils.TryDns(target.Host);
                        break;
                    case "ftp":
                        await BeaconUtils.TryFtp(target.Host);
                        break;
                    case "ssh":
                        await BeaconUtils.TrySsh(target.Host);
                        break;
                    case "tcp":
                        if (target.Ports?.Count > 0)
                            await BeaconUtils.TryTcp(target.Host, target.Ports[rand.Next(target.Ports.Count)]);
                        break;
                    case "websocket":
                        await BeaconUtils.TryWebSocket(target.Host);
                        break;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error during {method.ToUpper()} to {target.Host}: {ex.Message}");
            }

            int delay = rand.Next(60, 300);
            Console.WriteLine($"Sleeping for {delay} seconds...\n");
            await Task.Delay(TimeSpan.FromSeconds(delay));
        }
    }
}
