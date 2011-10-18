namespace AggieTweets.ViewModels
{
    using System.ComponentModel;

    public class ViewModelBase : INotifyPropertyChanged
    {
        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
