using Newtonsoft.Json;

namespace PoeRecipeGeneratorApi.Services;

public class PoeProperty {
    public PoeProperty(string name, List<object[]> values, int displayMode) {
        Name = name;
        ValuesObject = values;
        DisplayMode = displayMode;

        Values = ValuesObject.Select(a => (Convert.ToString(a[0]), Convert.ToInt32(a[1]))).ToList();
    }

    [JsonProperty("name")] public string Name { get; }

    [JsonProperty("values")] private List<object[]> ValuesObject { get; }

    public List<(string value, int displayMode)> Values { get; }

    [JsonProperty("displayMode")] public int DisplayMode { get; }
}
