using System.Text.Json.Serialization;

namespace TvhAdapter.JsonEntities
{
    public class ChanneltagEntity
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("uuid")]
        public string uuid { get; set; }

        [JsonPropertyName("comment")]
        public string comment { get; set; }
    }

    public class ChanneltagEntryList
    {
        [JsonPropertyName("total")]
        public int count { get; set; }

        [JsonPropertyName("entries")]
        public List<ChanneltagEntity> entries { get; set; }
    }
}
