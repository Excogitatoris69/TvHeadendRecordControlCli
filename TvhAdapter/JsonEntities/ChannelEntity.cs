using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvhAdapter.JsonEntities
{

    /// <summary>
    /// Contains channelname and uuid
    /// </summary>
    public class ChannelEntity
    {
        [JsonPropertyName("key")]
        public string key { get; set; } // channel/list = uuid

        [JsonPropertyName("val")]
        public string val { get; set; } // channel/list = name

        [JsonPropertyName("uuid")]
        public string uuid { get; set; } // channel/grid

        [JsonPropertyName("name")]
        public string name { get; set; } // channel/grid

        [JsonPropertyName("number")]
        public int number { get; set; }

        [JsonPropertyName("enabled")]
        public bool enabled { get; set; }

        [JsonPropertyName("icon_public_url")]
        public string iconPublicUrl { get; set; }

        [JsonPropertyName("icon")]
        public string icon { get; set; }

        [JsonPropertyName("tags")]
        public List<string> tagUuidListList { get; set; }

    }


    /// <summary>
    /// Contains list of channelentries.
    /// TvHeadend-Server send Data in this json format:
    ///        <code>
    ///        {
    ///        "entries":
    ///         [
    ///            {"key":"d8518d80f7633510846bad57f56eea59","val":"RTLplus"},
    ///            {"key":"47db1d02b583e93e1a64e3b4feb3001e","val":"3sat"},
    ///            {"key":"72c87a034b59d91d005c08cb5e342c92","val":"NDR FS MV"}
    ///         ]
    ///        } 
    /// </code>
    /// </summary>
    public class ChannelEntryList
    {
        [JsonPropertyName("total")]
        public int count { get; set; }

        [JsonPropertyName("entries")]
        public List<ChannelEntity> entries { get; set; }
    }
}
