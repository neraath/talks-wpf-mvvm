namespace AggieTweets.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using AggieTweets.Models;

    public class TweetViewModel
    {
        private Tweet tweet;
        private ICommand editTweetCommand;

        public TweetViewModel(Tweet theTweet)
        {
            tweet = theTweet;
        }

        public ICommand EditTweet
        {
            get
            {
                if (this.editTweetCommand == null)
                {
                    this.editTweetCommand = new DelegateCommand((param) => this.OpenEditWindow());
                }

                return this.editTweetCommand;
            }
        }

        public string Message
        {
            get
            {
                return tweet.Message;
            }

            set
            {
                if (string.IsNullOrEmpty(value)) MessageBox.Show("You must enter a valid tweet.");
                tweet.Message = value;
            }
        }

        public string Author
        {
            get
            {
                return tweet.Author;
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                return tweet.CreatedDate;
            }
        }

        private void OpenEditWindow()
        {
            EditTweet editWindow = new EditTweet();
            editWindow.DataContext = this;
            editWindow.Show();
        }
    }
}
