using IQVIA.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQVIA.Test.Mock
{
    public class TestTweetService : ITweetService
    {
        private List<Tweet> tweets;

        public TestTweetService(string tweetsPath)
        {
            tweets = new List<Tweet>();
            LoadTweets(tweetsPath);
        }

        public void LoadTweets(string tweetsPath)
        {
            string text = File.ReadAllText(tweetsPath);
            tweets = JsonConvert.DeserializeObject<List<Tweet>>(text);
        }

        public Task<List<Tweet>> GetTweetsByDateAsync(DateTime startDate, DateTime endDate)
        {
            return Task.Run(() => tweets.Where(m => m.Stamp >= startDate && m.Stamp <= endDate).Take(ResponseTweetCount).ToList());
        }

        public int ResponseTweetCount
        {
            get
            {
                return 100;
            }
        }

        public int TotalTweetCount
        {
            get
            {
                return tweets.Count;
            }
        }
    }
}
