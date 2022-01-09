using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace FridgeManagement.Views {
  /// <summary>
  /// View of all Items of an Location
  /// </summary>
  public partial class LocationView : UserControlWithTitle {
    #region Data
    protected Data.Location _location;
    protected BindingList<Data.Item> _items;
    protected BindingList<Data.Entry> _entries;
    #endregion

    #region Public Properties
    public Data.Location location {
      get => _location;
      set {
        if (_location != value)
        {
          _location = value;
          Title = _location.name;
          recreate();
          NotifyPropertyChanged();
        }
      }
    }


    public BindingList<Data.Item> items {
      get => _items;
      set {
        if (_items != value)
        {
          _items = value;
          recreate();
          NotifyPropertyChanged();
        }
      }
    }
    public BindingList<Data.Entry> entries {
      get => _entries;
      set {
        if (_entries != value)
        {
          _entries = value;
          recreate();
          NotifyPropertyChanged();
        }
      }
    }
    #endregion

    public LocationView()
    {
      InitializeComponent();
    }

    private void recreate()
    {
      wndPanel.Children.Clear();
      if ((Data.DataContainer.localConnection  == null) || (items == null) || (entries == null) || (location == null))
      {
        return;
      }

      var foundEntries = from e in entries
                         where e.location == location
                         orderby e.numberOfItems descending, e.item.name ascending
                         select e;

      List<Data.Item> foundItems = new List<Data.Item>();
      List<Data.Entry> foundEntriesEmpty = new List<Data.Entry>();
      foreach (Data.Entry entry in foundEntries)
      {
        if (entry.numberOfItems != 0)
        {
          addAddSubControl(entry);
          foundItems.Add(entry.item);
        }
        else
        {
          foundEntriesEmpty.Add(entry);
        }

      }

      // Add all items which are not present in the location yet
      foreach (var item in items) { 
        if(foundItems.IndexOf(item) == -1)
        {
          Data.Entry entry;
          var tmp = from e in foundEntriesEmpty where e.item == item select e;
          if (tmp.Count() == 0)
          {
            entry = Data.Entry.createDelayed(Data.DataContainer.localConnection, item, location);
          }
          else
          {
            entry = tmp.First();
          }
          
          entries.Add(entry);
          addAddSubControl(entry);
        }
      }
    }

    private void addAddSubControl(Data.Entry entry)
    {
      var newItem = new Controls.AddSubToItemInLocationView();
      newItem.entry = entry;
      newItem.Margin = new Thickness(5);
      newItem.Height = 200;
      newItem.Width = 300;
      newItem.BorderBrush = new SolidColorBrush(Colors.Black);
      newItem.BorderThickness = new Thickness(1);
      wndPanel.Children.Add(newItem);
    }
  }
}
