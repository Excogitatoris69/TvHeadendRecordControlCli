using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvhAdapter.JsonEntities
{
    public class DvrAddEntity
    {
        [JsonPropertyName("enabled")]
        public bool enabled { get; set; }

        [JsonPropertyName("start")]
        public long startTime { get; set; }

        [JsonPropertyName("stop")]
        public long stopTime { get; set; }

        [JsonPropertyName("channelname")]
        public string channelName { get; set; }

        [JsonPropertyName("pri")]
        public int priority { get; set; }

        [JsonPropertyName("title")]
        public MultilingualText title { get; set; }

        [JsonPropertyName("subtitle")]
        public MultilingualText subtitle { get; set; }

        [JsonPropertyName("description")]
        public MultilingualText description { get; set; }

        [JsonPropertyName("comment")]
        public string comment { get; set; }

        [JsonPropertyName("config_name")]
        public string dvrProfileUuid { get; set; }

        [JsonPropertyName("disp_extratext")]
        public string dispExtratext { get; set; }

        [JsonPropertyName("disp_description")]
        public string dispDescription { get; set; }

        [JsonPropertyName("disp_subtitle")]
        public string dispSubtitle { get; set; }
    }
}
