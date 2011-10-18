using System.Windows;

namespace AggieTweets
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Windows.Controls;
    using System.Windows.Media;

    using AggieTweets.DataAccess;
    using AggieTweets.Models;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TwitterAuthorizeUrlSettingsKey = "TwitterAuthorizeUrl";
        private readonly string authorizeUrl;
        private TwitterAuthorize authorization = new TwitterAuthorize();

        public MainWindow()
        {
            authorizeUrl = ConfigurationManager.AppSettings[TwitterAuthorizeUrlSettingsKey];
            InitializeComponent();
            this.DataContext = this;
            this.AuthorizedUser.Foreground = new SolidColorBrush(Colors.DarkRed);
            this.AuthorizedUser.Content = "NOT AUTHORIZED";
        }

        private void Reauthorize_Click(object sender, RoutedEventArgs e)
        {
            string requestToken = authorization.GetRequestToken();
            string assembledUrl = authorizeUrl + "?oauth_token=" + requestToken;

            Window browserWindow = new Window() { Height = 600, Width = 800 };
            WebBrowser browser = new WebBrowser();
            browserWindow.Content = browser;
            browserWindow.Show();
            browser.Navigate(assembledUrl);
        }

        private void UpdateToken_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.Token.Text))
            {
                MessageBox.Show("You must enter a valid PIN.");
                return;
            }

            string pin = this.Token.Text.Trim();
            if (!authorization.Authorize(pin))
            {
                MessageBox.Show("The PIN you entered is invalid.");
                return;
            }

            MessageBox.Show("Successfully Authorized as " + authorization.ScreenName);
            this.AuthorizedUser.Foreground = new SolidColorBrush(Colors.Black);
            this.AuthorizedUser.Content = authorization.ScreenName;
        }

        private void RefreshTweets_Click(object sender, RoutedEventArgs e)
        {
            var dataLayer = new TwitterDataLayer(authorization.Tokens);
            IEnumerable<Tweet> mytweets = dataLayer.GetMyTweets();

            this.Tweets.ItemsSource = mytweets;
        }
    }
}
