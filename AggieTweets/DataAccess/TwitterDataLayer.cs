namespace AggieTweets.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using AggieTweets.Models;

    using Twitterizer;

    public class TwitterDataLayer
    {
        private readonly OAuthTokens token;

        public TwitterDataLayer(OAuthTokens accessToken)
        {
            token = accessToken;
        }

        public IEnumerable<Tweet> GetMyTweets()
        {
            var response = TwitterTimeline.UserTimeline(token);
            if (response.Result != RequestResult.Success) throw new InvalidOperationException("Cannot fetch Twitter Data.");

            IList<Tweet> tweets = new List<Tweet>();
            foreach (var individualTweet in response.ResponseObject)
            {
                tweets.Add(
                    new Tweet()
                        {
                            Author = individualTweet.User.ScreenName,
                            CreatedDate = individualTweet.CreatedDate,
                            Messsage = individualTweet.Text
                        });
            }

            return tweets;
        }

        public IEnumerable<Tweet> GetHomeTweets()
        {
            var response = TwitterTimeline.HomeTimeline(this.token);
            if (response.Result != RequestResult.Success) throw new InvalidOperationException("Cannot fetch Twitter Data.");

            IList<Tweet> tweets = new List<Tweet>();
            foreach (var individualTweet in response.ResponseObject)
            {
                tweets.Add(
                    new Tweet()
                        {
                            Author = individualTweet.User.ScreenName,
                            CreatedDate = individualTweet.CreatedDate,
                            Messsage = individualTweet.Text
                        });
            }

            return tweets;
        }
    }
}
