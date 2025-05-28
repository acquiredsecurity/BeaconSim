using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


public class TargetEntry
{
    public string Host { get; set; } = string.Empty;
    public List<string> Protocols { get; set; } = new();
    public List<int>? Ports { get; set; }
}

public class TargetConfig
{
    public List<TargetEntry> Targets { get; set; } = new();
}

public static class TargetLoader
{
    public static List<TargetEntry> LoadYamlTargets(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"[!] Cannot find YAML config at: {path}");
            Environment.Exit(1);
        }

        var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
            .WithNamingConvention(YamlDotNet.Serialization.NamingConventions.UnderscoredNamingConvention.Instance)
            .Build();

        var yaml = File.ReadAllText(path);
        var config = deserializer.Deserialize<TargetConfig>(yaml);
        Console.WriteLine($"Loaded {config.Targets.Count} targets from YAML.");
        return config.Targets;
    }
}

