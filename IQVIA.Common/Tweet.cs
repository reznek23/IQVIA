using Newtonsoft.Json;
using System;

namespace IQVIA.Common
{
    public class Tweet
    {
        /// <summary>
        /// Id 
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Timestamp (in UTC)
        /// </summary>
        [JsonProperty("stamp")]
        public DateTime Stamp { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
