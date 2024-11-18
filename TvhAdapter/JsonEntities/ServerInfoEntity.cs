using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvhAdapter.JsonEntities
{
    public class ServerInfoEntity
    {
        [JsonPropertyName("api_version")]
        public int versionApi { get; set; }

        [JsonPropertyName("sw_version")]
        public string versionTvhServerSoftware { get; set; }
    }
}
