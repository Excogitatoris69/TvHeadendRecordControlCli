using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvhAdapter.JsonEntities
{
    public class DvrProfileEntity
    {
        [JsonPropertyName("uuid")]
        public string uuid { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("pre-extra-time")]
        public int preExtraTime { get; set; }

        [JsonPropertyName("post-extra-time")]
        public int postExtraTime { get; set; }

        
    }

    public class DvrProfileEntityList
    {
        [JsonPropertyName("total")]
        public int count { get; set; }

        [JsonPropertyName("entries")]
        public List<DvrProfileEntity> entityList { get; set; }
    }
}
