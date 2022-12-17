using System.Text.RegularExpressions;
using BackendForFrontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendForFrontend.Controllers;

[ApiController]
[Route("api/character-window")]
public class CharacterWindowController : ControllerBase {
    private readonly IPoeClient PoeClient;

    public CharacterWindowController(IPoeClient poeClient) {
        PoeClient = poeClient;
    }

    [HttpGet("get-stash-items")]
    public async Task<List<PoeStashItemModel>> Get(
        [FromQuery] string accountName,
        [FromQuery] string sessionId,
        [FromQuery] string realm,
        [FromQuery] string league,
        [FromQuery] string stashTabName
    ) {
        List<PoeStashItem> items = await PoeClient.GetItemsAsync(accountName,
            sessionId,
            realm,
            league,
            stashTabName);

        return items.Select(AdaptToModel).ToList();
    }

    private static PoeStashItemModel AdaptToModel(PoeStashItem item) {
        return new() {
            IconUrl = item.IconUrl,
            Quality = GetQuality(item),
            ItemLevel = item.ItemLevel,
            ExplicitMods = item.ExplicitMods ?? new(),
            Width = item.Width,
            X = item.PositionX,
            Y = item.PositionY,
            Name = item.TypeLine,
        };
    }

    private static int GetQuality(PoeStashItem item) {
        PoeProperty? qualityProperty = item.Properties.FirstOrDefault(p => p.Name == "Quality");
        if (qualityProperty == null) {
            return 0;
        }

        return qualityProperty.Values
            .Select(t => Convert.ToInt32(Regex.Match(t.value, @"\+(\d+)\%").Groups[1].Value)).Sum();
    }
}
