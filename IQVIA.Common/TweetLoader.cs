using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IQVIA.Common
{
    public class TweetLoader
    {
        private ITweetService _tweetService;

        public TweetLoader(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        /// <summary>
        /// Returns all tweets in specified interval
        /// </summary>
        /// <param name="startDate">Earliest entry timestamp in UTC, inclusive</param>
        /// <param name="endDate">Latest entry timestamp in UTC, inclusive</param>
        /// <param name="checkForDuplicates">Remove duplicates from response</param>
        /// <returns></returns>
        public async Task<TweetLoaderResponse> LoadTweetsByDate(DateTime startDate, DateTime endDate, bool removeDuplicates = true)
        {
            if(startDate > endDate)
            {
                return new TweetLoaderResponse
                {
                    ErrorMessage = "Start date is greater than end date"
                };
            }
            try
            {
                return await LoadTweetsByDateInternal(startDate, endDate, removeDuplicates);
            }
            catch(Exception ex)
            {
                return new TweetLoaderResponse
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        private async Task<TweetLoaderResponse> LoadTweetsByDateInternal(DateTime startDate, DateTime endDate, bool removeDuplicates = true)
        {
            DateTime _startDate = startDate;
            IEnumerable<Tweet> resultTweets = new List<Tweet>();
            List<string> loadedIds = new List<string>();
            bool endLoading = false;
            while (!endLoading)
            {
                List<Tweet> tweetsData;
                try
                {
                    tweetsData = await _tweetService.GetTweetsByDateAsync(_startDate, endDate);
                }
                catch (Exception ex)
                {
                    return new TweetLoaderResponse
                    {
                        ErrorMessage = ex.Message
                    };
                }
                if (tweetsData != null && tweetsData.Count > 0)
                {
                    if (removeDuplicates)
                    {
                        List<Tweet> filteredTweets = new List<Tweet>();
                        foreach (var tweet in tweetsData)
                        {
                            if (loadedIds.Contains(tweet.Id))
                            {
                                // count amount of duplicates ?
                            }
                            else
                            {
                                loadedIds.Add(tweet.Id);
                                filteredTweets.Add(tweet);
                            }
                        }
                        resultTweets = resultTweets.Concat(filteredTweets);
                    }
                    else
                    {
                        resultTweets = resultTweets.Concat(tweetsData);
                    }
                    _startDate = tweetsData.Last().Stamp.AddTicks(1);
                    endLoading = tweetsData.Count < _tweetService.ResponseTweetCount || _startDate >= endDate;
                }
                else
                {
                    endLoading = true;
                }
            }
            return new TweetLoaderResponse
            {
                Tweets = resultTweets.ToList()
            };
        }

        /// <summary>
        /// Save serialized tweets (json) in file
        /// </summary>
        /// <param name="tweets">Tweets to serialize</param>
        /// <param name="filePath">Path to file</param>
        public void SaveToFile(List<Tweet> tweets, string filePath)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            File.WriteAllText(filePath, JsonConvert.SerializeObject(tweets, settings));
        }
    }
}
