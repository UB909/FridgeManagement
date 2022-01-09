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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FridgeManagement.Controls {
  /// <summary>
  /// Interaktionslogik für TrafficLight.xaml
  /// </summary>
  public partial class TrafficLight : UserControl {
    public enum State {
      off, green, yellow, red
    };

    private State _state = State.off;
    public State state {
      get => _state;
      set {
        _state = value;
        switch (_state) {
          case State.off:
            grey.Visibility = Visibility.Visible;
            red.Visibility = Visibility.Collapsed;
            yellow.Visibility = Visibility.Collapsed;
            green.Visibility = Visibility.Collapsed;
            break;
          case State.red:
            grey.Visibility = Visibility.Collapsed;
            red.Visibility = Visibility.Visible;
            yellow.Visibility = Visibility.Collapsed;
            green.Visibility = Visibility.Collapsed;
            break;
          case State.yellow:
            grey.Visibility = Visibility.Collapsed;
            red.Visibility = Visibility.Collapsed;
            yellow.Visibility = Visibility.Visible;
            green.Visibility = Visibility.Collapsed;
            break;
          case State.green:
            grey.Visibility = Visibility.Collapsed;
            red.Visibility = Visibility.Collapsed;
            yellow.Visibility = Visibility.Collapsed;
            green.Visibility = Visibility.Visible;
            break;
        }
      }
      }

    public TrafficLight() {
      InitializeComponent();
    }
  }
}
