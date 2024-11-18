using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TvhAdapter.JsonEntities
{
    public class TvHResponseData
    {
        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }
    }
}
