using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace WebRecipeHelper
{
    public class PoeProperty
    {
        [JsonProperty("name")]
        public string Name { get; }
        [JsonProperty("values")]
        private List<object[]> ValuesObject { get; }
        public List<(string value, int displayMode)> Values { get; }
        [JsonProperty("displayMode")]
        public int DisplayMode { get; }

        public PoeProperty(string name, List<object[]> values, int displayMode)
        {
            this.Name = name;
            this.ValuesObject = values;
            this.DisplayMode = displayMode;

            this.Values = this.ValuesObject.Select(a => (Convert.ToString(a[0]), Convert.ToInt32(a[1]))).ToList();
        }
    }
}
