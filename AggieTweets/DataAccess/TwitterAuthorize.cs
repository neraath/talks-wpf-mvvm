namespace AggieTweets.DataAccess
{
    using System.Configuration;

    using Twitterizer;

    class TwitterAuthorize
    {
        private const string TwitterConsumerKeySettingsKey = "TwitterConsumerKey";
        private const string TwitterConsumerSecretSettingsKey = "TwitterConsumerSecret";

        private readonly string consumerKey;
        private readonly string consumerSecret;

        private string requestToken;
        private OAuthTokenResponse accessToken;

        public TwitterAuthorize()
        {
            consumerKey = ConfigurationManager.AppSettings[TwitterConsumerKeySettingsKey];
            consumerSecret = ConfigurationManager.AppSettings[TwitterConsumerSecretSettingsKey];
        }

        public OAuthTokens Tokens
        {
            get
            {
                return new OAuthTokens()
                    {
                        AccessToken = accessToken.Token,
                        AccessTokenSecret = accessToken.TokenSecret,
                        ConsumerKey = consumerKey,
                        ConsumerSecret = consumerSecret
                    };
            }
        }

        public string ScreenName
        {
            get
            {
                return accessToken != null ? accessToken.ScreenName : string.Empty;
            }
        }

        public bool IsAuthorized
        {
            get
            {
                return accessToken != null && !string.IsNullOrEmpty(accessToken.Token) &&
                       !string.IsNullOrEmpty(accessToken.TokenSecret);
            }
        }

        public bool HasSubmittedRequest
        {
            get
            {
                return requestToken != null;
            }
        }

        public string GetRequestToken()
        {
            OAuthTokenResponse response = OAuthUtility.GetRequestToken(consumerKey, consumerSecret, "oob");
            requestToken = response.Token;
            return requestToken;
        }

        public bool Authorize(string pin)
        {
            accessToken = OAuthUtility.GetAccessToken(consumerKey, consumerSecret, requestToken, pin);
            return !string.IsNullOrEmpty(accessToken.Token);
        }
    }
}
