using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IQVIA.Common
{
    public interface ITweetService
    {
        /// <summary>
        /// Returns tweets in specified inteval, ordered by stamp. See ResponseTweetCount for max amount of tweets returned
        /// </summary>
        /// <param name="startDate">Earliest entry timestamp in UTC, inclusive</param>
        /// <param name="endDate">Latest entry timestamp in UTC, inclusive</param>
        /// <returns></returns>
        Task<List<Tweet>> GetTweetsByDateAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Amount of tweets returned
        /// </summary>
        int ResponseTweetCount { get; }
    }
}
