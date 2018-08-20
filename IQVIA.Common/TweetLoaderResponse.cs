using System.Collections.Generic;

namespace IQVIA.Common
{
    public class TweetLoaderResponse
    {
        public TweetLoaderResponse()
        {
            Tweets = new List<Tweet>();
        }

        public string ErrorMessage { get; set; }

        public List<Tweet> Tweets { get; set; }
    }
}
