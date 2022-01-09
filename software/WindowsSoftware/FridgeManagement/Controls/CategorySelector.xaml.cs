using System;
using System.ComponentModel;
using System.Collections.Generic;
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

namespace FridgeManagement.Controls {
  /// <summary>
  /// Implementation of User Control displaying the categories
  /// </summary>
  public partial class CategorySelector : UserControl {
    /// <summary>
    /// Event Type if a category was selected
    /// </summary>
    /// <param name="category">the selected category</param>
    public delegate void CategorySelectedEventHandler(Data.Category category);

    /// <summary>
    /// Internal storage of categories
    /// </summary>
    private BindingList<Data.Category> _categories = new BindingList<Data.Category>();

    /// <summary>
    /// The categories in this control
    /// </summary>
    public BindingList<Data.Category> categories {
      get => _categories;
      set {
        // deregister old change handler
        _categories.ListChanged -= Categories_ListChanged;

        _categories = value;

        // register new change handler
        _categories.ListChanged += Categories_ListChanged;

        // recreate buttons
        wndGrid.Children.Clear();
        wndGrid.Columns = value.Count+1;
        wndGrid.Children.Add(createButton(new Data.Category(null, 0, "Alle")));
        foreach (Data.Category c in value) {
          wndGrid.Children.Add(createButton(c));
        }
      }
    }

    /// <summary>
    /// event triggered in case a category was selected
    /// </summary>
    public event CategorySelectedEventHandler CategorySelected;

    public CategorySelector() {
      InitializeComponent();

      categories.ListChanged += Categories_ListChanged;
    }

    private Button createButton(Data.Category category) {
      Button btn = new Button();
      btn.DataContext = category;
      btn.SetBinding(Button.ContentProperty, new Binding("name"));
      btn.Margin = new Thickness(5);
      btn.Click += Btn_Click;
      return btn;
    }

    private void Categories_ListChanged(object sender, ListChangedEventArgs e) {
      switch (e.ListChangedType) {
        case ListChangedType.ItemAdded: {
            // New category. Just add the last category
            wndGrid.Columns = categories.Count + 1;
            wndGrid.Children.Insert(e.NewIndex + 1, createButton(categories[e.NewIndex]));
            break;
          }
        case ListChangedType.ItemDeleted: {
            wndGrid.Columns = categories.Count + 1;
            wndGrid.Children.RemoveAt(e.NewIndex + 1);
            break;
          }
        case ListChangedType.ItemChanged: {
            // Nothing to do, handled by bindings of buttons
            break;
          }
        case ListChangedType.ItemMoved: {
            throw new NotImplementedException();
          }
        case ListChangedType.Reset: {
            if(wndGrid.Children.Count > 1)
            {
              wndGrid.Children.RemoveRange(1, wndGrid.Children.Count - 1);
              wndGrid.Columns = categories.Count + 1;
            };
            break;
          }
      }
    }

    /// <summary>
    /// Callback if a button was clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Btn_Click(object sender, RoutedEventArgs e) {
      /// forward call
      CategorySelected((sender as Button).DataContext as Data.Category);
    }


  }
}
