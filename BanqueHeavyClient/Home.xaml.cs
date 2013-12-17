using System.Windows;
using System.Windows.Controls;
using BanqueHeavyClient.ViewModel;
using System;

namespace BanqueHeavyClient
{
    /// <summary>
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private void TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            TabItem item = (TabItem)sender;
            ((HomeViewModel)this.DataContext).Refresh(Convert.ToInt32(item.Tag));
        }
    }
}
