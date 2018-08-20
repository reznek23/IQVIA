using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IQVIA.TweetLoader
{
    public class TweetLoader
    {
        private ITweetService _tweetService;

        public TweetLoader(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        public async Task<List<Tweet>> LoadTweetsByDate(DateTime startDate, DateTime endDate)
        {
            // to do: validation
            // to do: error handling
            // to d: distinct
            DateTime _startDate = startDate;
            IEnumerable<Tweet> resultTweets = new List<Tweet>();
            bool endLoading = false;
            while (!endLoading)
            {
                var tweetsData = await _tweetService.GetTweetsByDateAsync(_startDate, endDate);
                if (tweetsData != null && tweetsData.Count > 0)
                {
                    resultTweets = resultTweets.Concat(tweetsData);
                    _startDate = tweetsData.Last().Stamp.AddTicks(1);
                    endLoading = tweetsData.Count < _tweetService.TweetCount || _startDate > endDate;
                }
                else
                {
                    endLoading = true;
                }
            }
            return resultTweets.ToList();
        }
    }
}
