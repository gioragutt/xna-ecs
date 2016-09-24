using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections;

namespace WpfServer.Windows
{
    public sealed class ObservableItemCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public ObservableItemCollection()
        {
            CollectionChanged += items_CollectionChanged;
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
        }

        private void items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RemovePropertyChangedNotifications(e.OldItems);
            AddPropertyChangedNotifications(e.NewItems);
        }

        private void AddPropertyChangedNotifications(IEnumerable items)
        {
            if (items == null)
                return;

            foreach (INotifyPropertyChanged item in items)
                item.PropertyChanged += item_PropertyChanged;
        }

        private void RemovePropertyChangedNotifications(IEnumerable items)
        {
            if (items == null)
                return;

            foreach (INotifyPropertyChanged item in items)
                item.PropertyChanged -= item_PropertyChanged;
        }
    }
}