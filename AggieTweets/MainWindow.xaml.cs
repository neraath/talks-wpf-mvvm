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
    using AggieTweets.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TwitterAuthorizeUrlSettingsKey = "TwitterAuthorizeUrl";

        public MainWindow()
        {
            InitializeComponent();
            string authorizeUrl = ConfigurationManager.AppSettings[TwitterAuthorizeUrlSettingsKey];
            this.DataContext = new MainWindowViewModel(authorizeUrl);
        }
    }
}
