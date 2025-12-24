namespace ConfigHotReloadEngine
{
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Nodes;

    public static class Json
    {
        public static JsonNode LoadFromFile(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);

            return JsonNode.Parse(jsonString)!;
        }
    }
}
