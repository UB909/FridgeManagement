using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
  /// Interaktionslogik für ItemView.xaml
  /// </summary>
  public partial class ItemView : UserControlWithTitle {

    #region Data
    protected Data.Item _item;
    protected BindingList<Data.Category> _categories = null;
    protected BindingList<Data.Location> _locations = null;
    protected BindingList<Data.Entry> _entries = null;
    #endregion

    #region Public Properties
    public Data.Item item {
      get => _item;
      set {
        if (_item != value)
        {
          _item = value;
          Title = _item.name;
          wndImage.item = value;
          updateAddSubToCategories();
          NotifyPropertyChanged();
        }
      }
    }

    public BindingList<Data.Category> categories {
      get => _categories;
      set {
        if (_categories != value)
        {
          _categories = value;
          NotifyPropertyChanged();
        }
      }
    }

    public BindingList<Data.Location> locations {
      get => _locations;
      set {
        if (_locations != value)
        {
          if (_locations != null)
          {
            _locations.ListChanged -= _locations_ListChanged;
          }
          _locations = value;

          _locations.ListChanged += _locations_ListChanged;

          updateNumberOfGridRows();
          updateAddSubToCategories();

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
          updateAddSubToCategories();
          NotifyPropertyChanged();
        }
      }
    }

    #endregion

    public ItemView()
    {
      InitializeComponent();
      DataContext = this;
    }

    protected void updateNumberOfGridRows()
    {
      // add/remove row definitions
      int numberOfRowDefinitions = (locations.Count + 1) / 2 + 1;
      while (wndPanel.RowDefinitions.Count < numberOfRowDefinitions)
      {
        wndPanel.RowDefinitions.Add(new RowDefinition());
      }
      while (wndPanel.RowDefinitions.Count > numberOfRowDefinitions)
      {
        wndPanel.RowDefinitions.RemoveAt(wndPanel.RowDefinitions.Count - 1);
      }

      for (int i = 0; i < wndPanel.RowDefinitions.Count - 1; i++)
      {
        wndPanel.RowDefinitions[i].Height = new GridLength(100);
      }
      wndPanel.RowDefinitions[wndPanel.RowDefinitions.Count - 1].Height = new GridLength(1, GridUnitType.Star);

      while (wndPanel.Children.Count < locations.Count)
      {
        var newItem = new Controls.AddSubToLocationOfItem();
        Grid.SetRow(newItem, wndPanel.Children.Count / 2);
        Grid.SetColumn(newItem, 2 * (wndPanel.Children.Count % 2));
        wndPanel.Children.Add(newItem);
      }
      while (wndPanel.Children.Count > locations.Count)
      {
        wndPanel.Children.RemoveAt(wndPanel.Children.Count - 1);
      }
    }

    protected void updateAddSubToCategories()
    {
      if ((item == null) || (locations == null) || (entries == null) || (Data.DataContainer.localConnection == null))
      {
        return;
      }

      for (int i = 0; i < locations.Count; i++)
      {
        // check if we have a entry
        var foundEntries = from e in entries
                           where e.item == item
                           where e.location == locations[i]
                           select e;

        if (foundEntries.Count() == 0)
        {
          var newEntry = Data.Entry.createDelayed(Data.DataContainer.localConnection, item, locations[i]);
          (wndPanel.Children[i] as Controls.AddSubToLocationOfItem).entry = newEntry;
          item.entriesOfItem.Add(newEntry);
          _entries.Add(newEntry);
        }
        else
        {
          (wndPanel.Children[i] as Controls.AddSubToLocationOfItem).entry = foundEntries.First();
        }
      }
    }

    private void _locations_ListChanged(object sender, ListChangedEventArgs e)
    {
      updateNumberOfGridRows();
      updateAddSubToCategories();
    }

    private void wndImage_Click(Data.Item item)
    {
      OpenFileDialog dlg = new OpenFileDialog();
      dlg.DefaultExt = ".png"; // Default file extension
      dlg.Filter = "Portable Network Graphics (.png)|*.png";

      if (dlg.ShowDialog() == true)
      {
        item.setImage(dlg.FileName);
      }
    }
  }
}
