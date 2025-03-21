using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace TvhAdapter.JsonEntities
{
    public class MultilingualText
    {
        [JsonExtensionData]
        [JsonPropertyName("Language")]
        public Dictionary<string, object> Language { get; set; }

        /// <summary>
        /// Get the first occurrence of the film regardless of the language.
        /// </summary>
        /// <returns></returns>
        public string GetFirstTitle()
        {
            string result = string.Empty;
            object obj = null;
            if (Language != null && Language.Count > 0)
            {
                foreach (string key in Language.Keys)
                {
                    Language.TryGetValue(key, out obj);
                    if (obj != null)
                    {
                        result = ((JsonElement)obj).GetString();
                        break;
                    }
                }
            }
            return result;
        }

    }
}
