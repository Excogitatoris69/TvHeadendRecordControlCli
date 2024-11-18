using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvhAdapter.JsonEntities
{
    public class EpgEntity
    {
        [JsonPropertyName("channelName")]
        public string channelName { get; set; }

        [JsonPropertyName("channelUuid")]
        public string channelUuid { get; set; }
        

        [JsonPropertyName("title")]
        public string title { get; set; }

        [JsonPropertyName("subtitle")]
        public string subtitle { get; set; }

        [JsonPropertyName("description")]
        public string description { get; set; }

        [JsonPropertyName("start")]
        public long startTime { get; set; }

        [JsonPropertyName("stop")]
        public long stopTime { get; set; }

        [JsonPropertyName("dvrUuid")]
        public string dvrUuid { get; set; }

        [JsonPropertyName("dvrState")]
        public string dvrState { get; set; }


        [JsonPropertyName("summary")]
        public string summary { get; set; }

        [JsonPropertyName("eventId")]
        public int eventId { get; set; }

        [JsonPropertyName("nextEventId")]
        public int nextEventId { get; set; }

        

    }

    public class EpgEntryList
    {
        [JsonPropertyName("totalCount")]
        public int count { get; set; }

        [JsonPropertyName("entries")]
        public List<EpgEntity> epgEntityList { get; set; }
    }

}
