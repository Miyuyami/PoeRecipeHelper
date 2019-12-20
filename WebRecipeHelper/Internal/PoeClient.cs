using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebRecipeHelper
{
    public class PoeClient : IPoeClient
    {
        private readonly HttpClient Client;

        public PoeClient(HttpClient client)
        {
            this.Client = client;
        }

        public async Task<List<PoeItem>> GetItemsAsync(string sessionId, string league, string realm, string accountName, string tabName)
        {
            var query = new Dictionary<string, string>()
            {
                { "league", league },
                { "realm", realm },
                { "accountName", accountName },
                { "tabs", "1" },
            };

            var requestUri = QueryHelpers.AddQueryString("/character-window/get-stash-items", query);
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Cookie", $"POESESSID={sessionId}");

            var response = await this.Client.SendAsync(request);
            response = response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var jo = JObject.Parse(json);
            var tab = jo["tabs"].FirstOrDefault(t => t.Value<string>("n") == tabName);
            if (tab == null)
            {
                throw new Exception("tab not found");
            }

            var index = tab.Value<int>("i");

            query.Add("tabIndex", index.ToString());

            var requestUri2 = QueryHelpers.AddQueryString("/character-window/get-stash-items", query);
            var request2 = new HttpRequestMessage(HttpMethod.Get, requestUri2);
            request2.Headers.Add("Cookie", $"POESESSID={sessionId}");
            var response2 = await this.Client.SendAsync(request2);
            response2 = response2.EnsureSuccessStatusCode();

            var json2 = await response2.Content.ReadAsStringAsync();
            
            return JObject.Parse(json2)["items"].ToObject<List<PoeItem>>();
        }
    }
}
