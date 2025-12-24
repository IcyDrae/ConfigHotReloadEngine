using System.Text.Json.Nodes;

namespace ConfigHotReloadEngine
{
    /* Placeholder for the ConfigurationProvider class.
       This class is intended to provide configuration settings
       and support hot-reloading of configuration changes.
       It exposes a way for other parts of the app to get the current config safely.
    */
    public class ConfigurationProvider
    {
        private JsonNode _currentConfig;
        private readonly string _filePath;
        private readonly FileSystemWatcher _watcher;
        public JsonNode Current => Volatile.Read(ref _currentConfig);
        private event Action<JsonNode>? _onConfigChange;

        public ConfigurationProvider(string filePath)
        {
            _filePath = filePath;

            // Load initial config
            _currentConfig = Json.LoadFromFile(_filePath);

            // Setup file watcher
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(_filePath)!)
            {
                Filter = Path.GetFileName(_filePath),
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size
            };

            _watcher.Changed += (s, e) => Reload();
            _watcher.EnableRaisingEvents = true;
        }

        public void OnChange(Action<JsonNode> callback)
        {
            _onConfigChange += callback;
        }

        private void Reload()
        {
            try
            {
                var newConfig = Json.LoadFromFile(_filePath);

                // Atomic swap
                Interlocked.Exchange(ref _currentConfig, newConfig);

                // Notify subscribers
                _onConfigChange?.Invoke(newConfig);
            }
            catch(Exception)
            {
                Console.WriteLine("Failed to reload configuration, invalid JSON format. Keeping previous configuration.");
            }
        }
    }
}