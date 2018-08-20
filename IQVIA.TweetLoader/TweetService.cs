using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IQVIA.TweetLoader
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
            // to do: error handling
            // to do: validation
            // to do: logging
            // to do: configure await
            List<Tweet> tweets = new List<Tweet>();
            string _startDate = startDate.ToUniversalTime().ToString("O");
            string _endDate = endDate.ToUniversalTime().ToString("O");
            string urlString = String.Format("{0}?startDate={1}&endDate={2}", _tweetUrl, _startDate, _endDate);
            using (HttpResponseMessage httpResponse = await _client.GetAsync(urlString))
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    string stringResponse = await httpResponse.Content.ReadAsStringAsync();
                    tweets = JsonConvert.DeserializeObject<List<Tweet>>(stringResponse);
                }
            }
            return tweets.OrderBy(m => m.Stamp).ToList();
        }

        /// <summary>
        /// Max amount of tweets in one request
        /// </summary>
        public int TweetCount
        {
            get
            {
                return 100;
            }
        }

    }
}
