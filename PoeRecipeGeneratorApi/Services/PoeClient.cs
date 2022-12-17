using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json.Linq;

namespace PoeRecipeGeneratorApi.Services;

public class PoeClient : IPoeClient {
    private readonly HttpClient Client;

    public PoeClient(HttpClient client) {
        Client = client;
    }

    public async Task<List<PoeStashItem>> GetItemsAsync(
        string accountName,
        string sessionId,
        string realm,
        string league,
        string tabName
    ) {
        var query = new Dictionary<string, string> {
            { "league", league },
            { "realm", realm },
            { "accountName", accountName },
            { "tabs", "1" },
            { "tabIndex", "0" },
        };

        string requestUri = QueryHelpers.AddQueryString("/character-window/get-stash-items", query);
        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Add("Cookie", $"POESESSID={sessionId}");

        HttpResponseMessage response = await Client.SendAsync(request);
        response = response.EnsureSuccessStatusCode();
        string json = await response.Content.ReadAsStringAsync();
        JObject jo = JObject.Parse(json);
        JToken tab = jo["tabs"].FirstOrDefault(t => t.Value<string>("n") == tabName);
        if (tab == null) {
            throw new("tab not found");
        }

        int index = tab.Value<int>("i");

        query["tabIndex"] = index.ToString();

        string requestUri2 = QueryHelpers.AddQueryString("/character-window/get-stash-items", query);
        var request2 = new HttpRequestMessage(HttpMethod.Get, requestUri2);
        request2.Headers.Add("Cookie", $"POESESSID={sessionId}");
        HttpResponseMessage response2 = await Client.SendAsync(request2);
        response2 = response2.EnsureSuccessStatusCode();

        string json2 = await response2.Content.ReadAsStringAsync();

        return JObject.Parse(json2)["items"].ToObject<List<PoeStashItem>>();
    }
}
