using MySql.Data.MySqlClient;
using System;
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
using System.Windows.Shapes;

namespace FridgeManagement.Windows {
  /// <summary>
  /// Interaktionslogik für NewLocationWindow.xaml
  /// </summary>
  public partial class NewLocationWindow : Window {
public Data.Location newItem { get; protected set; } = null;

    public NewLocationWindow()
    {
      InitializeComponent();
    }

    private void okClicked(object sender, RoutedEventArgs e)
    {
      try
      {
        newItem = Data.Location.createAndAddItem(Data.DataContainer.localConnection, wndName.Text);
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
