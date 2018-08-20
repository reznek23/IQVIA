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
        public async Task<TweetLoaderResponse> LoadTweetsByDate(DateTime startDate, DateTime endDate)
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
                return await LoadTweetsByDateInternal(startDate, endDate);
            }
            catch(Exception ex)
            {
                return new TweetLoaderResponse
                {
                    ErrorMessage = ex.Message
                };
            }
        }

        private async Task<TweetLoaderResponse> LoadTweetsByDateInternal(DateTime startDate, DateTime endDate)
        {
            DateTime _startDate = startDate;
            IEnumerable<Tweet> resultTweets = new List<Tweet>();
            Dictionary<string, int> loadedIds = new Dictionary<string, int>();
            bool endLoading = false;
            bool removeDuplicates = true;
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
                    // left removeDuplicates variable for test purposes
                    // we use dictionary for duplicate search
                    if (removeDuplicates)
                    {
                        List<Tweet> filteredTweets = new List<Tweet>();
                        foreach (var tweet in tweetsData)
                        {
                            if (loadedIds.ContainsKey(tweet.Id))
                            {
                                loadedIds[tweet.Id]++; // count amount of duplicates
                            }
                            else
                            {
                                loadedIds[tweet.Id] = 1;
                                filteredTweets.Add(tweet);
                            }
                        }
                        resultTweets = resultTweets.Concat(filteredTweets);
                    }
                    else
                    {
                        resultTweets = resultTweets.Concat(tweetsData);
                    }
                    // we can use two options
                    // 1. non-overlapping intervals (using .AddTicks(1))
                    // 2. overlapping intervals with explicit duplicates removal
                    // option 1 will be incorrect in cases with two unique tweets having fully equal stamps (including ticks)
                    // option 2 will be incorrect in cases, when we have more than 100 tweets with same stamp
                    // both of them are equally impossible, so for a basic working version, we use second option
                    _startDate = tweetsData.Last().Stamp;
                    // we assume that _startDate will always be greater than startDate and change only in the direction of endDate
                    // (meaning, our service can't return a tweet from 2014 when we search for tweets from 2016 to 2018)
                    endLoading = tweetsData.Count < _tweetService.ResponseTweetCount || _startDate > endDate;
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
