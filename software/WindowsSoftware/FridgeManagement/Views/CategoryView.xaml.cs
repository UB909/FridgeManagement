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

namespace FridgeManagement.Views {
  /// <summary>
  /// Interaktionslogik für CategoryView.xaml
  /// </summary>
  public partial class CategoryView : UserControlWithTitle {
    #region Data
    protected Data.Category _category;
    protected BindingList<Data.Item> _items = null;
    #endregion

    #region Public Properties
    public Data.Category category {
      get => _category;
      set {
        if (_category != value)
        {
          _category = value;
          Title = "Kategorie " + category.name;
          updateVisibility();
          NotifyPropertyChanged();
        }
      }
    }

    public BindingList<Data.Item> items {
      get => _items;
      set {
        if (_items != value)
        {
          if(_items != null)
          {
            _items.ListChanged -= _items_ListChanged;
          }
          _items = value;
          
          _items.ListChanged += _items_ListChanged;

          wndPanel.Children.Clear();
          foreach (var item in items)
          {
            wndPanel.Children.Add(createItem(item));
          }
          updateVisibility();
          NotifyPropertyChanged();
        }
      }
    }
    #endregion

    public delegate void ItemSelectHandler(Data.Item item);
    public event ItemSelectHandler ItemSelect;

    public CategoryView()
    {
      InitializeComponent();
      wndPanel.Children.Clear();
      category = new Data.Category(Data.DataContainer.localConnection, 0, "Alle");
    }

    protected void updateVisibility()
    {
      foreach (var i in wndPanel.Children)
      {
        Controls.Item item = i as Controls.Item;
        if(i == null)
        {
          continue;
        }

        // Is category or is all selected?
        if (item.item.category == category || category.id == 0)
        {
          item.Visibility = Visibility.Visible;
        }
        else
        {
          item.Visibility = Visibility.Collapsed;
        }
      }
    }

    protected Controls.Item createItem(Data.Item item)
    {
      Controls.Item newItem = new Controls.Item();
      newItem.item = item;
      newItem.numberOfItems = 0;
      foreach(var entry in item.entriesOfItem)
      {
        newItem.numberOfItems += entry.numberOfItems;
      }
      item.entriesOfItem.ListChanged += (sender, e) => EntriesOfItem_ListChanged(item, newItem); 
      newItem.Margin = new Thickness(4);
      newItem.Height = 200;
      newItem.Width = 200;
      newItem.Click += ItemSelected;
      return newItem;
    }

    private void EntriesOfItem_ListChanged(Data.Item item, Controls.Item itemControl)
    {
      itemControl.numberOfItems = 0;
      foreach (var entry in item.entriesOfItem)
      {
        itemControl.numberOfItems += entry.numberOfItems;
      }
    }

    private void ItemSelected(Data.Item item)
    {
      ItemSelect(item);
    }

    private void _items_ListChanged(object sender, ListChangedEventArgs e)
    {
      switch (e.ListChangedType)
      {
        case ListChangedType.ItemAdded:
          {
            // New category. Just add the last category
            wndPanel.Children.Insert(e.NewIndex, createItem(_items[e.NewIndex]));
            updateVisibility();
            break;
          }
        case ListChangedType.ItemDeleted:
          {
            wndPanel.Children.RemoveAt(e.NewIndex);
            break;
          }
        case ListChangedType.ItemChanged:
          {
            // Nothing to do, handled by bindings of buttons
            break;
          }
        case ListChangedType.ItemMoved:
          {
            throw new NotImplementedException();
          }
        case ListChangedType.Reset:
          {
            wndPanel.Children.Clear();
            break;
          }
      }
    }
  }
}
