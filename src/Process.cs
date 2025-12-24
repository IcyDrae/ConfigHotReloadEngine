namespace ConfigHotReloadEngine
{
    public static class Process
    {
        public static async Task Start()
        {
            Console.WriteLine("Watching config. Press Ctrl+C to exit.");
            await Task.Delay(-1);
        }
    }
}
