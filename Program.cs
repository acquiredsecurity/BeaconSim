using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.WebSockets;

class Program
{
    static async Task Main(string[] args)
    {
        // --- Show Help Early ---
        if (args.Length > 0 && (args[0] == "--help" || args[0] == "-h"))
        {
            PrintHelp();
            return;
        }

        // --- Default Configuration ---
        string configPath = "targets.yaml";
        int minInterval = 60;  // in seconds
        int maxInterval = 300; // in seconds
        int loopCount = -1;    // -1 = infinite

        // --- Parse CLI Args ---
        foreach (var arg in args)
        {
            if (arg.StartsWith("--config="))
                configPath = arg.Substring("--config=".Length);
            else if (arg.StartsWith("--minInterval="))
                minInterval = int.Parse(arg.Substring("--minInterval=".Length));
            else if (arg.StartsWith("--maxInterval="))
                maxInterval = int.Parse(arg.Substring("--maxInterval=".Length));
            else if (arg.StartsWith("--loopCount="))
                loopCount = int.Parse(arg.Substring("--loopCount=".Length));
        }

        Console.WriteLine($"[i] Loading config from: {configPath}");
        Console.WriteLine($"[i] Beacon interval: {minInterval}–{maxInterval} seconds");
        Console.WriteLine(loopCount > 0 ? $"[i] Loop count: {loopCount}" : "[i] Loop count: infinite");
        Console.WriteLine();

        var rand = new Random();
        var httpClient = new HttpClient();
        var targets = TargetLoader.LoadYamlTargets(configPath);

        int loop = 0;
        while (loopCount < 0 || loop < loopCount)
        {
            loop++;
            var target = targets[rand.Next(targets.Count)];
            string method = target.Protocols[rand.Next(target.Protocols.Count)];

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Attempt {loop}: {method.ToUpper()} → {target.Host}");

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
                        {
                            int port = target.Ports[rand.Next(target.Ports.Count)];
                            await BeaconUtils.TryTcp(target.Host, port);
                        }
                        else
                        {
                            Console.WriteLine("    [!] TCP requested but no ports defined.");
                        }
                        break;
                    case "websocket":
                        await BeaconUtils.TryWebSocket(target.Host);
                        break;
                    default:
                        Console.WriteLine($"    [!] Unsupported method: {method}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"    [!] Exception during {method.ToUpper()} to {target.Host}: {ex.Message}");
            }

            int delay = rand.Next(minInterval, maxInterval + 1);
            Console.WriteLine($"[i] Sleeping for {delay} seconds...\n");
            await Task.Delay(TimeSpan.FromSeconds(delay));
        }

        Console.WriteLine("[✓] Beacon loop finished.");
    }

    static void PrintHelp()
    {
        Console.WriteLine("BeaconSim - C2 Beacon Simulator");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Usage:");
        Console.WriteLine("  BeaconSim.exe [--config=path] [--minInterval=N] [--maxInterval=N] [--loopCount=N]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --config=path        Path to YAML config file (default: targets.yaml)");
        Console.WriteLine("  --minInterval=N      Minimum delay in seconds between beacons (default: 60)");
        Console.WriteLine("  --maxInterval=N      Maximum delay in seconds between beacons (default: 300)");
        Console.WriteLine("  --loopCount=N        Number of beacon attempts (-1 = infinite)");
        Console.WriteLine("  --help, -h           Show this help menu and exit");
        Console.WriteLine();
        Console.WriteLine("Example:");
        Console.WriteLine("  BeaconSim.exe --config=mytargets.yaml --minInterval=30 --maxInterval=120 --loopCount=10");
        Console.WriteLine();
    }
}
