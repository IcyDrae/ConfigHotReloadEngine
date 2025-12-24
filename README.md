# .NET Configuration Hot Reload Engine

A pure .NET, single-node configuration management component that supports
safe runtime configuration reloading without application restarts.

This project focuses on correctness, thread safety, and clean design rather
than infrastructure or UI concerns.

---

## What Problem This Solves

Most applications load configuration once at startup. Any change requires
a full restart, which is not always desirable.

This project provides a simple mechanism to:
- Load strongly typed configuration from JSON
- Keep configuration immutable and safe in memory
- Detect file changes at runtime
- Reload configuration atomically
- Notify consumers when configuration changes

All within a single .NET process, without external dependencies.

---

## Core Concepts

The system revolves around a single idea:
there is always exactly one valid configuration instance in memory.

Configuration updates are applied atomically:
either the new configuration is fully valid and replaces the old one,
or the old configuration remains active.

Consumers never see partial or invalid state.

---

## Features

- Strongly typed configuration loading
- File-based configuration source (JSON)
- Automatic hot reload on file changes
- Thread-safe access to configuration
- Atomic config replacement
- Change notification for subscribers
- Graceful handling of invalid configuration files

---

## Example Usage

```csharp
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

```

## ASP.NET Core example

Even though ASP.NET Core has ```IConfiguration``` and ```reloadOnChange: true``` for appsettings.json, it implements its own watcher internally.

Why? Because:
- They don’t want to restart the app
- They want thread-safe, in-memory updates
- They notify parts of the app (like DI services) when config changes

This ConfigurationProvider is essentially a simplified, custom version of that mechanism.
