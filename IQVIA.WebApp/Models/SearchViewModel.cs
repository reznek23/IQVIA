using IQVIA.Common;
using System;
using System.Collections.Generic;

namespace IQVIA.WebApp.Models
{
    public class SearchViewModel
    {
        public SearchViewModel()
        {
            Tweets = new List<Tweet>();
            StartDate = new DateTime(2016, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            EndDate = new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        }

        public string ErrorMessage { get; set; }

        public List<Tweet> Tweets { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TweetCount
        {
            get
            {
                return Tweets != null ? Tweets.Count : 0;
            }
        }

        public string StartDateFormatted
        {
            get
            {
                return StartDate.ToUniversalTime().ToString("O");
            }
        }

        public string EndDateFormatted
        {
            get
            {
                return EndDate.ToUniversalTime().ToString("O");
            }
        }
    }
}
