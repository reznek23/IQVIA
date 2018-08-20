using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IQVIA.Common
{
    public class TweetService : ITweetService
    {
        private static HttpClient _client = new HttpClient();

        private string _tweetUrl;

        public TweetService(string tweetUrl)
        {
            _tweetUrl = tweetUrl;
        }

        /// <summary>
        /// Returns 100 tweets in specified inteval, ordered by stamp
        /// </summary>
        /// <param name="startDate">Earliest entry timestamp in UTC, inclusive</param>
        /// <param name="endDate">Latest entry timestamp in UTC, inclusive</param>
        /// <returns></returns>
        public async Task<List<Tweet>> GetTweetsByDateAsync(DateTime startDate, DateTime endDate)
        {
            // to do: logging
            List<Tweet> tweets = new List<Tweet>();
            string urlString = String.Format("{0}?startDate={1}&endDate={2}", _tweetUrl, FormatDate(startDate), FormatDate(endDate));
            using (HttpResponseMessage httpResponse = await _client.GetAsync(urlString).ConfigureAwait(false))
            {
                httpResponse.EnsureSuccessStatusCode();
                string stringResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
                tweets = JsonConvert.DeserializeObject<List<Tweet>>(stringResponse, settings);
            }
            return tweets.OrderBy(m => m.Stamp).ToList();
        }

        /// <summary>
        /// Amount of tweets returned
        /// </summary>
        public int ResponseTweetCount
        {
            get
            {
                return 100;
            }
        }

        /// <summary>
        /// Returns ISO-8601 date format string 
        /// </summary>
        /// <param name="date">Date to format</param>
        /// <returns></returns>
        public static string FormatDate(DateTime date)
        {
            return date.ToUniversalTime().ToString("O");
        }
    }
}
