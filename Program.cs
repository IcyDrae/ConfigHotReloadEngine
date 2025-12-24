// See https://aka.ms/new-console-template for more information
using ConfigHotReloadEngine;

var provider = new ConfigurationProvider("./AppConfig.json");

provider.OnChange(config =>
{
    foreach (var kvp in config.AsObject())
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    }
    Console.WriteLine("---- Configuration Reloaded ----");
});

// Print initial config
// Print initial config
foreach (var kvp in provider.Current.AsObject())
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

await Process.Start();
