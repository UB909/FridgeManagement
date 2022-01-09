using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FridgeManagement {
  /// <summary>
  /// Interaktionslogik für MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, INotifyPropertyChanged {
    #region current view
    private Views.UserControlWithTitle _currentView = null;
    public Views.UserControlWithTitle currentView {
      get => _currentView; set {
        if (_currentView != value)
        {
          if (_currentView != null)
          {
            _currentView.Visibility = Visibility.Collapsed;
          }
          _currentView = value;
          _currentView.Visibility = Visibility.Visible;
          if(value != locationView)
          {
            wndLocationSelection.SelectedItem = null;
          }
          NotifyPropertyChanged();
        }
      }
    }
    #endregion

    public MainWindow()
    {
      InitializeComponent();

      // Connect and load data
      wndSqlConnectionState.state = Controls.TrafficLight.State.yellow;
      lblInfoMsg.Text = "Connecing to SQL database...";
      Task.Run(() =>
      {
        try
        {
          // set these values correctly for your database server
          //var builder = new MySqlConnectionStringBuilder
          //{
          //  Server = "192.168.xxx.xxx",
          //  UserID = "fridge",
          //  Password = "xxx",
          //  Database = "fridge",
          //};

          var builder = new MySqlConnectionStringBuilder
          {
            Server = "localhost",
            UserID = "fridge",
            Password = "xxx",
            Database = "fridge",
          };

          // open a connection
          MySqlConnection connection = new MySqlConnection(builder.ConnectionString);
          connection.Open();
          Dispatcher.Invoke(() => lblInfoMsg.Text = "Loading initial data from SQL database...");

          // load data
          Data.DataContainer.loadLocalInitialData(connection);

          Dispatcher.Invoke(() =>
          {
            // update categories list
            wndCategories.categories = Data.DataContainer.categories;

            // update items in categoryView
            categoryView.items = Data.DataContainer.items;

            // update itemView
            itemView.categories = Data.DataContainer.categories;
            itemView.entries = Data.DataContainer.entries;
            itemView.locations = Data.DataContainer.locations;

            // update locationView
            locationView.items = Data.DataContainer.items;
            locationView.entries = Data.DataContainer.entries;

            // set current view to all
            currentView = categoryView;

            wndLocationSelection.ItemsSource = Data.DataContainer.locations;

            // update status bar
            wndSqlConnectionState.state = Controls.TrafficLight.State.green;
            lblInfoMsg.Text = "";

            DataContext = this;


            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            Data.DataContainer.synchronizeOverWifi();
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
            // DEbug
          });
        }
        catch (Exception ex)
        {
          Dispatcher.Invoke(() =>
          {
            wndSqlConnectionState.state = Controls.TrafficLight.State.red;
            lblInfoMsg.Text = "Error: " + ex.Message;
          });
        }
      });
    }

    private void CategoryView_ItemSelect(Data.Item item)
    {
      currentView = itemView;
      itemView.item = item;
    }

    private void wndCategories_CategorySelected(Data.Category category)
    {
      currentView = categoryView;
      categoryView.category = category;
    }

    private void btnAdd_Click(object sender, RoutedEventArgs e)
    {
      ContextMenu contextMenu = btnAdd.ContextMenu;
      contextMenu.PlacementTarget = btnAdd;
      contextMenu.IsOpen = true;
      e.Handled = true;
    }

    private void addItem_Clicked(object sender, RoutedEventArgs e)
    {
      Windows.NewItemWindow newWindow = new Windows.NewItemWindow(Data.DataContainer.categories);
      if (newWindow.ShowDialog() == true)
      {
        int idx = 0;
        for (; idx < Data.DataContainer.items.Count; idx++)
        {
          if (String.Compare(Data.DataContainer.items[idx].name.ToLower(), newWindow.newItem.name.ToLower()) >= 0)
          {
            break;
          }
        }
        Data.DataContainer.items.Insert(idx, newWindow.newItem);
        currentView = itemView;
        itemView.item = newWindow.newItem;
      }
    }

    private void addLocation_Clicked(object sender, RoutedEventArgs e)
    {
      Windows.NewLocationWindow newWindow = new Windows.NewLocationWindow();
      if (newWindow.ShowDialog() == true)
      {
        int idx = 0;
        for (; idx < Data.DataContainer.locations.Count; idx++)
        {
          if (String.Compare(Data.DataContainer.locations[idx].name.ToLower(), newWindow.newItem.name.ToLower()) >= 0)
          {
            break;
          }
        }
        Data.DataContainer.locations.Insert(idx, newWindow.newItem);
      }
    }

    private void addCategory_Clicked(object sender, RoutedEventArgs e)
    {
      Windows.NewCategoryWindow newWindow = new Windows.NewCategoryWindow();
      if (newWindow.ShowDialog() == true)
      {
        int idx = 0;
        for (; idx < Data.DataContainer.categories.Count; idx++)
        {
          if (String.Compare(Data.DataContainer.categories[idx].name.ToLower(), newWindow.newItem.name.ToLower()) >= 0)
          {
            break;
          }
        }
        Data.DataContainer.categories.Insert(idx, newWindow.newItem);
      }
    }

    private void LocationSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var location = wndLocationSelection.SelectedItem as Data.Location;
      if (location != null)
      {
        locationView.location = location;
        currentView = locationView;
      }
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion
  }
}
