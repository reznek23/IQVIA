using IQVIA.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IQVIA.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = ConfigurationManager.AppSettings["tweetServiceUrl"];
            bool canExecute = true;
            if (String.IsNullOrEmpty(url))
            {
                Console.WriteLine("Error: url not specified");
                canExecute = false;
            }
            var fileName = ConfigurationManager.AppSettings["resultFileName"];
            if (String.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("resultFile not specified, tweets will not be saved");
            }
            DateTime? startDate = null;
            try
            {
                startDate = DateTime
                    .Parse(ConfigurationManager.AppSettings["startDate"], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                    .ToUniversalTime();
            }
            catch (Exception)
            {
                Console.WriteLine("Error: startDate not specified or in wrong format");
                canExecute = false;
            }
            DateTime? endDate = null;
            try
            {
                endDate = DateTime
                .Parse(ConfigurationManager.AppSettings["endDate"], CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                .ToUniversalTime();
            }
            catch (Exception)
            {
                Console.WriteLine("Error: endDate not specified or in wrong format");
                canExecute = false;
            }
            if (canExecute)
            {
                Console.WriteLine("Loading tweets");
                Console.WriteLine("URL: " + url);
                Console.WriteLine("StartDate: " + startDate.Value.ToString("O"));
                Console.WriteLine("EndDate: " + endDate.Value.ToString("O"));
                if (!string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine("File: " + fileName);
                }
                Console.WriteLine("------");
                LoadTweets(url, fileName, startDate.Value, endDate.Value).GetAwaiter().GetResult();
            }
            Console.WriteLine("Press any Key to Exit");
            Console.ReadLine();

        }

        static async Task LoadTweets(string tweetServiceUrl, string resultFileName, DateTime startDate, DateTime endDate)
        {
            TweetService service = new TweetService(tweetServiceUrl);
            TweetLoader loader = new TweetLoader(service);
            TweetLoaderResponse response = await loader.LoadTweetsByDate(startDate, endDate).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                Console.WriteLine("Error while loading tweets: " + response.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Loaded tweets count: " + response.Tweets.Count);
            }
            if (!string.IsNullOrEmpty(resultFileName) && response.Tweets != null)
            {
                try
                {
                    loader.SaveToFile(response.Tweets, resultFileName);
                    Console.WriteLine("Saved tweets to: " + resultFileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error while saving tweets: " + ex.Message);
                }
            }
        }
    }
}
