using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

namespace FridgeManagement.Windows {
  /// <summary>
  /// Interaktionslogik für NewItem.xaml
  /// </summary>
  public partial class NewItemWindow : Window, INotifyPropertyChanged {
    private BindingList<Data.Category> _categories;
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

    public Data.Item newItem { get; protected set; } = null;

    public NewItemWindow(BindingList<Data.Category> categories)
    {
      InitializeComponent();
      _categories = categories;
      DataContext = this;

      wndCategory.SelectedIndex = 0;
    }
    private void btnSearch_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog dlg = new OpenFileDialog();
      dlg.FileName = wndImgPath.Text; // Default file name
      dlg.DefaultExt = ".png"; // Default file extension
      dlg.Filter = "Portable Network Graphics (.png)|*.png";

      if (dlg.ShowDialog() == true)
      {
        wndImgPath.Text = dlg.FileName;
        var rawData = File.ReadAllBytes(dlg.FileName);
        MemoryStream strm = new MemoryStream(rawData);
        wndPreview.Source = new PngBitmapDecoder(strm, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default).Frames[0];
        if (rawData.Length > 1024 * 1024)
        {
          lblWarning.Content = "Warning: Image has " + (rawData.Length / 1024).ToString() + "kB.";
          lblWarning.Visibility = Visibility.Visible;
        }
        else
        {
          lblWarning.Visibility = Visibility.Collapsed;
        }
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

    private void okClicked(object sender, RoutedEventArgs e)
    {
      try
      {
        newItem = Data.Item.createAndAddItem(Data.DataContainer.localConnection, wndName.Text, wndCategory.SelectedItem as Data.Category, wndImgPath.Text);
        DialogResult = true; 
        this.Close();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    private void cancelClicked(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close();
    }
  }
}
