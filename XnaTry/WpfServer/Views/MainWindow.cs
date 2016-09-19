using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json.Linq;
using WpfServer.ViewModels;
using XnaServerLib;
using ItemType = System.String;

namespace WpfServer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //private readonly Server server;

        //public string ServerStatus => string.Format("Sever is {0}", server.Listening ? "Running" : "Not Running");

        //public ObservableCollection<ItemType> Items
        //{
        //    get
        //    {
        //        return (ObservableCollection<ItemType>)GetValue(ItemsProperty);
        //    }
        //    set
        //    {
        //        SetValue(ItemsProperty, value);
        //    }
        //}

        //// Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ItemsProperty =
        //    DependencyProperty.Register("Items", typeof(ObservableCollection<ItemType>), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();
            //server = new Server();
            //server.SubscribeToAll(Callback_ToAll);

            //Items = new ObservableCollection<ItemType>();
            DataContext = new ServerViewModel();
        }
    }
}
