using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FridgeManagement.Views {
  public abstract class UserControlWithTitle : UserControl, INotifyPropertyChanged {
    protected string _Title = "";
    public string Title {
      get => _Title;
      set {
        if (_Title != value)
        {
          _Title = value;
          NotifyPropertyChanged();
        }
      }
    }

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
  }
}
