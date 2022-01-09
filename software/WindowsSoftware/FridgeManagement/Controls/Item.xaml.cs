using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

namespace FridgeManagement.Controls {
  /// <summary>
  /// Displays one Item
  /// </summary>
  public partial class Item : UserControl, INotifyPropertyChanged {
    private Data.Item _item = null;
    public Data.Item item {
      get => _item;
      set {
        if (_item != value)
        {
          _item = value;
          NotifyPropertyChanged();
        }
      }
    }

    private int _numberOfItems = -1;
    public int numberOfItems {
      get => _numberOfItems;
      set {
        if (_numberOfItems != value)
        {
          _numberOfItems = value;
          NotifyPropertyChanged();
          NotifyPropertyChanged("numberOfItemsVisibility");
          NotifyPropertyChanged("grayImageVisibility");
          NotifyPropertyChanged("colorImageVisibility");
        }
      }
    }
    public Visibility numberOfItemsVisibility {
      get {
        if (_numberOfItems == -1)
          return Visibility.Collapsed;
        else
          return Visibility.Visible;
      }
    }
    public Visibility colorImageVisibility {
      get {
        if (_numberOfItems == 0)
          return Visibility.Collapsed;
        else
          return Visibility.Visible;
      }
    }
    public Visibility grayImageVisibility {
      get {
        if (_numberOfItems != 0)
          return Visibility.Collapsed;
        else
          return Visibility.Visible;
      }
    }

    public delegate void ItemSelectHandler(Data.Item item);
    public event ItemSelectHandler Click;

    public Item()
    {
      InitializeComponent();
      DataContext = this;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      if (Click != null)
      {
        Click(item);
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
    #endregion  }
  }
}