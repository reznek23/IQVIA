using Newtonsoft.Json;
using System;

namespace IQVIA.TweetLoader
{
    public class Tweet
    {
        /// <summary>
        /// Id of Tweet
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
