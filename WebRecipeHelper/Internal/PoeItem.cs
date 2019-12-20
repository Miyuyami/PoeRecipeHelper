using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebRecipeHelper
{
    public class PoeItem
    {
        [JsonProperty("verified")]
        public bool Verified { get; }
        [JsonProperty("w")]
        public int Width { get; }
        [JsonProperty("h")]
        public int Height { get; }
        [JsonProperty("icon")]
        public string IconUrl { get; }
        [JsonProperty("id")]
        public string Id { get; }
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("typeLine")]
        public string TypeLine { get; }
        [JsonProperty("identified")]
        public bool Identified { get; }
        [JsonProperty("ilvl")]
        public int ItemLevel { get; }
        [JsonProperty("properties")]
        public List<PoeProperty> Properties { get; }
        [JsonProperty("requirements")]
        public List<PoeProperty> Requirements { get; }
        [JsonProperty("utilityMods")]
        public List<string> UtilityMods { get; }
        [JsonProperty("explicitMods")]
        public List<string> ExplicitMods { get; }
        [JsonProperty("descrText")]
        public string Description { get; }
        [JsonProperty("frameType")]
        public int FrameType { get; }
        [JsonProperty("x")]
        public int PositionX { get; }
        [JsonProperty("y")]
        public int PositionY { get; }
        [JsonProperty("inventoryId")]
        public string InventoryId { get; }

        [JsonConstructor]
        public PoeItem(bool verified, int width, int height, string iconUrl, string id, string name, string typeLine, bool identified, int itemLevel, List<PoeProperty> properties, List<PoeProperty> requirements, List<string> utilityMods, List<string> explicitMods, string description, int frameType, int positionX, int positionY, string inventoryId)
        {
            this.Verified = verified;
            this.Width = width;
            this.Height = height;
            this.IconUrl = iconUrl;
            this.Id = id;
            this.Name = name;
            this.TypeLine = typeLine;
            this.Identified = identified;
            this.ItemLevel = itemLevel;
            this.Properties = properties;
            this.Requirements = requirements;
            this.UtilityMods = utilityMods;
            this.ExplicitMods = explicitMods;
            this.Description = description;
            this.FrameType = frameType;
            this.PositionX = positionX;
            this.PositionY = positionY;
            this.InventoryId = inventoryId;
        }
    }
}
