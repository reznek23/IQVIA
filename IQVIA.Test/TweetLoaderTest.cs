using System;
using IQVIA.Common;
using IQVIA.Test.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQVIA.Test
{
    [TestClass]
    public class TweetLoaderTest
    {
        [TestMethod]
        public void CheckLoader()
        {
            TestTweetService testService = new TestTweetService("Mock\\testData.txt");
            TweetLoader loader = new TweetLoader(testService);
            DateTime startDate = new DateTime(2016, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime endDate = new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddTicks(-1);
            var response = loader.LoadTweetsByDate(startDate, endDate).GetAwaiter().GetResult();
            Assert.IsTrue(string.IsNullOrEmpty(response.ErrorMessage));
            Assert.IsTrue(response.Tweets.Count == testService.TotalTweetCount);
        }
    }
}
