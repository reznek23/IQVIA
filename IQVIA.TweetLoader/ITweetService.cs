using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IQVIA.TweetLoader
{
    public interface ITweetService
    {
        /// <summary>
        /// Returns 100 tweets in specified inteval, ordered by stamp
        /// </summary>
        /// <param name="startDate">Earliest entry timestamp in UTC, inclusive</param>
        /// <param name="endDate">Latest entry timestamp in UTC, inclusive</param>
        /// <returns></returns>
        Task<List<Tweet>> GetTweetsByDateAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Max amount of tweets in one request
        /// </summary>
        int TweetCount { get; }
    }
}
