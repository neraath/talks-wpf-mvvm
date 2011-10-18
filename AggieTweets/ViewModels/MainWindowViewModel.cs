namespace AggieTweets.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    using AggieTweets.DataAccess;
    using AggieTweets.Models;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly string authorizeUrl;
        private DelegateCommand reauthorizeCommand;
        private DelegateCommand updateAuthorizationCommand;
        private DelegateCommand refreshTweetsCommand;
        private TwitterAuthorize authorization = new TwitterAuthorize();
        private ObservableCollection<TweetViewModel> tweets;
        private string pin;

        public MainWindowViewModel(string urlToAuthorize)
        {
            authorizeUrl = urlToAuthorize;
        }

        public ICommand Reauthorize
        {
            get
            {
                if (this.reauthorizeCommand == null)
                {
                    this.reauthorizeCommand = new DelegateCommand((param) => this.Authorize());
                    this.NotifyPropertyChanged("Reauthorize");
                }

                return this.reauthorizeCommand;
            }
        }

        public ICommand UpdateAuthorization
        {
            get
            {
                if (this.updateAuthorizationCommand == null)
                {
                    this.updateAuthorizationCommand = new DelegateCommand((param) => this.UpdateToken(), (param) => this.CanUpdateToken());
                    this.NotifyPropertyChanged("UpdateAuthorization");
                }

                return this.updateAuthorizationCommand;
            }
        }

        public ICommand GetTweets
        {
            get
            {
                if (this.refreshTweetsCommand == null)
                {
                    this.refreshTweetsCommand = new DelegateCommand((param) => this.RefreshTweets(), (param) => this.CanRefreshTweets());
                }

                return this.refreshTweetsCommand;
            }
        }

        public string Pin
        {
            get
            {
                return this.pin;
            }

            set
            {
                this.pin = value;
                this.NotifyPropertyChanged("Pin");
            }
        }

        public string AuthorizedUser
        {
            get
            {
                return string.IsNullOrEmpty(authorization.ScreenName) ? "NOT AUTHORIZED" : authorization.ScreenName;
            }
        }

        public SolidColorBrush AuthorizedUserColor
        {
            get
            {
                return string.IsNullOrEmpty(authorization.ScreenName) ? new SolidColorBrush(Colors.DarkRed) : new SolidColorBrush(Colors.Black);
            }
        }

        public ObservableCollection<TweetViewModel> Tweets
        {
            get
            {
                return this.tweets;
            }

            set
            {
                this.tweets = value;
                this.NotifyPropertyChanged("Tweets");
            }
        }

        private void Authorize()
        {
            string requestToken = authorization.GetRequestToken();
            string assembledUrl = authorizeUrl + "?oauth_token=" + requestToken;

            Window browserWindow = new Window() { Height = 600, Width = 800 };
            WebBrowser browser = new WebBrowser();
            browserWindow.Content = browser;
            browserWindow.Show();
            browser.Navigate(assembledUrl);           
        }

        private void UpdateToken()
        {
            if (string.IsNullOrEmpty(this.Pin))
            {
                MessageBox.Show("You must enter a valid PIN.");
                return;
            }

            string pin = this.Pin.Trim();
            if (!authorization.Authorize(pin))
            {
                MessageBox.Show("The PIN you entered is invalid.");
                return;
            }

            this.NotifyPropertyChanged("AuthorizedUser");
            this.NotifyPropertyChanged("AuthorizedUserColor");
        }

        private void RefreshTweets()
        {
            var dataLayer = new TwitterDataLayer(authorization.Tokens);
            IEnumerable<Tweet> mytweets = dataLayer.GetMyTweets();
            var tweetViewModels = from tweet in mytweets select new TweetViewModel(tweet);

            this.Tweets = new ObservableCollection<TweetViewModel>(tweetViewModels);
        }

        private bool CanUpdateToken()
        {
            return !string.IsNullOrEmpty(this.Pin);
        }

        private bool CanRefreshTweets()
        {
            return authorization.IsAuthorized;
        }
    }
}
