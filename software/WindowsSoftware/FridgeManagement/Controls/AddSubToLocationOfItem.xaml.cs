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
  /// Interaktionslogik für AddSubElement.xaml
  /// </summary>
  public partial class AddSubToLocationOfItem : UserControl, INotifyPropertyChanged {
    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
    #endregion

    Data.Entry _entry = null;

    public Data.Entry entry {
      get => _entry;
      set {
        if (_entry != value)
        {
          _entry = value;
          NotifyPropertyChanged();
        }
      }
    }

    public AddSubToLocationOfItem()
    {
      InitializeComponent();
      DataContext = this;
    }

    private void AddClick(object sender, RoutedEventArgs e)
    {
      entry.numberOfItems++;
    }

    private void SubClick(object sender, RoutedEventArgs e)
    {
      if(entry.numberOfItems != 0)
      {
        entry.numberOfItems--;
      }
    }
  }
}
