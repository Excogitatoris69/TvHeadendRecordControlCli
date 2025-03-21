using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvhAdapter.JsonEntities
{
    public class DvrUpcomingEntity
    {
        [JsonPropertyName("uuid")]
        public string? uuid { get; set; }

        [JsonPropertyName("owner")]
        public string? owner { get; set; }

        [JsonPropertyName("channelname")]
        public string? channelName { get; set; }

        [JsonPropertyName("title")]
        public MultilingualText title { get; set; }

        [JsonPropertyName("description")]
        public MultilingualText description { get; set; }

        [JsonPropertyName("subtitle")]
        public MultilingualText subtitle { get; set; }

        [JsonPropertyName("disp_subtitle")]
        public string? dispSubtitle { get; set; }

        [JsonPropertyName("disp_description")]
        public string? dispDescription { get; set; }

        [JsonPropertyName("disp_extratext")]
        public string? dispExtratext { get; set; }

        [JsonPropertyName("start")]
        public long startTime { get; set; }

        [JsonPropertyName("stop")]
        public long stopTime { get; set; }

        [JsonPropertyName("start_real")]
        public long startTimeReal { get; set; }

        [JsonPropertyName("stop_real")]
        public long stopTimeReal { get; set; }

        //[JsonPropertyName("pri")]
        //public int priority { get; set; }

        [JsonPropertyName("config_name")]
        public string? dvrProfileUuid { get; set; }

        [JsonPropertyName("channel")]
        public string? channelUuid { get; set; }

        [JsonPropertyName("sched_status")]
        public string? scheduledStatus { get; set; }

        [JsonPropertyName("status")]
        public string? status { get; set; }


    }

    public class DvrUpcomingEntryList
    {
        [JsonPropertyName("total")]
        public int count { get; set; }

        [JsonPropertyName("entries")]
        public List<DvrUpcomingEntity> dvrUpcomingEntityList { get; set; }
    }


}
